// Modules
import { useState, useMemo, useEffect } from 'react';

// Styling
import './ReAppointmentBox.css';

// Assets
import ExitIcon from '../../uploads/icon/exit.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useRequestReAppointment from '../../hook/useRequestReAppointment';

function ReAppointmentBox({ schedules, setIsReAppointment, patientID, doctorID, setError, setLoading, setReAppointment, setFinish }) {
    const [doctorNote, setDoctorNote] = useState('');
    const [patientNote, setPatientNote] = useState('');

    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedDate, setSelectedDate] = useState(null);
    const [filteredSchedules, setFilterSchedules] = useState([]);

    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    const currentYear = new Date().getFullYear();

    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const firstDayOfMonth = new Date(year, month, 1).getDay();

    const t1 = 'Chọn ngày tái khám';
    const t2 = 'Không có lịch trống trong ngày này';
    const t3 = 'Lịch trống';
    const t4 = 'Ghi chú của bác sĩ';
    const t5 = 'Ghi chú của bệnh nhân';

    const d1 = 'CN', d2 = 'T2', d3 = 'T3', d4 = 'T4', d5 = 'T5', d6 = 'T6', d7 = 'T7';
    const daysOfWeek = [d1, d2, d3, d4, d5, d6, d7];

    const autoResize = (e) => {
        e.target.style.height = 'auto';
        e.target.style.height = `${e.target.scrollHeight}px`;
    };

    const {
        handleRequest
    } = useRequestReAppointment({ setError, setLoading, setFinish })

    const appointmentDays = useMemo(() => {
        return schedules?.filter(s => {
            const date = new Date(s.date);
            return date.getFullYear() === year && date.getMonth() === month;
        }).map(s => new Date(s.date).getDate());
    }, [schedules, year, month]);

    useEffect(() => {
        if (!selectedDate) {
            setFilterSchedules([]);
            return;
        }

        const filtered = schedules?.filter(s =>
            new Date(s.date).toDateString() === selectedDate.toDateString()
        ) || [];

        setFilterSchedules(filtered);
    }, [selectedDate, schedules]);

    const prevMonth = () => {
        if (year === currentYear && month === 0) return;
        setCurrentDate(new Date(year, month - 1, 1));
    };

    const nextMonth = () => {
        if (year === currentYear + 1 && month === 11) return;
        setCurrentDate(new Date(year, month + 1, 1));
    };

    const days = [];
    for (let i = 0; i < firstDayOfMonth; i++) days.push(null);
    for (let i = 1; i <= daysInMonth; i++) days.push(i);

    return (
        <>
            {schedules && (
                <div
                    className='re-appointment-box-overlap'
                    onClick={(e) => {
                        if (!e.target.closest('.re-appointment-box')) {
                            setIsReAppointment(false);
                            e.stopPropagation();
                        }
                    }}
                >
                    <div className='re-appointment-box'>
                        <div className='header'>
                            <div className='logo'>
                                <img src={LogoPicture} alt='logo-picture' />
                                <img src={LogoText} alt='logo-text' />
                            </div>
                            <button
                                onClick={() => setIsReAppointment(false)}
                                className='exit'
                            >
                                <Icon src={ExitIcon} alt={'exit-icon'} />
                            </button>
                        </div>

                        <div className='body'>
                            <div className='title'>{t1}</div>
                            <div className="calendar-container">
                                <div className="calendar-header">
                                    <button onClick={prevMonth}>‹</button>
                                    <span className="year">
                                        <select
                                            value={year}
                                            onChange={(e) => setCurrentDate(new Date(+e.target.value, month, 1))}
                                        >
                                            <option value={currentYear}>{currentYear}</option>
                                            <option value={currentYear + 1}>{currentYear + 1}</option>
                                        </select>
                                        /{month + 1}
                                    </span>
                                    <button onClick={nextMonth}>›</button>
                                </div>

                                <div className="calendar-grid">
                                    {daysOfWeek.map((d, i) => (
                                        <div key={i} className="calendar-day-name">{d}</div>
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

                            {selectedDate && (
                                <>
                                    <div className='title'>{t4}</div>
                                    <div className='note'>
                                        <textarea
                                            value={doctorNote}
                                            onChange={(e) => {
                                                setDoctorNote(e.target.value);
                                                autoResize(e);
                                            }}
                                            placeholder="Nhập ghi chú của bác sĩ"
                                        />
                                    </div>

                                    <div className='title'>{t5}</div>
                                    <div className='note'>
                                        <textarea
                                            value={patientNote}
                                            onChange={(e) => {
                                                setPatientNote(e.target.value);
                                                autoResize(e);
                                            }}
                                            placeholder="Nhập ghi chú của bệnh nhân"
                                        />
                                    </div>

                                    <div className='title'>{t3}&nbsp;{selectedDate.toLocaleDateString('vi-VN')}</div>
                                    <div className='schedule-select'>
                                        {filteredSchedules.length > 0 ? (
                                            <ul className="schedule-list">
                                                {filteredSchedules.map(s => (
                                                    <li
                                                        key={s.doctorScheduleID}
                                                        onClick={() => {
                                                            setReAppointment(s);
                                                            handleRequest({
                                                                choosenAppointment: {
                                                                    doctorScheduleID: s.doctorScheduleID,
                                                                    notes: patientNote,
                                                                    doctorNotes: doctorNote,
                                                                    patientID: patientID,
                                                                    doctorID: doctorID
                                                                }
                                                            })
                                                        }
                                                        }
                                                    >
                                                        {s.location} | {s.startTime} - {s.endTime}
                                                    </li>
                                                ))}
                                            </ul>
                                        ) : (
                                            <p>{t2}</p>
                                        )}
                                    </div>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}

export default ReAppointmentBox;
