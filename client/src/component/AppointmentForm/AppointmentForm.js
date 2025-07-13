// Modules
import { useEffect, useMemo, useState } from 'react';

// Styling sheet
import './AppointmentForm.css';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useLoadAllDoctor from '../../hook/useLoadAllDoctor';
import useLoadDoctorSchedule from '../../hook/useLoadDoctorSchedule';
import useUpdateAptSchedule from '../../hook/useUpdateAptSchedule';

function AppointmentForm({ setOpenUpdateAppointment, openUpdateAppointment, setError, setLoading, setFinish, setReload }) {
    const t1 = 'Cập nhật lịch hẹn';
    const t2 = 'Bác sĩ';

    const currentYear = new Date().getFullYear();
    const [selectedDoctorId, setSelectedDoctorId] = useState(openUpdateAppointment.doctorID);
    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedDate, setSelectedDate] = useState(null);
    const [filteredSchedules, setFilteredSchedules] = useState([]);

    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const firstDayOfMonth = new Date(year, month, 1).getDay();

    const {
        schedules
    } = useLoadDoctorSchedule({ setError, setLoading, doctorId: selectedDoctorId });

    const {
        doctors
    } = useLoadAllDoctor({ setError, setLoading });

    const {
        updateAppointmentSchedule
    } = useUpdateAptSchedule({ setError, setLoading, setFinish, setReload });
    const days = [];
    for (let i = 0; i < firstDayOfMonth; i++) days.push(null);
    for (let i = 1; i <= daysInMonth; i++) days.push(i);

    const appointmentDays = useMemo(() => {
        return schedules?.filter(s => {
            const date = new Date(s.date);
            return date.getFullYear() === year && date.getMonth() === month;
        }).map(s => new Date(s.date).getDate());
    }, [schedules, year, month]);

    useEffect(() => {
        if (!selectedDate) {
            setFilteredSchedules([]);
            return;
        }

        const filtered = schedules?.filter(s =>
            new Date(s.date).toDateString() === selectedDate.toDateString()
        ) || [];

        setFilteredSchedules(filtered);
    }, [selectedDate, schedules]);

    return (
        <div
            className='appointment-form-overlap'
            onClick={(e) => {
                if (!e.target.closest('.appointment-form')) {
                    setOpenUpdateAppointment(null);
                    e.stopPropagation();
                }
            }}
        >
            <div className='appointment-form'>
                <h2>{t1}</h2>

                <label>{t2}</label>
                <select
                    value={selectedDoctorId}
                    onChange={(e) => setSelectedDoctorId(+e.target.value)}
                >
                    {doctors?.map(doctor => (
                        <option key={doctor.userID} value={doctor.userID}>
                            {doctor.name}
                        </option>
                    ))}
                </select>

                {schedules && (
                    <div className="calendar">
                        <div className="calendar-header">
                            <button onClick={() => setCurrentDate(new Date(year, month - 1, 1))}>‹</button>
                            <span>{`${year}/${month + 1}`}</span>
                            <button onClick={() => setCurrentDate(new Date(year, month + 1, 1))}>›</button>
                        </div>

                        <div className="calendar-grid">
                            {["CN", "T2", "T3", "T4", "T5", "T6", "T7"].map((day, i) => (
                                <div key={i} className="calendar-day-name">{day}</div>
                            ))}

                            {days.map((day, i) => {
                                const isValid = appointmentDays.includes(day);
                                const date = new Date(year, month, day);

                                return (
                                    <div
                                        key={i}
                                        className={`calendar-day ${day ? 'filled' : 'empty'} ${isValid ? 'has-appointment' : ''}`}
                                        onClick={() => isValid && setSelectedDate(date)}
                                    >
                                        {day || ''}
                                    </div>
                                );
                            })}
                        </div>
                    </div>
                )}

                {selectedDate && (
                    <ul className="schedule-list">
                        {filteredSchedules.map(s => (
                            <li 
                            onClick={() => updateAppointmentSchedule({
                                appointmentID: openUpdateAppointment.appointmentID,
                                doctorScheduleID: s.doctorScheduleID,
                                doctorID: selectedDoctorId
                            })}
                            key={s.doctorScheduleID}>
                                {s.location} | {s.startTime} - {s.endTime}
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
}

export default AppointmentForm;