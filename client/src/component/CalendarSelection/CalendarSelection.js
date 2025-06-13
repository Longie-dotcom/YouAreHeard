// Modules
import React, { useState } from 'react';

// Styling sheet
import './CalendarSelection.css';

// Assets

// Components
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';

// Hooks
import useLoadDoctorSchedule from '../../hook/useLoadDoctorSchedule';

function CalendarSelection({ doctor, setChoosenAppointment }) {
    const labels = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
    const t1 = 'Chọn ngày/tháng/năm cho cuộc hẹn';
    const t2 = '-';

    const today = new Date();
    const [month, setMonth] = useState(today.getMonth());
    const [year, setYear] = useState(today.getFullYear());
    const currentYear = today.getFullYear();
    const availableYears = [currentYear, currentYear + 1];
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const firstDay = new Date(year, month, 1).getDay();
    const {
        schedules, loading,
        error, setError,
    } = useLoadDoctorSchedule({ doctorId: doctor.userID });

    const calendarDays = [];

    for (let i = 0; i < firstDay; i++) {
        calendarDays.push(null);
    }

    for (let day = 1; day <= daysInMonth; day++) {
        calendarDays.push(day);
    }

    const monthNames = [
        'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4',
        'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8',
        'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'
    ];

    const scheduleMap = schedules?.reduce((map, schedule) => {
        const date = new Date(schedule.date);
        const key = `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
        if (!map[key]) {
            map[key] = [];
        }
        map[key].push(schedule);
        return map;
    }, {});

    const handleMonthChange = (e) => {
        setMonth(Number(e.target.value));
    };

    const handleYearChange = (e) => {
        setYear(Number(e.target.value));
    };

    return (
        <div className="calendar-selection">
            <div className="calendar-header">
                <div className='title'>
                    {t1}
                </div>

                <div className='selection'>
                    <select value={month} onChange={handleMonthChange}>
                        {monthNames.map((name, idx) => (
                            <option value={idx} key={idx}>{name}</option>
                        ))}
                    </select>

                    <select value={year} onChange={handleYearChange}>
                        {availableYears.map(y => (
                            <option key={y} value={y}>
                                {y}
                            </option>
                        ))}
                    </select>
                </div>
            </div>

            <div className="calendar-grid">
                {labels.map(d => (
                    <div className="calendar-day-label" key={d}>{d}</div>
                ))}
                {calendarDays.map((day, idx) => {
                    const key = day !== null ? `${year}-${month}-${day}` : null;
                    const daySchedules = key && scheduleMap?.[key];

                    return (
                        <div key={idx} className={`calendar-day ${daySchedules?.length ? 'has-schedule' : ''}`}>
                            <div className='date'>
                                {day || ''}
                            </div>
                            <div className='doctor'>
                                {daySchedules?.map((s, i) => (
                                    <div
                                        onClick={() => {
                                            setChoosenAppointment({ doctor, schedule: s });
                                        }}
                                        key={i} className="schedule-item"
                                    >
                                        {doctor.name}
                                        <div className='slots'>
                                            {s.startTime?.slice(0, 5)}&nbsp;{t2}&nbsp;{s.endTime?.slice(0, 5)}
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    );
                })}
            </div>

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}
        </div>
    );
}

export default CalendarSelection;
