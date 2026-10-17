/* InsureX — CRUD frontend
 * Под реальное API:
 *   GET    /{Controller}
 *   GET    /{Controller}/{id}
 *   POST   /{Controller}           body: JSON entity
 *   PUT    /{Controller}?id={id}   body: JSON entity  (id из query!)
 *   DELETE /{Controller}?id={id}   (id из query!)
 *   POST   /api/auth/login         { email, password }
 *   POST   /api/auth/refresh       body: "refreshTokenString"  (сырая JSON-строка)
 */

var API_BASE = "http://localhost:5000";

var currentController = "";
var modalMode = "add";
var editItemId = null;
var currentKeys = [];
var currentData = [];

/* Схемы по моделям Common/DB — имена свойств PascalCase как в C# */
var SCHEMAS = {
  AgentsContrl: {
    label: "Агенты",
    idKey: "AgentsID",
    fields: [
      { name: "FirstName", label: "Имя", type: "text" },
      { name: "LastName", label: "Фамилия", type: "text" },
      { name: "Phone", label: "Телефон (11 цифр)", type: "tel" },
      { name: "Email", label: "Email", type: "email" },
      { name: "HireDate", label: "Дата найма", type: "date" }
    ]
  },
  AgreementsVehiclesContrl: {
    label: "Договоры ТС",
    idKey: "AgreementsVehiclesID",
    fields: [
      { name: "PoliciesID", label: "ID полиса", type: "number" },
      { name: "VehiclesID", label: "ID транспорта", type: "number" }
    ]
  },
  ClientsContrl: {
    label: "Клиенты",
    idKey: "ClientsID",
    fields: [
      { name: "FirstName", label: "Имя", type: "text" },
      { name: "LastName", label: "Фамилия", type: "text" },
      { name: "BirthDate", label: "Дата рождения", type: "date" },
      { name: "Passport", label: "Паспорт", type: "text" },
      { name: "Phone", label: "Телефон (11 цифр)", type: "tel" },
      { name: "Email", label: "Email", type: "email" }
    ]
  },
  InsuranceContrl: {
    label: "Страховки",
    idKey: "InsuranceID",
    fields: [
      { name: "Name", label: "Название", type: "text" },
      { name: "Description", label: "Описание", type: "text" },
      { name: "Terms", label: "Условия", type: "text" },
      { name: "Tariff", label: "Тариф", type: "number" },
      { name: "InsuranceTypesID", label: "ID типа страховки", type: "number" }
    ]
  },
  InsuranceTypesContrl: {
    label: "Типы страховок",
    idKey: "InsuranceTypesID",
    fields: [
      { name: "Name", label: "Название", type: "text" }
    ]
  },
  PaymentsContrl: {
    label: "Платежи",
    idKey: "PaymentsID",
    fields: [
      { name: "PoliciesID", label: "ID полиса", type: "number" },
      { name: "PaymentDate", label: "Дата платежа", type: "date" },
      { name: "Amount", label: "Сумма", type: "number" },
      { name: "Method", label: "Способ оплаты", type: "text" },
      { name: "Reference", label: "Референс", type: "text" }
    ]
  },
  PoliciesContrl: {
    label: "Полисы",
    idKey: "PoliciesID",
    fields: [
      { name: "PolicyNumber", label: "Номер полиса", type: "text" },
      { name: "ClientsID", label: "ID клиента", type: "number" },
      { name: "InsuranceID", label: "ID страховки", type: "number" },
      { name: "AgentsID", label: "ID агента", type: "number" },
      { name: "StartDate", label: "Дата начала", type: "date" },
      { name: "EndDate", label: "Дата окончания", type: "date" },
      { name: "SumInsured", label: "Страховая сумма", type: "number" },
      { name: "Premium", label: "Премия", type: "number" },
      { name: "Status", label: "Статус", type: "text" }
    ]
  },
  VehiclesContrl: {
    label: "Транспорт",
    idKey: "VehiclesID",
    fields: [
      { name: "Vin", label: "VIN", type: "text" },
      { name: "Make", label: "Марка", type: "text" },
      { name: "Model", label: "Модель", type: "text" },
      { name: "Year", label: "Год", type: "number" }
    ]
  }
};

var LABELS = {};
Object.keys(SCHEMAS).forEach(function (k) {
  LABELS[k] = SCHEMAS[k].label;
});

