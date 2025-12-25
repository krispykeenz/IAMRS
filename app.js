// DEMO MODE ONLY: static showcase that mirrors IAMRS.Web UI structure.
// No .NET runtime / SQL Server required.

function clamp(n, min, max) {
  return Math.max(min, Math.min(max, n));
}

function pick(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
}

function isoNow() {
  return new Date().toISOString();
}

function formatDateTime(iso) {
  if (!iso) return '—';
  try {
    return new Date(iso).toLocaleString();
  } catch {
    return String(iso);
  }
}

function formatTime(iso) {
  if (!iso) return '—';
  try {
    return new Date(iso).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' });
  } catch {
    return String(iso);
  }
}

function toHash(path) {
  if (!path.startsWith('/')) return `#/${path}`;
  return `#${path}`;
}

const STATUS = {
  Online: 'Online',
  Warning: 'Warning',
  Critical: 'Critical',
  Offline: 'Offline',
  Maintenance: 'Maintenance'
};

const state = {
  machines: [
    {
      id: 'M-1001',
      machineCode: 'M-1001',
      name: 'Compressor A',
      type: 'Compressor',
      location: 'Plant 1 • Bay A',
      status: STATUS.Online,
      lastTelemetryAt: isoNow(),
      thresholds: { tempWarn: 80, tempCrit: 90, vibMax: 5.0 }
    },
    {
      id: 'M-1002',
      machineCode: 'M-1002',
      name: 'Pump B',
      type: 'Pump',
      location: 'Plant 1 • Bay C',
      status: STATUS.Warning,
      lastTelemetryAt: isoNow(),
      thresholds: { tempWarn: 80, tempCrit: 90, vibMax: 5.0 }
    },
    {
      id: 'M-1003',
      machineCode: 'M-1003',
      name: 'Conveyor C',
      type: 'Conveyor',
      location: 'Plant 2 • Line 2',
      status: STATUS.Online,
      lastTelemetryAt: isoNow(),
      thresholds: { tempWarn: 75, tempCrit: 88, vibMax: 4.5 }
    },
    {
      id: 'M-1004',
      machineCode: 'M-1004',
      name: 'Generator D',
      type: 'Generator',
      location: 'Plant 2 • Power Room',
      status: STATUS.Critical,
      lastTelemetryAt: isoNow(),
      thresholds: { tempWarn: 78, tempCrit: 90, vibMax: 5.0 }
    },
    {
      id: 'M-1005',
      machineCode: 'M-1005',
      name: 'Boiler E',
      type: 'Boiler',
      location: 'Plant 1 • Utilities',
      status: STATUS.Offline,
      lastTelemetryAt: null,
      thresholds: { tempWarn: 82, tempCrit: 92, vibMax: 5.0 }
    }
  ],
  telemetry: new Map(), // machineId -> Telemetry[]
  alerts: [],
  tickets: [],
  ui: {
    maintenanceShowForm: false
  }
};

const els = {
  app: document.getElementById('app'),
  navToggle: document.getElementById('navToggle'),
  navMenu: document.getElementById('navMenu'),
  tickButton: document.getElementById('demoTickButton'),
  lastTick: document.getElementById('lastTick')
};

function ensureTelemetry(machineId) {
  if (state.telemetry.has(machineId)) return;

  const seed = [];
  const baseTemp = 60 + Math.random() * 10;
  const baseVib = 1.5 + Math.random();
  const basePressure = 2.2 + Math.random();

  const start = Date.now() - 50 * 2000;
  for (let i = 0; i < 50; i++) {
    const timestamp = new Date(start + i * 2000).toISOString();
    seed.push({
      timestamp,
      temperature: baseTemp + Math.sin(i / 5) * 3 + (Math.random() * 2 - 1),
      vibration: baseVib + Math.sin(i / 7) * 0.5 + (Math.random() * 0.3 - 0.15),
      pressure: basePressure + Math.sin(i / 9) * 0.3 + (Math.random() * 0.2 - 0.1)
    });
  }

  state.telemetry.set(machineId, seed);
}

