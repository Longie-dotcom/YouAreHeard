// Modules
import { useState, useEffect } from 'react';

// Styling sheet
import './DoctorAppointment.css';

// Assets
import VideoWhiteIcon from '../../uploads/icon/video-white.png';
import HospitalWhiteIcon from '../../uploads/icon/hospital-white.png';
import UserIcon from '../../uploads/icon/user.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import ZoomIcon from '../../uploads/icon/zoom.png';
import TicketIcon from '../../uploads/icon/ticket.png';

// Components
import Icon from '../Icon/Icon';
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import AppointmentDetailBox from '../AppointmentDetail/AppointmentDetailBox';

// Hooks
import useGetPatientDetailByAppointment from '../../hook/useGetPatientDetailByAppointment';

function DoctorAppointment({ user, appointments }) {
    const t2 = 'Người khám bệnh';
    const t3 = 'Ngày';
    const t4 = 'Thời gian';
    const t5 = 'Địa điểm';
    const t6 = 'Đường dẫn';
    const t7 = '-';
    const t8 = 'Zoom meeting (Online)';
    const t9 = 'BS.';
    const t10 = 'Vào phòng Zoom';
    const t11 = 'Ghi chú';
    const t12 = 'Lý do';
    const t14m1 = 'Trạng thái lịch hẹn';
    const t18 = 'Số thứ tự';
    const t19 = 'Tư vấn trực tuyến'
    const t20 = 'Khám/Điều trị trực tiếp'
    const t21 = 'Cuộc hẹn sắp tới';
    const t22 = '-';
    const t23 = 'Trực tuyến';
    const t24 = 'Xem thông tin';
    const t25 = 'Số thứ tự: ';

    const [nextAppointment, setNextAppointment] = useState(null);
    const [error, setError] = useState();
    const [loading, setLoading] = useState();
    const [upcomingAppointments, setUpcomingAppointments] = useState(null);
    const [appointmentDetail, setAppointmentDetail] = useState(null);
    const [filterType, setFilterType] = useState('all');

    const {
        getPatientProfileByAppointmentId
    } = useGetPatientDetailByAppointment({ setError, setLoading, setAppointmentDetail });

    const formatTime = (timeStr) => {
        const [hours, minutes] = timeStr.split(':');
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}`;
    };

    useEffect(() => {
        if (!appointments) return;

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        let upcomingAppointments = appointments
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
                return appointmentDate >= today;
            });

        if (filterType === 'online') {
            upcomingAppointments = upcomingAppointments.filter(app => app.zoomLink);
        } else if (filterType === 'offline') {
            upcomingAppointments = upcomingAppointments.filter(app => !app.zoomLink);
        }

        setUpcomingAppointments(upcomingAppointments || []);
        setNextAppointment(upcomingAppointments?.[0] || null);
    }, [appointments, filterType]);

    return (
        <div className='doctor-appointment'>
            {nextAppointment && (
                <>
                    <div className='main-title'>
                        {t21}
                    </div>
                    <div className='today'>
                        <div className="patient">
                            <div className="title">
                                <Icon src={UserIcon} alt={'person-icon'} />
                                {t2}
                            </div>
                            <div className="detail">
                                {nextAppointment.patientName}
                            </div>
                        </div>

                        <div className="date">
                            <div className="title">
                                <Icon src={ScheduleIcon} alt={'person-icon'} />
                                {t3}
                            </div>
                            <div className="detail">
                                {nextAppointment.scheduleDate?.split("T")[0]}
                            </div>
                        </div>

                        <div className="time">
                            <div className="title">
                                <Icon src={TimeIcon} alt={'person-icon'} />
                                {t4}
                            </div>
                            <div className="detail">
                                {nextAppointment.startTime}&nbsp;{t7}&nbsp;{nextAppointment.endTime}
                            </div>
                        </div>

                        <div className="queue-number">
                            <div className="title">
                                <Icon src={TicketIcon} alt={'person-icon'} />
                                {t18}
                            </div>
                            <div className="detail">
                                {nextAppointment.queueNumber}
                            </div>
                        </div>

                        {!nextAppointment.zoomLink && (
                            <div className="location">
                                <div className="title">
                                    <Icon src={LocationIcon} alt={'person-icon'} />
                                    {t5}
                                </div>
                                <div className="detail">
                                    {nextAppointment.zoomLink ? t8 : nextAppointment.location}
                                </div>
                            </div>
                        )}

                        {nextAppointment.zoomLink && (
                            <div className="zoom">
                                <div className="title">
                                    {t6}
                                </div>
                                <div className="detail">
                                    <a
                                        href={nextAppointment.zoomLink} target="_blank" rel="noopener noreferrer"
                                        className='zoom-link'>
                                        <Icon src={ZoomIcon} alt={'zoom-icon'} />
                                        &nbsp;&nbsp;&nbsp;{t10}
                                    </a>
                                </div>
                            </div>
                        )}

                        {nextAppointment.notes && (
                            <div className="note">
                                <div className="title">
                                    {t11}
                                </div>
                                <div className="detail">
                                    {nextAppointment.notes}
                                </div>
                            </div>
                        )}

                        {nextAppointment.reason && (
                            <div className="reason">
                                <div className="title">
                                    {t12}
                                </div>
                                <div className="detail">
                                    {nextAppointment.reason}
                                </div>
                            </div>
                        )}

                        {nextAppointment.appointmentStatusName && (
                            <div className="status">
                                <div className="title">
                                    {t14m1}
                                </div>
                                <div className="detail">
                                    {nextAppointment.appointmentStatusName}
                                </div>
                            </div>
                        )}
                    </div>
                </>
            )}

            <div className='category'>
                <button
                    className={`online ${filterType === 'online' ? 'active' : ''}`}
                    onClick={() => setFilterType(prev => prev === 'online' ? 'all' : 'online')}
                >
                    <Icon src={VideoWhiteIcon} alt={'video-white-icon'} />
                    <div>{t19}</div>
                </button>

                <button
                    className={`offline ${filterType === 'offline' ? 'active' : ''}`}
                    onClick={() => setFilterType(prev => prev === 'offline' ? 'all' : 'offline')}
                >
                    <Icon src={HospitalWhiteIcon} alt={'hospital-white-icon'} />
                    <div>{t20}</div>
                </button>
            </div>

            {appointments && upcomingAppointments && (
                <>
                    <div className='appointment'>
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
                                        <div className='zoom-link'>
                                            <a href={appointment.zoomLink}>
                                                <Icon src={ZoomIcon} alt={'zoom-icon'} /> {t23}
                                            </a>
                                        </div>
                                    ) : appointment.location}
                                </div>
                                <div className='queue-number'>
                                    {t25}{appointment.queueNumber}
                                </div>
                                <div className='patient-detail'>
                                    <button
                                        onClick={
                                            () => getPatientProfileByAppointmentId(
                                                {
                                                    appointmentId: appointment.appointmentID
                                                }
                                            )
                                        }
                                    >
                                        {t24}
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                </>
            )}

            {appointmentDetail && (
                <AppointmentDetailBox
                    appointmentDetail={appointmentDetail}
                    setAppointmentDetail={setAppointmentDetail}
                />
            )}

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}
        </div>
    )
}

export default DoctorAppointment;