function parseValue(raw, type) {
  if (raw === "" || raw == null) return null;
  var s = String(raw).trim();
  if (s === "") return null;

  if (type === "number") {
    var n = Number(s.replace(",", "."));
    return isNaN(n) ? null : n;
  }
  if (type === "date") {
    if (/^\d{4}-\d{2}-\d{2}$/.test(s)) {
      return new Date(s + "T00:00:00").toISOString();
    }
    var d = Date.parse(s);
    return isNaN(d) ? s : new Date(d).toISOString();
  }
  if (s === "true") return true;
  if (s === "false") return false;
  return s;
}

function collectBodyFromForm(schema, includeId) {
  var body = {};
  document.querySelectorAll("#modalFields input[data-key]").forEach(function (inp) {
    var key = inp.dataset.key;
    if (!includeId && key === schema.idKey) return;
    var fieldDef = schema.fields.find(function (f) { return f.name === key; });
    var type = fieldDef ? fieldDef.type : (inp.type === "number" ? "number" : "text");
    body[key] = parseValue(inp.value, type);
  });
  return body;
}

function toDateInputValue(val) {
  if (val == null || val === "") return "";
  var d = new Date(val);
  if (isNaN(d.getTime())) {
    var m = String(val).match(/^(\d{4}-\d{2}-\d{2})/);
    return m ? m[1] : String(val);
  }
  var y = d.getFullYear();
  var mo = String(d.getMonth() + 1).padStart(2, "0");
  var day = String(d.getDate()).padStart(2, "0");
  return y + "-" + mo + "-" + day;
}

/* ===================== API =====================
 * body: object → JSON.stringify
 * для refresh передаём уже готовую JSON-строку через rawBody=true
 */