function latestTelemetry(machineId) {
  ensureTelemetry(machineId);
  const list = state.telemetry.get(machineId);
  return list[list.length - 1];
}

function pushAlert({ severity, type, message, machineId }) {
  state.alerts.unshift({
    id: `A-${Date.now()}-${Math.floor(Math.random() * 1000)}`,
    severity,
    type,
    message,
    machineId,
    createdAt: isoNow(),
    isAcknowledged: false,
    acknowledgedAt: null
  });

  // keep list bounded
  if (state.alerts.length > 120) state.alerts.length = 120;
}

function bootstrapMachineStatusBadge(status) {
  switch (status) {
    case STATUS.Online:
      return 'bg-success';
    case STATUS.Warning:
      return 'bg-warning text-dark';
    case STATUS.Critical:
      return 'bg-danger';
    case STATUS.Maintenance:
      return 'bg-info';
    case STATUS.Offline:
      return 'bg-secondary';
    default:
      return 'bg-secondary';
  }
}

function bootstrapAlertSeverityBadge(severity) {
  switch (severity) {
    case 'Critical':
      return 'bg-danger';
    case 'Warning':
      return 'bg-warning text-dark';
    default:
      return 'bg-info';
  }
}

function bootstrapAlertSeverityRow(severity) {
  switch (severity) {
    case 'Critical':
      return 'table-danger';
    case 'Warning':
      return 'table-warning';
    default:
      return '';
  }
}

function machineBorderClass(status) {
  switch (status) {
    case STATUS.Online:
      return 'border-success';
    case STATUS.Warning:
      return 'border-warning';
    case STATUS.Critical:
      return 'border-danger';
    default:
      return 'border-secondary';
  }
}

function counts() {
  const total = state.machines.length;
  const online = state.machines.filter((m) => m.status === STATUS.Online).length;
  const warning = state.machines.filter((m) => m.status === STATUS.Warning).length;
  const critical = state.machines.filter((m) => m.status === STATUS.Critical).length;
  return { total, online, warning, critical };
}

function closeMobileNavIfNeeded() {
  // mimic Blazor default: tapping a nav link closes the drawer on mobile
  if (window.matchMedia('(max-width: 640.98px)').matches) {
    els.navToggle.checked = false;
  }
}

