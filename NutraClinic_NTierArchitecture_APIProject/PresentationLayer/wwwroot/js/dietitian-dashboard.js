
    document.querySelectorAll('.scroll-link').forEach(a => {
        a.addEventListener('click', function (e) {
            const target = this.getAttribute('href');
            if (target && target.startsWith('#')) {
                e.preventDefault();
                const el = document.querySelector(target);
                if (el) {
                    el.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        });
    });

    renderCalendar();

    function renderCalendar() {
        const grid = document.getElementById('dt-calendar-grid');
        if (!grid) return;

        const workingDays = grid.dataset.workingDays || 'Mon-Fri';
        const workingHours = grid.dataset.workingHours || '09:00-17:00';

        let appts = [];
        const raw = grid.dataset.appts;
        if (raw) {
            try { appts = JSON.parse(raw) || []; } catch { }
        }

        const wdIdx = parseWorkingDays(workingDays);
        const today = new Date();
        const days = [];

        for (let i = 0; i < 30; i++) {
            const d = new Date(today);
            d.setHours(0, 0, 0, 0);
            d.setDate(d.getDate() + i);
            if (!wdIdx.includes(d.getDay())) continue;
            days.push(d);
        }
        const apptMap = {};
        appts.forEach(a => {
            const iso = dateIso(a.appointmentDate);
            if (!apptMap[iso]) apptMap[iso] = [];
            apptMap[iso].push(a);
        });

        grid.innerHTML = '';
        if (days.length === 0) {
            grid.innerHTML = '<div class="text-light">No working days configured.</div>';
            return;
        }

        days.forEach(d => {
            const iso = d.toISOString().split('T')[0];
            const dow = d.toLocaleDateString('en-US', { weekday: 'short' }); // English
            const month = d.toLocaleDateString('en-US', { month: 'short' });
            const dayNum = d.getDate();
            const myAppts = apptMap[iso] || [];

            const cell = document.createElement('div');
            cell.className = 'dt-cal-day';
            if (myAppts.length > 0) cell.classList.add('has-appt');

            // tooltip
            let tip = '';
            if (myAppts.length > 0) {
                tip += '<strong>Appointments:</strong><br>';
                myAppts.forEach(a => {
                    const st = timeHHMM(a.startTime);
                    const en = timeHHMM(a.endTime);
                    const who = a.userFullName ?? '';
                    tip += `${st}-${en} ${who}<br>`;
                });
            } else {
                tip += 'No appointments.';
            }
            cell.setAttribute('data-bs-toggle', 'tooltip');
            cell.setAttribute('data-bs-html', 'true');
            cell.setAttribute('title', tip);

            cell.innerHTML = `
            <span class="dt-cal-dow">${dow}</span>
            <span class="dt-cal-num">${month} ${dayNum}</span>
            ${myAppts.length > 0 ? `<span class="dt-cal-badge">${myAppts.length}</span>` : ''}
        `;
            grid.appendChild(cell);
        });

        if (typeof bootstrap !== 'undefined') {
            [...document.querySelectorAll('[data-bs-toggle="tooltip"]')]
                .forEach(el => new bootstrap.Tooltip(el));
        }
    }

    function parseWorkingDays(str) {
        const map = { Sun: 0, Mon: 1, Tue: 2, Wed: 3, Thu: 4, Fri: 5, Sat: 6 };
        if (!str) return [1, 2, 3, 4, 5];

        const s2 = str.trim();
        if (s2.includes('-')) {
            const [a, b] = s2.split('-').map(x => x.trim().substring(0, 3));
            const sa = map[a] ?? 1;
            const sb = map[b] ?? 5;
            const arr = [];

            if (sa <= sb) {
                for (let i = sa; i <= sb; i++) arr.push(i);
            } else {
                for (let i = sa; i < 7; i++) arr.push(i);
                for (let i = 0; i <= sb; i++) arr.push(i);
            }

            return arr;
        }
        return s2.split(',')
            .map(x => map[x.trim().substring(0, 3)])
            .filter(x => x !== undefined);
    }

    function dateIso(value) {
        if (!value) return '';
        if (typeof value === 'string') {
            if (value.includes('T')) return value.split('T')[0];
            if (/^\d{4}-\d{2}-\d{2}$/.test(value)) return value;
        }
        try {
            const d = new Date(value);
            return d.toISOString().split('T')[0];
        } catch { return ''; }
    }

    function timeHHMM(ts) {
        if (!ts) return '';
        if (typeof ts === 'string') {
            const parts = ts.split(':');
            if (parts.length >= 2) return parts[0].padStart(2, '0') + ':' + parts[1].padStart(2, '0');
        }
        return ts.toString();
    }