async function api(path, method, body, rawBody) {
  method = method || "GET";
  var token = getCookieVal("accessToken") || accessToken;

  var opts = {
    method: method,
    headers: {
      Accept: "application/json"
    }
  };
  if (token) {
    opts.headers["Authorization"] = "Bearer " + token;
  }

  if (body !== undefined && body !== null && method !== "GET") {
    opts.headers["Content-Type"] = "application/json";
    opts.body = rawBody ? body : JSON.stringify(body);
  }

  try {
    var r = await fetch(API_BASE + "/" + String(path).replace(/^\//, ""), opts);

    if (r.status === 401) {
      var rt = getCookieVal("refreshToken") || refreshToken;
      if (!rt) {
        document.getElementById("logoutBtn").onclick();
        return null;
      }
      try {
        // AuthController.Refresh([FromBody] string refreshToken)
        // → тело = JSON-строка: "the-token-value"
        var ref = await fetch(API_BASE + "/api/auth/refresh", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(rt)
        });
        if (ref.ok) {
          var rd = await ref.json();
          accessToken = rd.accessToken || rd.AccessToken || "";
          refreshToken = rd.refreshToken || rd.RefreshToken || rt;
          setCookie();
          return api(path, method, body, rawBody);
        }
      } catch (e) {}
      document.getElementById("logoutBtn").onclick();
      return null;
    }

    if (!r.ok) {
      var errText = "Ошибка сервера";
      try {
        var errBody = await r.text();
        if (errBody) errText += "\n" + errBody.substring(0, 400);
      } catch (e) {}
      alert(errText + " (статус " + r.status + ")");
      return null;
    }

    if (r.status === 204) return true;

    var ct = (r.headers.get("content-type") || "").toLowerCase();
    if (ct.indexOf("application/json") !== -1) {
      var text = await r.text();
      if (!text || !text.trim()) return true;
      try {
        return JSON.parse(text);
      } catch (e) {
        return true;
      }
    }
    var plain = await r.text();
    return plain && plain.trim() ? plain : true;
  } catch (e) {
    alert("Нет соединения с сервером\n" + (e.message || ""));
    return null;
  }
}

/* ===================== NAV ===================== */
document.querySelectorAll(".nav-btn").forEach(function (btn) {
  btn.onclick = function () {
    loadEntity(btn.dataset.entity);
  };
});

async function loadEntity(controller) {
  if (typeof canViewEntity === "function" && !canViewEntity(controller)) {
    alert("Нет доступа к этому разделу");
    return;
  }

  currentController = controller;

  document.querySelectorAll(".nav-btn").forEach(function (b) {
    b.classList.toggle("active", b.dataset.entity === controller);
  });

  var data = await api(controller, "GET");
  if (data === null) return;

  currentData = Array.isArray(data) ? data : [];
  renderTable(currentData);
}

function getIdKey(controller) {
  if (SCHEMAS[controller]) return SCHEMAS[controller].idKey;
  if (currentKeys.length) return currentKeys[0];
  return "id";
}

/* ===================== TABLE ===================== */
function renderTable(data) {
  var area = document.getElementById("contentArea");
  var schema = SCHEMAS[currentController];

  if (data.length > 0) {
    // API может отдать camelCase — нормализуем ключи для отображения
    currentKeys = Object.keys(data[0]);
  } else if (schema) {
    currentKeys = [schema.idKey].concat(schema.fields.map(function (f) { return f.name; }));
  }

  var hdr = document.createElement("div");
  hdr.className = "section-header";

  var title = document.createElement("h2");
  title.className = "section-title";
  title.textContent = LABELS[currentController] || currentController;

  var canEdit = typeof canModifyEntity !== "function" || canModifyEntity(currentController);

  if (canEdit) {
    var addBtn = document.createElement("button");
    addBtn.className = "btn-primary";
    addBtn.textContent = "+ Добавить";
    addBtn.onclick = openAddModal;
    hdr.appendChild(title);
    hdr.appendChild(addBtn);
  } else {
    hdr.appendChild(title);
    var ro = document.createElement("span");
    ro.style.cssText = "color:var(--text-muted);font-size:13px;";
    ro.textContent = "только просмотр";
    hdr.appendChild(ro);
  }

  area.innerHTML = "";
  area.appendChild(hdr);

  if (data.length === 0) {
    var empty = document.createElement("div");
    empty.className = "no-data";
    empty.textContent = "Нет данных — нажмите «+ Добавить»";
    area.appendChild(empty);
    return;
  }

  var idKey = getIdKey(currentController);
  // поддержка camelCase id из JSON (agentsID и т.п.)
  function resolveId(item) {
    if (item[idKey] != null) return item[idKey];
    var camel = idKey.charAt(0).toLowerCase() + idKey.slice(1);
    if (item[camel] != null) return item[camel];
    return item[currentKeys[0]];
  }

  var table = document.createElement("div");
  table.className = "table-wrap";

  var html = "<table><thead><tr><th>Действия</th>";
  currentKeys.forEach(function (k) {
    html += "<th>" + escapeHtml(k) + "</th>";
  });
  html += "</tr></thead><tbody>";

  data.forEach(function (item, index) {
    var idVal = resolveId(item);
    html += "<tr><td class='td-actions'>";
    if (canEdit) {
      html += '<button class="btn-edit" data-index="' + index + '">Изменить</button> ';
      html +=
        '<button class="btn-delete" data-id="' +
        escapeAttr(String(idVal)) +
        '">Удалить</button>';
    } else {
      html += '<span style="color:var(--text-muted);font-size:12px;">—</span>';
    }
    html += "</td>";
    currentKeys.forEach(function (k) {
      html += "<td>" + escapeHtml(formatCell(item[k])) + "</td>";
    });
    html += "</tr>";
  });
  html += "</tbody></table>";
  table.innerHTML = html;
  area.appendChild(table);

  table.querySelectorAll(".btn-edit").forEach(function (btn) {
    btn.onclick = function () {
      openEditModalByIndex(Number(btn.dataset.index));
    };
  });
  table.querySelectorAll(".btn-delete").forEach(function (btn) {
    btn.onclick = function () {
      doDelete(btn.dataset.id);
    };
  });
}

function formatCell(val) {
  if (val == null) return "";
  if (typeof val === "string" && /^\d{4}-\d{2}-\d{2}T/.test(val)) {
    try {
      return toDateInputValue(val);
    } catch (e) {
      return String(val);
    }
  }
  return String(val);
}

function escapeHtml(s) {
  return String(s)
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function escapeAttr(s) {
  return String(s).replace(/"/g, "&quot;").replace(/'/g, "&#39;");
}

/* ===================== DELETE =====================
 * API: [HttpDelete] public void Delete(int id)  →  ?id=
 */
async function doDelete(id) {
  if (typeof canModifyEntity === "function" && !canModifyEntity(currentController)) {
    alert("Нет прав на удаление");
    return;
  }
  if (!confirm("Удалить запись #" + id + "?")) return;
  var result = await api(currentController + "?id=" + encodeURIComponent(id), "DELETE");
  if (result !== null) {
    loadEntity(currentController);
  }
}

/* ===================== MODAL ===================== */
function makeInputField(name, label, type, value) {
  var div = document.createElement("div");
  div.className = "field";

  var lbl = document.createElement("label");
  lbl.textContent = label || name;

  var inp = document.createElement("input");
  inp.dataset.key = name;
  inp.placeholder = label || name;

  if (type === "number") {
    inp.type = "number";
    inp.step = "any";
    inp.value = value != null && value !== "" ? String(value) : "";
  } else if (type === "date") {
    inp.type = "date";
    inp.value = toDateInputValue(value);
  } else if (type === "email") {
    inp.type = "email";
    inp.value = value != null ? String(value) : "";
  } else if (type === "tel") {
    inp.type = "tel";
    inp.value = value != null ? String(value) : "";
  } else {
    inp.type = "text";
    inp.value = value != null ? String(value) : "";
  }

  div.appendChild(lbl);
  div.appendChild(inp);
  return div;
}

/** Значение поля из item с учётом camelCase ответа API */
function pickField(item, name) {
  if (item[name] != null) return item[name];
  var camel = name.charAt(0).toLowerCase() + name.slice(1);
  if (item[camel] != null) return item[camel];
  return null;
}

function openAddModal() {
  if (!currentController) {
    alert("Сначала выберите раздел");
    return;
  }
  if (typeof canModifyEntity === "function" && !canModifyEntity(currentController)) {
    alert("Нет прав на добавление в этот раздел");
    return;
  }

  modalMode = "add";
  editItemId = null;
  document.getElementById("modalTitle").textContent = "Добавить запись";

  var fields = document.getElementById("modalFields");
  fields.innerHTML = "";

  var schema = SCHEMAS[currentController];
  if (schema) {
    schema.fields.forEach(function (f) {
      fields.appendChild(makeInputField(f.name, f.label, f.type, ""));
    });
  } else if (currentKeys && currentKeys.length > 0) {
    currentKeys.slice(1).forEach(function (k) {
      fields.appendChild(makeInputField(k, k, "text", ""));
    });
  } else {
    fields.innerHTML =
      '<p style="color:#e8c97a;line-height:1.5">Структура полей неизвестна.</p>';
  }

  document.getElementById("modal").style.display = "flex";
}

function openEditModalByIndex(index) {
  if (typeof canModifyEntity === "function" && !canModifyEntity(currentController)) {
    alert("Нет прав на изменение");
    return;
  }
  var item = currentData[index];
  if (!item) return;

  var schema = SCHEMAS[currentController];
  var idKey = getIdKey(currentController);

  modalMode = "edit";
  editItemId = pickField(item, idKey);
  if (editItemId == null) editItemId = item[currentKeys[0]];

  document.getElementById("modalTitle").textContent = "Изменить запись #" + editItemId;

  var fieldsEl = document.getElementById("modalFields");
  fieldsEl.innerHTML = "";

  if (schema) {
    var idField = makeInputField(idKey, idKey, "number", editItemId);
    idField.querySelector("input").readOnly = true;
    idField.querySelector("input").style.opacity = "0.6";
    fieldsEl.appendChild(idField);

    schema.fields.forEach(function (f) {
      fieldsEl.appendChild(makeInputField(f.name, f.label, f.type, pickField(item, f.name)));
    });
  } else {
    currentKeys.forEach(function (k) {
      var inp = makeInputField(k, k, "text", item[k]);
      if (k === idKey || k === currentKeys[0]) {
        inp.querySelector("input").readOnly = true;
        inp.querySelector("input").style.opacity = "0.6";
      }
      fieldsEl.appendChild(inp);
    });
  }

  document.getElementById("modal").style.display = "flex";
}

function closeModal() {
  document.getElementById("modal").style.display = "none";
}

async function saveModal() {
  if (typeof canModifyEntity === "function" && !canModifyEntity(currentController)) {
    alert("Нет прав на сохранение");
    return;
  }
  var schema = SCHEMAS[currentController];
  var body;

  if (schema) {
    body = collectBodyFromForm(schema, false); // id не в body
  } else {
    body = {};
    document.querySelectorAll("#modalFields input[data-key]").forEach(function (inp) {
      var v = inp.value;
      if (inp.readOnly) return;
      if (v === "") body[inp.dataset.key] = null;
      else if (!isNaN(v) && String(v).trim() !== "") body[inp.dataset.key] = Number(v);
      else body[inp.dataset.key] = v;
    });
  }

  if (Object.keys(body).length === 0) {
    alert("Нет полей для сохранения");
    return;
  }

  var result;
  if (modalMode === "add") {
    // POST /Controller  body JSON
    result = await api(currentController, "POST", body);
  } else {
    // PUT /Controller?id=N  body JSON  (как в API: Put(int id, T item))
    result = await api(
      currentController + "?id=" + encodeURIComponent(editItemId),
      "PUT",
      body
    );
  }

  if (result === null) return;

  closeModal();
  loadEntity(currentController);
}

window.openAddModal = openAddModal;
window.openEditModalByIndex = openEditModalByIndex;
window.doDelete = doDelete;

document.getElementById("modalSaveBtn").onclick = saveModal;
document.getElementById("modalCloseBtn").onclick = closeModal;
document.getElementById("modalCancelBtn").onclick = closeModal;
