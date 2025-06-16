import { useState, useMemo } from 'react';
import './Calendar.css';

function Calendar({ appointments }) {
    const [currentDate, setCurrentDate] = useState(new Date());
    const currentYear = new Date().getFullYear();

    const year = currentDate.getFullYear();
    const month = currentDate.getMonth(); // 0-indexed

    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const firstDayOfWeek = new Date(year, month, 1).getDay(); // 0 = Sunday

    // Highlight dates with appointments
    const appointmentDays = useMemo(() => {
        return appointments
            ?.filter(app => {
                const date = new Date(app.date);
                return date.getFullYear() === year && date.getMonth() === month;
            })
            .map(app => new Date(app.date).getDate());
    }, [appointments, year, month]);
    
    const prevMonth = () => {
        if (year === currentYear && month === 0) return;
        setCurrentDate(new Date(year, month - 1, 1));
    };

    const nextMonth = () => {
        if (year === currentYear + 1 && month === 11) return;
        setCurrentDate(new Date(year, month + 1, 1));
    };

    const days = [];
    for (let i = 0; i < firstDayOfWeek; i++) {
        days.push(null);
    }
    for (let i = 1; i <= daysInMonth; i++) {
        days.push(i);
    }

    return (
        <div className="calendar-container">
            <div className="calendar-header">
                <button onClick={prevMonth} disabled={year === currentYear && month === 0}>‹</button>

                <span className='year'>
                    <select
                        value={year}
                        onChange={(e) => setCurrentDate(new Date(+e.target.value, month, 1))}
                    >
                        <option value={currentYear}>{currentYear}</option>
                        <option value={currentYear + 1}>{currentYear + 1}</option>
                    </select>
                    /{month + 1}
                </span>

                <button onClick={nextMonth} disabled={year === currentYear + 1 && month === 11}>›</button>
            </div>

            <div className="calendar-grid">
                {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map((d, i) => (
                    <div key={i} className="calendar-day-name">{d}</div>
                ))}

                {days.map((day, i) => (
                    <div
                        key={i}
                        className={`calendar-day ${day ? 'filled' : 'empty'} ${appointmentDays?.includes(day) ? 'has-appointment' : ''}`}
                    >
                        {day || ''}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Calendar;
