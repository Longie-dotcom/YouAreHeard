// Modules
import { useState } from 'react';

// Styling sheet
import './AppointmentList.css';

// Assets
import UserIcon from '../../uploads/icon/user.png';
import PersonIcon from '../../uploads/icon/person.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import ZoomIcon from '../../uploads/icon/zoom.png';

// Components
import Icon from '../Icon/Icon';
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import DoctorProfileBox from '../DoctorProfileBox/DoctorProfileBox';

// Hooks
import useLoadAppointments from '../../hook/useLoadAppointments';
import useGetDoctorById from '../../hook/useGetDoctorById';
import useCancelAppointment from '../../hook/useCancelAppointment';

function AppointmentList({ user }) {
    const t1 = 'Bác sĩ';
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
    const t14 = 'Xem';
    const t15 = 'Đóng';
    const t16 = 'Xem bác sĩ';
    const t17 = 'Hủy lịch';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);    
    const [openNote, setOpenNote] = useState(null);
    const [openReason, setOpenReason] = useState(null);

    const {
        appointments,
    } = useLoadAppointments({ user, setError, setLoading });

    const {
        getDoctorById, doctor, setDoctor
    } = useGetDoctorById({ setError, setLoading });

    const {
        cancelAppointment
    } = useCancelAppointment({ setError, setLoading });

    return (
        <div className="appointment-list">
            {appointments && appointments.map((appointment) => (
                <div key={appointment.appointmentID} className="appointment">
                    <div className="doctor">
                        <div className="title">
                            <Icon src={PersonIcon} alt={'person-icon'} />
                            {t1}
                        </div>
                        <div className="detail">
                            <p className="name">{t9}&nbsp;{appointment.doctorName}</p>
                        </div>
                    </div>

                    <div className="patient">
                        <div className="title">
                            <Icon src={UserIcon} alt={'person-icon'} />
                            {t2}
                        </div>
                        <div className="detail">
                            <p className="name">{appointment.patientName}</p>
                        </div>
                    </div>

                    <div className="date">
                        <div className="title">
                            <Icon src={ScheduleIcon} alt={'person-icon'} />
                            {t3}
                        </div>
                        <div className="detail">
                            {appointment.date?.split("T")[0]}
                        </div>
                    </div>

                    <div className="time">
                        <div className="title">
                            <Icon src={TimeIcon} alt={'person-icon'} />
                            {t4}
                        </div>
                        <div className="detail">
                            {appointment.startTime}&nbsp;{t7}&nbsp;{appointment.endTime}
                        </div>
                    </div>

                    {!appointment.zoomLink && (
                        <div className="location">
                            <div className="title">
                                <Icon src={LocationIcon} alt={'person-icon'} />
                                {t5}
                            </div>
                            <div className="detail">
                                {appointment.zoomLink ? t8 : appointment.location}
                            </div>
                        </div>
                    )}

                    {appointment.zoomLink && (
                        <div className="zoom">
                            <div className="title">
                                {t6}
                            </div>
                            <div className="detail">
                                <a href={appointment.zoomLink} target="_blank" rel="noopener noreferrer" className='zoom-link'>
                                    <Icon src={ZoomIcon} alt={'zoom-icon'} />
                                    &nbsp;&nbsp;&nbsp;{t10}
                                </a>
                            </div>
                        </div>
                    )}

                    {appointment.notes && (
                        <div className="note">
                            <div className="title">
                                <button onClick={() =>
                                    setOpenNote(openNote === appointment.appointmentID ? null : appointment.appointmentID)
                                }>
                                    {openNote === appointment.appointmentID ? t15 : t14}
                                </button>&nbsp;&nbsp;&nbsp;&nbsp;{t11}
                            </div>

                            {openNote === appointment.appointmentID && (
                                <div className="detail">
                                    {appointment.notes}
                                </div>
                            )}
                        </div>
                    )}

                    {appointment.reason && (
                        <div className="reason">
                            <div className="title">
                                <button onClick={() =>
                                    setOpenReason(openReason === appointment.appointmentID ? null : appointment.appointmentID)
                                }>
                                    {openReason === appointment.appointmentID ? t15 : t14}
                                </button>&nbsp;&nbsp;&nbsp;&nbsp;{t12}
                            </div>

                            {openReason === appointment.appointmentID && (
                                <div className="detail">
                                    {appointment.reason}
                                </div>
                            )}
                        </div>
                    )}

                    {appointment.appointmentStatusName && (
                        <div className="status">
                            <div className="title">
                                {t14m1}
                            </div>
                            <div className="detail">
                                {appointment.appointmentStatusName}
                            </div>
                        </div>
                    )}

                    <div className='footer'>
                        <button
                            className='view'
                            onClick={() => getDoctorById({ doctorId: appointment.doctorID })}>
                            {t16}
                        </button>
                        <button
                            onClick={() => cancelAppointment({ appointmentId: appointment.appointmentID })}
                            className='cancel'
                        >
                            {t17}
                        </button>
                    </div>
                </div>
            ))}

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {doctor && (
                <DoctorProfileBox setViewDoctor={setDoctor} viewDoctor={doctor} />
            )}
        </div>
    )
}

export default AppointmentList;