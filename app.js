// DEMO MODE ONLY: Static showcase with mocked telemetry.
// Remove the /demo folder when publishing a real deployment.

function clamp(n, min, max) {
  return Math.max(min, Math.min(max, n));
}

function pick(arr) {
  return arr[Math.floor(Math.random() * arr.length)];
}

const STATUS = ['Online', 'Warning', 'Critical', 'Offline'];

const state = {
  filter: 'All',
  selectedId: 'M-1001',
  machines: [
    { id: 'M-1001', name: 'Compressor A', status: 'Online', temp: 63, vib: 2.1, pressure: 3.2 },
    { id: 'M-1002', name: 'Pump B', status: 'Warning', temp: 81, vib: 3.4, pressure: 2.7 },
    { id: 'M-1003', name: 'Conveyor C', status: 'Online', temp: 58, vib: 1.7, pressure: 1.2 },
    { id: 'M-1004', name: 'Generator D', status: 'Critical', temp: 94, vib: 5.1, pressure: 3.9 },
    { id: 'M-1005', name: 'Boiler E', status: 'Offline', temp: 0, vib: 0, pressure: 0 },
  ],
  trend: new Map(), // id -> [values]
};

const els = {
  statusFilter: document.getElementById('statusFilter'),
  tickBtn: document.getElementById('tickBtn'),
  table: document.getElementById('table'),
  selectedMachine: document.getElementById('selectedMachine'),
  selectedStatus: document.getElementById('selectedStatus'),
  trend: document.getElementById('trend'),
};

function ensureTrend(id) {
  if (!state.trend.has(id)) {
    const m = state.machines.find((x) => x.id === id);
    const base = m?.temp ?? 60;
    state.trend.set(id, Array.from({ length: 10 }, (_, i) => base + Math.sin(i / 2) * 2));
  }
}

function simulateTick() {
  for (const m of state.machines) {
    if (m.status === 'Offline') continue;

    // small random walk
    m.temp = clamp(m.temp + (Math.random() * 4 - 2), 40, 110);
    m.vib = clamp(m.vib + (Math.random() * 0.8 - 0.4), 0.5, 8);
    m.pressure = clamp(m.pressure + (Math.random() * 0.4 - 0.2), 0.5, 6);

    // status heuristics
    if (m.temp >= 92 || m.vib >= 5.0) m.status = 'Critical';
    else if (m.temp >= 80 || m.vib >= 3.2) m.status = 'Warning';
    else m.status = 'Online';

    ensureTrend(m.id);
    const arr = state.trend.get(m.id);
    arr.push(m.temp);
    while (arr.length > 18) arr.shift();
  }

  // occasionally flip offline/online
  const candidate = pick(state.machines);
  if (candidate && Math.random() < 0.12) {
    candidate.status = candidate.status === 'Offline' ? 'Online' : 'Offline';
    if (candidate.status === 'Offline') {
      candidate.temp = 0;
      candidate.vib = 0;
      candidate.pressure = 0;
    } else {
      candidate.temp = 62 + Math.random() * 8;
      candidate.vib = 1.6 + Math.random() * 0.8;
      candidate.pressure = 2.0 + Math.random() * 1.0;
    }
  }

  render();
}

function filteredMachines() {
  return state.filter === 'All' ? state.machines : state.machines.filter((m) => m.status === state.filter);
}

function statusPill(status) {
  const map = {
    Online: '🟢 Online',
    Warning: '🟡 Warning',
    Critical: '🔴 Critical',
    Offline: '⚫ Offline',
  };
  return `<span class="pill">${map[status] ?? status}</span>`;
}

function renderTable() {
  els.table.innerHTML = '';

  for (const m of filteredMachines()) {
    const row = document.createElement('button');
    row.type = 'button';
    row.className = 'cart-item';
    row.style.textAlign = 'left';
    row.style.cursor = 'pointer';

    row.innerHTML = `
      <div>
        <div class="name">${m.id} • ${m.name}</div>
        <div class="line">Temp: ${m.temp.toFixed(0)}°C • Vib: ${m.vib.toFixed(1)} • Pressure: ${m.pressure.toFixed(
      1
    )}</div>
      </div>
      <div>${statusPill(m.status)}</div>
    `;

    row.addEventListener('click', () => {
      state.selectedId = m.id;
      render();
    });

    els.table.appendChild(row);
  }

  if (!filteredMachines().length) {
    const empty = document.createElement('div');
    empty.className = 'muted';
    empty.textContent = 'No machines match this filter.';
    els.table.appendChild(empty);
  }
}

function drawTrend(values) {
  const ctx = els.trend.getContext('2d');
  const w = els.trend.width;
  const h = els.trend.height;
  ctx.clearRect(0, 0, w, h);

  ctx.fillStyle = 'rgba(0,0,0,0.18)';
  ctx.fillRect(0, 0, w, h);

  const pad = 30;
  const innerW = w - pad * 2;
  const innerH = h - pad * 2;

  const min = Math.min(...values);
  const max = Math.max(...values);
  const range = Math.max(1, max - min);

  ctx.strokeStyle = 'rgba(255,255,255,0.12)';
  ctx.lineWidth = 1;
  for (let i = 0; i <= 4; i++) {
    const y = pad + (innerH * i) / 4;
    ctx.beginPath();
    ctx.moveTo(pad, y);
    ctx.lineTo(pad + innerW, y);
    ctx.stroke();
  }

  ctx.strokeStyle = 'rgba(254, 202, 87, 0.95)';
  ctx.lineWidth = 3;
  ctx.beginPath();
  values.forEach((v, idx) => {
    const x = pad + (innerW * idx) / (values.length - 1);
    const y = pad + innerH - ((v - min) / range) * innerH;
    if (idx === 0) ctx.moveTo(x, y);
    else ctx.lineTo(x, y);
  });
  ctx.stroke();

  ctx.fillStyle = 'rgba(255, 107, 107, 0.95)';
  const last = values[values.length - 1];
  ctx.font = '14px ui-sans-serif, system-ui';
  ctx.fillText(`Temp: ${last.toFixed(0)}°C`, pad, pad - 8);
}

function renderSelected() {
  const m = state.machines.find((x) => x.id === state.selectedId) ?? state.machines[0];
  state.selectedId = m.id;

  els.selectedMachine.textContent = `${m.id} • ${m.name}`;
  els.selectedStatus.innerHTML = statusPill(m.status);

  ensureTrend(m.id);
  drawTrend(state.trend.get(m.id));
}

function render() {
  renderTable();
  renderSelected();
}

els.statusFilter.addEventListener('change', (e) => {
  state.filter = e.target.value;
  render();
});

els.tickBtn.addEventListener('click', simulateTick);

render();
