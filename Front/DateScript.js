/* InsureX — CRUD frontend
 * FK-поля: в UI — понятные подписи (VIN, ФИО, номер полиса),
 * в API/БД — по-прежнему числовые ID.
 */

var API_BASE = "http://localhost:5000";

var currentController = "";
var modalMode = "add";
var editItemId = null;
var currentKeys = [];
var currentData = [];

/* Кэш справочников: controller → массив записей */
var refCache = {};

/* ---------- Схемы ----------
 * type: text|number|date|email|tel|ref
 * ref: { controller, idKey, label(item) → строка для select/таблицы }
 */
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
      {
        name: "PoliciesID",
        label: "Полис",
        type: "ref",
        ref: {
          controller: "PoliciesContrl",
          idKey: "PoliciesID",
          label: function (p) {
            return (
              (pick(p, "PolicyNumber") || "Полис") +
              " · id " +
              pick(p, "PoliciesID")
            );
          }
        }
      },
      {
        name: "VehiclesID",
        label: "Транспорт (VIN)",
        type: "ref",
        ref: {
          controller: "VehiclesContrl",
          idKey: "VehiclesID",
          label: function (v) {
            var vin = pick(v, "Vin") || "—";
            var make = pick(v, "Make") || "";
            var model = pick(v, "Model") || "";
            var year = pick(v, "Year") || "";
            return vin + " — " + make + " " + model + " " + year;
          }
        }
      }
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
      {
        name: "InsuranceTypesID",
        label: "Тип страховки",
        type: "ref",
        ref: {
          controller: "InsuranceTypesContrl",
          idKey: "InsuranceTypesID",
          label: function (t) {
            return pick(t, "Name") || "Тип #" + pick(t, "InsuranceTypesID");
          }
        }
      }
    ]
  },
  InsuranceTypesContrl: {
    label: "Типы страховок",
    idKey: "InsuranceTypesID",
    fields: [
      {
        name: "Name",
        label: "Название",
        type: "text",
        placeholder: "Например: Личное Страхование или ОСАГО"
      }
    ]
  },
  PaymentsContrl: {
    label: "Платежи",
    idKey: "PaymentsID",
    fields: [
      {
        name: "PoliciesID",
        label: "Полис",
        type: "ref",
        ref: {
          controller: "PoliciesContrl",
          idKey: "PoliciesID",
          label: function (p) {
            return (
              (pick(p, "PolicyNumber") || "Полис") +
              " · id " +
              pick(p, "PoliciesID")
            );
          }
        }
      },
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
      {
        name: "ClientsID",
        label: "Клиент",
        type: "ref",
        ref: {
          controller: "ClientsContrl",
          idKey: "ClientsID",
          label: function (c) {
            return (
              (pick(c, "LastName") || "") +
              " " +
              (pick(c, "FirstName") || "") +
              " · " +
              (pick(c, "Email") || "")
            ).trim();
          }
        }
      },
      {
        name: "InsuranceID",
        label: "Страховка",
        type: "ref",
        ref: {
          controller: "InsuranceContrl",
          idKey: "InsuranceID",
          label: function (i) {
            return pick(i, "Name") || "Страховка #" + pick(i, "InsuranceID");
          }
        }
      },
      {
        name: "AgentsID",
        label: "Агент",
        type: "ref",
        ref: {
          controller: "AgentsContrl",
          idKey: "AgentsID",
          label: function (a) {
            return (
              (pick(a, "LastName") || "") +
              " " +
              (pick(a, "FirstName") || "") +
              " · " +
              (pick(a, "Email") || "")
            ).trim();
          }
        }
      },
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

/** Поле из объекта с учётом camelCase ответа API */
function pick(item, name) {
  if (!item || name == null) return null;
  if (item[name] != null) return item[name];
  var camel = name.charAt(0).toLowerCase() + name.slice(1);
  if (item[camel] != null) return item[camel];
  return null;
}

function parseValue(raw, type) {
  if (raw === "" || raw == null) return null;
  var s = String(raw).trim();
  if (s === "") return null;
  if (type === "number" || type === "ref") {
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

function collectBodyFromForm(schema) {
  var body = {};
  document.querySelectorAll("#modalFields [data-key]").forEach(function (el) {
    var key = el.dataset.key;
    if (key === schema.idKey) return;
    var fieldDef = schema.fields.find(function (f) {
      return f.name === key;
    });
    var type = fieldDef ? fieldDef.type : "text";
    body[key] = parseValue(el.value, type);
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

/* ===================== API ===================== */
async function api(path, method, body, rawBody) {
  method = method || "GET";
  var token = getCookieVal("accessToken") || accessToken;

  var opts = {
    method: method,
    headers: { Accept: "application/json" }
  };
  if (token) opts.headers["Authorization"] = "Bearer " + token;

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

/* ===================== Справочники (FK) ===================== */
async function loadRefList(controller, force) {
  if (!force && refCache[controller]) return refCache[controller];
  var data = await api(controller, "GET");
  if (data === null) {
    refCache[controller] = [];
    return [];
  }
  var list = Array.isArray(data) ? data : [];
  refCache[controller] = list;
  return list;
}

async function ensureRefsForSchema(schema) {
  if (!schema) return;
  var tasks = [];
  schema.fields.forEach(function (f) {
    if (f.type === "ref" && f.ref) {
      tasks.push(loadRefList(f.ref.controller));
    }
  });
  await Promise.all(tasks);
}

function resolveRefLabel(fieldDef, idVal) {
  if (idVal == null || idVal === "") return "";
  if (!fieldDef || fieldDef.type !== "ref" || !fieldDef.ref) return String(idVal);
  var list = refCache[fieldDef.ref.controller] || [];
  var idKey = fieldDef.ref.idKey;
  for (var i = 0; i < list.length; i++) {
    var row = list[i];
    var rid = pick(row, idKey);
    if (String(rid) === String(idVal)) {
      try {
        return fieldDef.ref.label(row);
      } catch (e) {
        return String(idVal);
      }
    }
  }
  return "#" + idVal;
}

function columnTitle(key, schema) {
  if (!schema) return key;
  if (key === schema.idKey) return "№";
  var f = schema.fields.find(function (x) {
    return x.name === key;
  });
  return f ? f.label : key;
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

  var schema = SCHEMAS[controller];
  // заранее подтянуть справочники для красивых подписей в таблице
  await ensureRefsForSchema(schema);

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
    currentKeys = Object.keys(data[0]);
  } else if (schema) {
    currentKeys = [schema.idKey].concat(
      schema.fields.map(function (f) {
        return f.name;
      })
    );
  }

  var hdr = document.createElement("div");
  hdr.className = "section-header";

  var title = document.createElement("h2");
  title.className = "section-title";
  title.textContent = LABELS[currentController] || currentController;

  var canEdit =
    typeof canModifyEntity !== "function" || canModifyEntity(currentController);

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

  function resolveId(item) {
    var v = pick(item, idKey);
    if (v != null) return v;
    return item[currentKeys[0]];
  }

  var table = document.createElement("div");
  table.className = "table-wrap";

  var html = "<table><thead><tr><th>Действия</th>";
  currentKeys.forEach(function (k) {
    html += "<th>" + escapeHtml(columnTitle(k, schema)) + "</th>";
  });
  html += "</tr></thead><tbody>";

  data.forEach(function (item, index) {
    var idVal = resolveId(item);
    html += "<tr><td class='td-actions'>";
    if (canEdit) {
      html +=
        '<button class="btn-edit" data-index="' + index + '">Изменить</button> ';
      html +=
        '<button class="btn-delete" data-id="' +
        escapeAttr(String(idVal)) +
        '">Удалить</button>';
    } else {
      html += '<span style="color:var(--text-muted);font-size:12px;">—</span>';
    }
    html += "</td>";
    currentKeys.forEach(function (k) {
      var val = pick(item, k);
      var fieldDef =
        schema &&
        schema.fields.find(function (f) {
          return f.name === k;
        });
      var display;
      if (fieldDef && fieldDef.type === "ref") {
        display = resolveRefLabel(fieldDef, val);
      } else {
        display = formatCell(val);
      }
      html += "<td>" + escapeHtml(display) + "</td>";
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


async function doDelete(id) {
  if (
    typeof canModifyEntity === "function" &&
    !canModifyEntity(currentController)
  ) {
    alert("Нет прав на удаление");
    return;
  }
  if (!confirm("Удалить запись #" + id + "?")) return;
  var result = await api(
    currentController + "?id=" + encodeURIComponent(id),
    "DELETE"
  );
  if (result !== null) {

    delete refCache[currentController];
    loadEntity(currentController);
  }
}


function makeInputField(name, label, type, value, placeholder) {
  var div = document.createElement("div");
  div.className = "field";

  var lbl = document.createElement("label");
  lbl.textContent = label || name;

  var inp = document.createElement("input");
  inp.dataset.key = name;
  inp.placeholder = placeholder || label || name;

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

function makeRefSelect(fieldDef, selectedId) {
  var div = document.createElement("div");
  div.className = "field";

  var lbl = document.createElement("label");
  lbl.textContent = fieldDef.label || fieldDef.name;

  var sel = document.createElement("select");
  sel.dataset.key = fieldDef.name;
  sel.style.cssText =
    "width:100%;padding:10px 12px;border-radius:8px;border:1px solid var(--border);background:var(--surface2);color:var(--text);font-family:var(--font-body);font-size:14px;";

  var opt0 = document.createElement("option");
  opt0.value = "";
  opt0.textContent = "— выберите —";
  sel.appendChild(opt0);

  var list = refCache[fieldDef.ref.controller] || [];
  var idKey = fieldDef.ref.idKey;
  list.forEach(function (row) {
    var id = pick(row, idKey);
    if (id == null) return;
    var opt = document.createElement("option");
    opt.value = String(id);
    try {
      opt.textContent = fieldDef.ref.label(row);
    } catch (e) {
      opt.textContent = String(id);
    }
    if (String(id) === String(selectedId)) opt.selected = true;
    sel.appendChild(opt);
  });

  if (list.length === 0) {
    var empty = document.createElement("option");
    empty.disabled = true;
    empty.textContent = "Нет записей в справочнике — сначала заполните раздел";
    sel.appendChild(empty);
  }

  div.appendChild(lbl);
  div.appendChild(sel);
  return div;
}

async function openAddModal() {
  if (!currentController) {
    alert("Сначала выберите раздел");
    return;
  }
  if (
    typeof canModifyEntity === "function" &&
    !canModifyEntity(currentController)
  ) {
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
    await ensureRefsForSchema(schema);
    schema.fields.forEach(function (f) {
      if (f.type === "ref") {
        fields.appendChild(makeRefSelect(f, null));
      } else {
        fields.appendChild(
          makeInputField(f.name, f.label, f.type, "", f.placeholder)
        );
      }
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

async function openEditModalByIndex(index) {
  if (
    typeof canModifyEntity === "function" &&
    !canModifyEntity(currentController)
  ) {
    alert("Нет прав на изменение");
    return;
  }
  var item = currentData[index];
  if (!item) return;

  var schema = SCHEMAS[currentController];
  var idKey = getIdKey(currentController);

  modalMode = "edit";
  editItemId = pick(item, idKey);
  if (editItemId == null) editItemId = item[currentKeys[0]];

  document.getElementById("modalTitle").textContent =
    "Изменить запись #" + editItemId;

  var fieldsEl = document.getElementById("modalFields");
  fieldsEl.innerHTML = "";

  if (schema) {
    await ensureRefsForSchema(schema);

    var idField = makeInputField(idKey, "№", "number", editItemId);
    idField.querySelector("input").readOnly = true;
    idField.querySelector("input").style.opacity = "0.6";
    fieldsEl.appendChild(idField);

    schema.fields.forEach(function (f) {
      var val = pick(item, f.name);
      if (f.type === "ref") {
        fieldsEl.appendChild(makeRefSelect(f, val));
      } else {
        fieldsEl.appendChild(
          makeInputField(f.name, f.label, f.type, val, f.placeholder)
        );
      }
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
  if (
    typeof canModifyEntity === "function" &&
    !canModifyEntity(currentController)
  ) {
    alert("Нет прав на сохранение");
    return;
  }
  var schema = SCHEMAS[currentController];
  var body;

  if (schema) {
    body = collectBodyFromForm(schema);
  } else {
    body = {};
    document.querySelectorAll("#modalFields [data-key]").forEach(function (el) {
      if (el.readOnly) return;
      var v = el.value;
      if (v === "") body[el.dataset.key] = null;
      else if (!isNaN(v) && String(v).trim() !== "")
        body[el.dataset.key] = Number(v);
      else body[el.dataset.key] = v;
    });
  }

  if (Object.keys(body).length === 0) {
    alert("Нет полей для сохранения");
    return;
  }


  if (schema) {
    for (var i = 0; i < schema.fields.length; i++) {
      var f = schema.fields[i];
      if (f.type === "ref" && (body[f.name] == null || body[f.name] === "")) {
        alert("Выберите значение: " + f.label);
        return;
      }
    }
  }

  var result;
  if (modalMode === "add") {
    result = await api(currentController, "POST", body);
  } else {
    result = await api(
      currentController + "?id=" + encodeURIComponent(editItemId),
      "PUT",
      body
    );
  }

  if (result === null) return;

  delete refCache[currentController];
  closeModal();
  loadEntity(currentController);
}

window.openAddModal = openAddModal;
window.openEditModalByIndex = openEditModalByIndex;
window.doDelete = doDelete;

document.getElementById("modalSaveBtn").onclick = saveModal;
document.getElementById("modalCloseBtn").onclick = closeModal;
document.getElementById("modalCancelBtn").onclick = closeModal;
