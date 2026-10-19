var accessToken = "";
var refreshToken = "";
var currentRole = ""; 


function setCookie() {
  var exp = new Date(Date.now() + 3600000).toUTCString();
  document.cookie = "accessToken=" + encodeURIComponent(accessToken) + "; expires=" + exp + "; path=/";
  document.cookie = "refreshToken=" + encodeURIComponent(refreshToken) + "; expires=" + exp + "; path=/";
  document.cookie = "userRole=" + encodeURIComponent(currentRole) + "; expires=" + exp + "; path=/";
}

function getCookieVal(name) {
  var parts = document.cookie.split(";");
  for (var i = 0; i < parts.length; i++) {
    var p = parts[i].trim().split("=");
    if (p[0] === name) return decodeURIComponent(p.slice(1).join("=") || "");
  }
  return "";
}

function clearCookies() {
  document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
  document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
  document.cookie = "userRole=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
}


function parseJwtPayload(token) {
  try {
    var parts = token.split(".");
    if (parts.length < 2) return null;
    var base64 = parts[1].replace(/-/g, "+").replace(/_/g, "/");
    while (base64.length % 4) base64 += "=";
    var json = decodeURIComponent(
      atob(base64)
        .split("")
        .map(function (c) {
          return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join("")
    );
    return JSON.parse(json);
  } catch (e) {
    return null;
  }
}

function extractRoleFromToken(token) {
  var payload = parseJwtPayload(token);
  if (!payload) return "";

  var role =
    payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
    payload.role ||
    payload.Role ||
    "";
  if (Array.isArray(role)) role = role[0] || "";
  return String(role).toLowerCase().trim();
}

function setTokensFromLogin(d) {
  accessToken = d.accessToken || d.AccessToken || "";
  refreshToken = d.refreshToken || d.RefreshToken || "";
  currentRole = extractRoleFromToken(accessToken);
  setCookie();
}


function isAdmin() {
  return currentRole === "admin";
}

function isManager() {
  return currentRole === "manager";
}


function canViewEntity(controller) {
  if (isAdmin()) return true;
  if (isManager()) {
    if (controller === "AgentsContrl") return false;
    if (controller === "InsuranceTypesContrl") return false;
    return true;
  }

  if (controller === "AgentsContrl" || controller === "InsuranceTypesContrl") return false;
  return true;
}


function canModifyEntity(controller) {
  if (isAdmin()) return true;
  if (isManager()) {

    if (controller === "InsuranceContrl") return false;
    if (controller === "AgentsContrl") return false;
    if (controller === "InsuranceTypesContrl") return false;
    return true;
  }
  return false;
}

function applyRoleToNav() {
  document.querySelectorAll(".nav-btn").forEach(function (btn) {
    var entity = btn.dataset.entity;
    if (canViewEntity(entity)) {
      btn.style.display = "";
    } else {
      btn.style.display = "none";
    }
  });
}


function showApp() {
  document.getElementById("loginPage").style.display = "none";
  document.getElementById("appPage").style.display = "flex";
  applyRoleToNav();
}

function showLogin() {
  document.getElementById("appPage").style.display = "none";
  document.getElementById("loginPage").style.display = "flex";
}


document.getElementById("loginBtn").onclick = doLogin;
document.getElementById("password").onkeydown = function (e) {
  if (e.key === "Enter") doLogin();
};

async function doLogin() {
  var email = document.getElementById("email").value.trim();
  var password = document.getElementById("password").value;
  var errEl = document.getElementById("loginError");

  errEl.textContent = "";

  if (!email || !password) {
    errEl.textContent = "Введите email и пароль";
    return;
  }

  try {
    var r = await fetch("http://localhost:5000/api/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email: email, password: password })
    });

    if (!r.ok) {
      errEl.textContent = "Неверный email или пароль (статус " + r.status + ")";
      return;
    }

    var d = await r.json();
    setTokensFromLogin(d);

    if (!accessToken) {
      errEl.textContent = "Сервер не вернул accessToken";
      return;
    }

    showApp();
  } catch (e) {
    errEl.textContent = "Нет соединения с сервером";
  }
}


document.getElementById("logoutBtn").onclick = function () {
  accessToken = "";
  refreshToken = "";
  currentRole = "";
  clearCookies();

  document.getElementById("email").value = "";
  document.getElementById("password").value = "";
  document.getElementById("loginError").textContent = "";
  document.getElementById("contentArea").innerHTML =
    '<div class="empty-state"><span>⬡</span><p>Выберите раздел</p></div>';


  document.querySelectorAll(".nav-btn").forEach(function (btn) {
    btn.style.display = "";
  });

  showLogin();
};


var savedAccess = getCookieVal("accessToken");
var savedRefresh = getCookieVal("refreshToken");
var savedRole = getCookieVal("userRole");
if (savedAccess) {
  accessToken = savedAccess;
  refreshToken = savedRefresh || "";
  currentRole = savedRole || extractRoleFromToken(savedAccess);
  showApp();
}
