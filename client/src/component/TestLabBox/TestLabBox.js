// Modules
import { useEffect, useState } from 'react';

// Styling sheet
import './TestLabBox.css';

// Assets
import ZoomIcon from '../../uploads/icon/zoom.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function TestLabBox({ appointments }) {
    const t1 = 'Danh sách bệnh nhân';
    const t2 = 'Hồ sơ bệnh nhân';
    const t3 = 'Điền kết quả xét nghiệm';

    const t22 = '-';
    const t23 = 'Trực tuyến';
    const t24 = 'Tạo kế hoạch';
    const t25 = 'Số thứ tự: ';

    const [upcomingAppointments, setUpcomingAppointments] = useState(null);
    const [selectedTestOf, setSelectedTestOf] = useState(null);

    const formatTime = (timeStr) => {
        const [hours, minutes] = timeStr.split(':');
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}`;
    };

    useEffect(() => {
        if (!appointments) return;

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const upcomingAppointments = appointments
            .map(app => {
                const dateStr = app.scheduleDate.split('T')[0];
                const dateTime = new Date(`${dateStr}T${app.startTime}`);
                return {
                    ...app,
                    dateTime,
                };
            })
            .filter(app => {
                const appointmentDate = new Date(app.dateTime);
                appointmentDate.setHours(0, 0, 0, 0);
                return appointmentDate >= today && !app.zoomLink;
            });

        setUpcomingAppointments(upcomingAppointments || []);
    }, [appointments]);

    return (
        <div className='test-lab-box'>
            <div className='title'>
                {t1}
            </div>

            {upcomingAppointments && (
                <div className='patient-list'>
                    {upcomingAppointments.map((appointment) => (
                        <div className='appointment-later'>
                            <div className='name'>
                                {appointment.patientName}
                            </div>
                            <div className='date'>
                                {appointment.scheduleDate?.split("T")[0]}
                            </div>
                            <div className='time'>
                                {formatTime(appointment.startTime)}{t22}{formatTime(appointment.endTime)}
                            </div>
                            <div className='location'>
                                {appointment.zoomLink ? (
                                    <div>
                                        <Icon src={ZoomIcon} alt={'zoom-icon'} /> {t23}
                                    </div>
                                ) : appointment.location}
                            </div>
                            <div className='queue-number'>
                                {t25}{appointment.queueNumber}
                            </div>
                            <div className='patient-detail'>
                                <button
                                    onClick={() => setSelectedTestOf(appointment)}
                                >
                                    {t24}
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {selectedTestOf && (
                <>
                    <div className='title'>
                        {t2}
                    </div>
                    <div className='patient-info'>
                        Thông tin bệnh nhân {selectedTestOf.patientName} sẽ nằm ở đây
                    </div>

                    <div className='title'>
                        {t3}
                    </div>
                    <div className='test-form'>
                        
                    </div>
                </>
            )}
        </div>
    )
}

export default TestLabBox;