function parseRoute() {
  const raw = window.location.hash || '#/';
  const path = raw.replace(/^#/, '');
  const parts = path.split('/').filter(Boolean);
  // [] => dashboard
  return parts;
}

function setActiveNav(routeKey) {
  const links = document.querySelectorAll('[data-nav]');
  links.forEach((a) => {
    const key = a.getAttribute('data-nav');
    if (key === routeKey) a.classList.add('active');
    else a.classList.remove('active');
  });
}

function renderDashboard() {
  setActiveNav('dashboard');

  const c = counts();
  const recentAlerts = state.alerts.slice(0, 10);

  const machineCards = state.machines
    .map((m) => {
      return `
        <div class="col-md-4 mb-3">
          <div class="card machine-card ${machineBorderClass(m.status)}" data-action="navigate" data-to="${toHash(
        `/machines/${encodeURIComponent(m.id)}`
      )}">
            <div class="card-body">
              <h6 class="card-title">${m.machineCode}</h6>
              <p class="card-text mb-1">${m.name}</p>
              <small class="text-muted">${m.location}</small>
              <div class="mt-2">
                <span class="badge ${bootstrapMachineStatusBadge(m.status)}">${m.status}</span>
              </div>
            </div>
          </div>
        </div>
      `;
    })
    .join('');

  const alertsHtml =
    recentAlerts.length === 0
      ? `<p class="text-muted">No recent alerts</p>`
      : `
        <ul class="list-group list-group-flush">
          ${recentAlerts
            .map((a) => {
              const sevClass = a.severity === 'Critical' ? 'list-group-item-danger' : a.severity === 'Warning' ? 'list-group-item-warning' : '';
              const machineCode =
                state.machines.find((m) => m.id === a.machineId)?.machineCode || 'Unknown';

              return `
                <li class="list-group-item ${sevClass}">
                  <strong>${a.severity}</strong> (${machineCode}): ${a.message}
                  <br><small class="text-muted">${formatDateTime(a.createdAt)}</small>
                </li>
              `;
            })
            .join('')}
        </ul>
      `;

  return `
    <h1 class="mb-4">Dashboard</h1>

    <div class="row mb-4">
      <div class="col-md-3">
        <div class="card text-white bg-primary">
          <div class="card-body">
            <h5 class="card-title">Total Machines</h5>
            <h2 class="card-text">${c.total}</h2>
          </div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card text-white bg-success">
          <div class="card-body">
            <h5 class="card-title">Online</h5>
            <h2 class="card-text">${c.online}</h2>
          </div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card text-white bg-warning">
          <div class="card-body">
            <h5 class="card-title">Warnings</h5>
            <h2 class="card-text">${c.warning}</h2>
          </div>
        </div>
      </div>
      <div class="col-md-3">
        <div class="card text-white bg-danger">
          <div class="card-body">
            <h5 class="card-title">Critical</h5>
            <h2 class="card-text">${c.critical}</h2>
          </div>
        </div>
      </div>
    </div>

    <div class="row">
      <div class="col-md-8">
        <div class="card">
          <div class="card-header">Machine Status</div>
          <div class="card-body">
            <div class="row">
              ${machineCards}
            </div>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card">
          <div class="card-header">Recent Alerts</div>
          <div class="card-body" style="max-height: 400px; overflow-y: auto;">
            ${alertsHtml}
          </div>
        </div>
      </div>
    </div>
  `;
}

function renderMachines() {
  setActiveNav('machines');

  const rows = state.machines
    .map((m) => {
      const last = m.lastTelemetryAt ? formatDateTime(m.lastTelemetryAt) : 'Never';
      return `
        <tr data-action="navigate" data-to="${toHash(`/machines/${encodeURIComponent(m.id)}`)}">
          <td><strong>${m.machineCode}</strong></td>
          <td>${m.name}</td>
          <td>${m.type}</td>
          <td>${m.location}</td>
          <td><span class="badge ${bootstrapMachineStatusBadge(m.status)}">${m.status}</span></td>
          <td>${last}</td>
        </tr>
      `;
    })
    .join('');

  return `
    <h1 class="mb-4">Machines</h1>
    <div class="card">
      <div class="card-body">
        <table class="table table-striped table-hover">
          <thead>
            <tr>
              <th>Code</th>
              <th>Name</th>
              <th>Type</th>
              <th>Location</th>
              <th>Status</th>
              <th>Last Telemetry</th>
            </tr>
          </thead>
          <tbody>
            ${rows}
          </tbody>
        </table>
      </div>
    </div>
  `;
}

function drawTelemetryChart(canvas, points) {
  if (!canvas) return;
  const ctx = canvas.getContext('2d');
  if (!ctx) return;

  const w = canvas.width;
  const h = canvas.height;
  ctx.clearRect(0, 0, w, h);

  ctx.fillStyle = 'white';
  ctx.fillRect(0, 0, w, h);

  const pad = 36;
  const innerW = w - pad * 2;
  const innerH = h - pad * 2;

  const temps = points.map((p) => p.temperature);
  const vibs = points.map((p) => p.vibration);

  const all = temps.concat(vibs.map((v) => v * 15)); // scale vib for single chart
  const min = Math.min(...all);
  const max = Math.max(...all);
  const range = Math.max(1, max - min);

  // grid
  ctx.strokeStyle = 'rgba(0,0,0,0.08)';
  ctx.lineWidth = 1;
  for (let i = 0; i <= 4; i++) {
    const y = pad + (innerH * i) / 4;
    ctx.beginPath();
    ctx.moveTo(pad, y);
    ctx.lineTo(pad + innerW, y);
    ctx.stroke();
  }

  function plot(series, color) {
    ctx.strokeStyle = color;
    ctx.lineWidth = 2;
    ctx.beginPath();
    series.forEach((v, idx) => {
      const x = pad + (innerW * idx) / (series.length - 1);
      const y = pad + innerH - ((v - min) / range) * innerH;
      if (idx === 0) ctx.moveTo(x, y);
      else ctx.lineTo(x, y);
    });
    ctx.stroke();
  }

  plot(temps, 'rgb(255, 99, 132)');
  plot(vibs.map((v) => v * 15), 'rgb(54, 162, 235)');

  ctx.fillStyle = 'rgba(0,0,0,0.6)';
  ctx.font = '12px Helvetica Neue, Helvetica, Arial, sans-serif';
  const last = points[points.length - 1];
  ctx.fillText(`Temp: ${last.temperature.toFixed(1)}°C`, pad, 18);
  ctx.fillText(`Vibration: ${last.vibration.toFixed(2)} mm/s`, pad + 160, 18);
}

function renderMachineDetail(machineId) {
  setActiveNav('machines');

  const m = state.machines.find((x) => x.id === machineId);
  if (!m) {
    return `
      <h1>Machine</h1>
      <p class="text-muted">Machine not found.</p>
      <a href="#/machines" class="btn btn-outline-primary">Back</a>
    `;
  }

  ensureTelemetry(m.id);
  const telemetry = state.telemetry.get(m.id);
  const recentTelemetry = telemetry.slice(-20).reverse();

  const recentAlerts = state.alerts.filter((a) => a.machineId === m.id).slice(0, 10);

  const alertsList =
    recentAlerts.length === 0
      ? `<p class="text-muted">No alerts</p>`
      : `
        <ul class="list-group list-group-flush">
          ${recentAlerts
            .map((a) => {
              const sevClass = a.severity === 'Critical' ? 'list-group-item-danger' : a.severity === 'Warning' ? 'list-group-item-warning' : '';
              return `
                <li class="list-group-item ${sevClass}">
                  <strong>${a.severity}</strong>: ${a.message}
                  <br><small class="text-muted">${formatDateTime(a.createdAt)}</small>
                </li>
              `;
            })
            .join('')}
        </ul>
      `;

  return `
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><a href="#/machines">Machines</a></li>
        <li class="breadcrumb-item active" aria-current="page">${m.machineCode}</li>
      </ol>
    </nav>

    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1 class="mb-0">${m.name}</h1>
      <span class="badge fs-6 ${bootstrapMachineStatusBadge(m.status)}">${m.status}</span>
    </div>

    <div class="row mb-4">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Machine Information</div>
          <div class="card-body">
            <dl class="row mb-0">
              <dt class="col-sm-4">Code</dt>
              <dd class="col-sm-8">${m.machineCode}</dd>
              <dt class="col-sm-4">Type</dt>
              <dd class="col-sm-8">${m.type}</dd>
              <dt class="col-sm-4">Location</dt>
              <dd class="col-sm-8">${m.location}</dd>
              <dt class="col-sm-4">Last Telemetry</dt>
              <dd class="col-sm-8">${m.lastTelemetryAt ? formatDateTime(m.lastTelemetryAt) : 'Never'}</dd>
            </dl>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Thresholds</div>
          <div class="card-body">
            <dl class="row mb-0">
              <dt class="col-sm-6">Temp Warning</dt>
              <dd class="col-sm-6">${m.thresholds.tempWarn} °C</dd>
              <dt class="col-sm-6">Temp Critical</dt>
              <dd class="col-sm-6">${m.thresholds.tempCrit} °C</dd>
              <dt class="col-sm-6">Vibration Max</dt>
              <dd class="col-sm-6">${m.thresholds.vibMax} mm/s</dd>
            </dl>
          </div>
        </div>
      </div>
    </div>

    <div class="card mb-4">
      <div class="card-header d-flex justify-content-between align-items-center">
        <span>Telemetry Trend (Demo)</span>
        <button class="btn btn-sm btn-outline-primary" data-action="refresh-machine" data-id="${m.id}">Refresh</button>
      </div>
      <div class="card-body">
        <canvas id="telemetryChart" width="900" height="320" style="width: 100%; height: auto;"></canvas>
        <div class="text-muted small mt-2">Red: temperature • Blue: vibration (scaled)</div>
      </div>
    </div>

    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Recent Telemetry Data</div>
          <div class="card-body" style="max-height: 300px; overflow-y: auto;">
            <table class="table table-sm">
              <thead>
                <tr>
                  <th>Time</th>
                  <th>Temp</th>
                  <th>Vibration</th>
                  <th>Pressure</th>
                </tr>
              </thead>
              <tbody>
                ${recentTelemetry
                  .map((t) => {
                    return `
                      <tr>
                        <td>${formatTime(t.timestamp)}</td>
                        <td>${t.temperature.toFixed(1)} °C</td>
                        <td>${t.vibration.toFixed(2)}</td>
                        <td>${t.pressure.toFixed(1)}</td>
                      </tr>
                    `;
                  })
                  .join('')}
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Recent Alerts</div>
          <div class="card-body" style="max-height: 300px; overflow-y: auto;">
            ${alertsList}
          </div>
        </div>
      </div>
    </div>
  `;
}

function renderAlerts() {
  setActiveNav('alerts');

  const rows = state.alerts
    .slice(0, 60)
    .map((a) => {
      const machineCode = state.machines.find((m) => m.id === a.machineId)?.machineCode || 'Unknown';
      return `
        <tr class="${bootstrapAlertSeverityRow(a.severity)}">
          <td><span class="badge ${bootstrapAlertSeverityBadge(a.severity)}">${a.severity}</span></td>
          <td>${a.type}</td>
          <td>${a.message}</td>
          <td>${machineCode}</td>
          <td>${formatDateTime(a.createdAt)}</td>
          <td>
            ${
              a.isAcknowledged
                ? '<span class="badge bg-secondary">Acknowledged</span>'
                : `<button class="btn btn-sm btn-outline-primary" data-action="ack-alert" data-id="${a.id}">Acknowledge</button>`
            }
          </td>
        </tr>
      `;
    })
    .join('');

  return `
    <h1 class="mb-4">Alerts</h1>

    <div class="card">
      <div class="card-body">
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Severity</th>
              <th>Type</th>
              <th>Message</th>
              <th>Machine</th>
              <th>Created</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            ${rows || `<tr><td colspan="6" class="text-muted">No alerts yet. Click “Simulate tick”.</td></tr>`}
          </tbody>
        </table>
      </div>
    </div>
  `;
}

function priorityBadge(priority) {
  switch (priority) {
    case 'Urgent':
      return 'bg-danger';
    case 'High':
      return 'bg-warning text-dark';
    case 'Medium':
      return 'bg-info';
    default:
      return 'bg-secondary';
  }
}

function ticketStatusBadge(status) {
  switch (status) {
    case 'Open':
      return 'bg-primary';
    case 'In Progress':
      return 'bg-info';
    case 'Resolved':
      return 'bg-success';
    case 'Closed':
      return 'bg-secondary';
    default:
      return 'bg-dark';
  }
}

function renderMaintenance() {
  setActiveNav('maintenance');

  const machineOptions = state.machines
    .map((m) => `<option value="${m.id}">${m.machineCode} - ${m.name}</option>`)
    .join('');

  const formHtml = state.ui.maintenanceShowForm
    ? `
      <div class="card mb-4">
        <div class="card-header">Create Ticket</div>
        <div class="card-body">
          <form data-action="create-ticket">
            <div class="row">
              <div class="col-md-6 mb-3">
                <label class="form-label">Machine</label>
                <select class="form-select" name="machineId" required>
                  <option value="">Select...</option>
                  ${machineOptions}
                </select>
              </div>
              <div class="col-md-6 mb-3">
                <label class="form-label">Priority</label>
                <select class="form-select" name="priority" required>
                  <option>Low</option>
                  <option selected>Medium</option>
                  <option>High</option>
                  <option>Urgent</option>
                </select>
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label">Title</label>
              <input type="text" class="form-control" name="title" required />
            </div>
            <div class="mb-3">
              <label class="form-label">Description</label>
              <textarea class="form-control" rows="3" name="description"></textarea>
            </div>
            <button type="submit" class="btn btn-success">Save</button>
            <button type="button" class="btn btn-secondary" data-action="toggle-ticket-form">Cancel</button>
          </form>
        </div>
      </div>
    `
    : '';

  const rows = state.tickets
    .slice(0, 50)
    .map((t) => {
      const machineCode = state.machines.find((m) => m.id === t.machineId)?.machineCode || '?';
      return `
        <tr>
          <td><strong>${t.ticketNumber}</strong></td>
          <td>${machineCode}</td>
          <td>${t.title}</td>
          <td><span class="badge ${priorityBadge(t.priority)}">${t.priority}</span></td>
          <td><span class="badge ${ticketStatusBadge(t.status)}">${t.status}</span></td>
          <td>${formatDateTime(t.createdAt)}</td>
        </tr>
      `;
    })
    .join('');

  return `
    <h1 class="mb-4">Maintenance Tickets</h1>

    <div class="mb-3">
      <button class="btn btn-primary" data-action="toggle-ticket-form">+ Create Ticket</button>
    </div>

    ${formHtml}

    <div class="card">
      <div class="card-body">
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Ticket #</th>
              <th>Machine</th>
              <th>Title</th>
              <th>Priority</th>
              <th>Status</th>
              <th>Created</th>
            </tr>
          </thead>
          <tbody>
            ${
              rows ||
              `<tr><td colspan="6" class="text-muted">No tickets yet. Create one to demo the workflow.</td></tr>`
            }
          </tbody>
        </table>
      </div>
    </div>
  `;
}

function renderRoute() {
  const parts = parseRoute();

  // Default: dashboard
  if (parts.length === 0) {
    els.app.innerHTML = renderDashboard();
    closeMobileNavIfNeeded();
    return;
  }

  const [root, id] = parts;

  if (root === 'machines' && id) {
    els.app.innerHTML = renderMachineDetail(decodeURIComponent(id));

    // draw chart after DOM has the canvas
    const canvas = document.getElementById('telemetryChart');
    const machineId = decodeURIComponent(id);
    ensureTelemetry(machineId);
    drawTelemetryChart(canvas, state.telemetry.get(machineId));

    closeMobileNavIfNeeded();
    return;
  }

  if (root === 'machines') {
    els.app.innerHTML = renderMachines();
    closeMobileNavIfNeeded();
    return;
  }

  if (root === 'alerts') {
    els.app.innerHTML = renderAlerts();
    closeMobileNavIfNeeded();
    return;
  }

  if (root === 'maintenance') {
    els.app.innerHTML = renderMaintenance();
    closeMobileNavIfNeeded();
    return;
  }

  // Fallback
  window.location.hash = '#/';
}

function simulateTick() {
  // update a random subset to keep it lively
  for (const m of state.machines) {
    ensureTelemetry(m.id);

    // Offline machines can come back occasionally
    if (m.status === STATUS.Offline) {
      if (Math.random() < 0.08) {
        m.status = STATUS.Online;
        m.lastTelemetryAt = isoNow();
        pushAlert({
          severity: 'Info',
          type: 'Machine',
          message: `${m.machineCode} is back online.`,
          machineId: m.id
        });
      }
      continue;
    }

    const prevStatus = m.status;
    const last = latestTelemetry(m.id);

    const temperature = clamp(last.temperature + (Math.random() * 4 - 2), 35, 110);
    const vibration = clamp(last.vibration + (Math.random() * 0.8 - 0.4), 0.2, 8);
    const pressure = clamp(last.pressure + (Math.random() * 0.4 - 0.2), 0.5, 6);

    const next = { timestamp: isoNow(), temperature, vibration, pressure };

    const list = state.telemetry.get(m.id);
    list.push(next);
    while (list.length > 50) list.shift();

    m.lastTelemetryAt = next.timestamp;

    // heuristic status updates
    if (temperature >= m.thresholds.tempCrit || vibration >= m.thresholds.vibMax) m.status = STATUS.Critical;
    else if (temperature >= m.thresholds.tempWarn || vibration >= 3.2) m.status = STATUS.Warning;
    else m.status = STATUS.Online;

    if (m.status !== prevStatus && (m.status === STATUS.Warning || m.status === STATUS.Critical)) {
      pushAlert({
        severity: m.status === STATUS.Critical ? 'Critical' : 'Warning',
        type: 'Telemetry',
        message: `${m.machineCode} entered ${m.status} state (Temp ${temperature.toFixed(0)}°C, Vib ${vibration.toFixed(
          1
        )}).`,
        machineId: m.id
      });
    }

    // occasional offline event
    if (Math.random() < 0.015) {
      m.status = STATUS.Offline;
      pushAlert({
        severity: 'Warning',
        type: 'Machine',
        message: `${m.machineCode} is not responding (offline).`,
        machineId: m.id
      });
    }
  }

  els.lastTick.textContent = `Last tick: ${new Date().toLocaleTimeString()}`;
  renderRoute();
}

// Seed a couple alerts so pages don't look empty
pushAlert({ severity: 'Warning', type: 'Telemetry', message: 'M-1002 temperature trending high.', machineId: 'M-1002' });
pushAlert({ severity: 'Critical', type: 'Telemetry', message: 'M-1004 vibration exceeded limit.', machineId: 'M-1004' });

// Seed a couple tickets
state.tickets.unshift({
  id: 'T-1',
  ticketNumber: `MT-${new Date().toISOString().slice(0, 10).replace(/-/g, '')}-090012`,
  machineId: 'M-1004',
  title: 'Inspect vibration isolators',
  description: 'High vibration readings sustained over 20 minutes.',
  priority: 'High',
  status: 'Open',
  createdAt: isoNow()
});

state.tickets.unshift({
  id: 'T-2',
  ticketNumber: `MT-${new Date().toISOString().slice(0, 10).replace(/-/g, '')}-081540`,
  machineId: 'M-1002',
  title: 'Check cooling line',
  description: 'Temperature warnings observed during peak load.',
  priority: 'Medium',
  status: 'In Progress',
  createdAt: isoNow()
});

// Event handlers
window.addEventListener('hashchange', renderRoute);

els.tickButton.addEventListener('click', simulateTick);

// Delegate clicks inside the app region
els.app.addEventListener('click', (event) => {
  const target = event.target.closest('[data-action]');
  if (!target) return;

  const action = target.getAttribute('data-action');
  if (action === 'navigate') {
    const to = target.getAttribute('data-to');
    if (to) window.location.hash = to.replace(/^#/, '');
    return;
  }

  if (action === 'ack-alert') {
    const id = target.getAttribute('data-id');
    const alert = state.alerts.find((a) => a.id === id);
    if (alert) {
      alert.isAcknowledged = true;
      alert.acknowledgedAt = isoNow();
      renderRoute();
    }
    return;
  }

  if (action === 'toggle-ticket-form') {
    state.ui.maintenanceShowForm = !state.ui.maintenanceShowForm;
    renderRoute();
    return;
  }

  if (action === 'create-ticket') {
    // handled by submit
    return;
  }

  if (action === 'refresh-machine') {
    const id = target.getAttribute('data-id');
    if (id) {
      simulateTick();
      // keep route
      window.location.hash = `#/machines/${encodeURIComponent(id)}`;
    }
    return;
  }
});

els.app.addEventListener('submit', (event) => {
  const form = event.target;
  if (!(form instanceof HTMLFormElement)) return;

  if (form.getAttribute('data-action') !== 'create-ticket') return;

  event.preventDefault();
  const fd = new FormData(form);
  const machineId = String(fd.get('machineId') || '');
  const priority = String(fd.get('priority') || 'Medium');
  const title = String(fd.get('title') || '').trim();
  const description = String(fd.get('description') || '').trim();

  if (!machineId || !title) return;

  const ticketNumber = `MT-${new Date().toISOString().slice(0, 10).replace(/-/g, '')}-${String(Date.now()).slice(-6)}`;
  state.tickets.unshift({
    id: `T-${Date.now()}`,
    ticketNumber,
    machineId,
    title,
    description,
    priority,
    status: 'Open',
    createdAt: isoNow()
  });

  state.ui.maintenanceShowForm = false;
  renderRoute();
});

// Initial render
els.lastTick.textContent = `Last tick: ${new Date().toLocaleTimeString()}`;
renderRoute();
