// Modules
import { useState } from 'react';

// Styling sheet
import './AppointmentList.css';

// Assets
import PersonIcon from '../../uploads/icon/person.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import ZoomIcon from '../../uploads/icon/zoom.png';
import TicketIcon from '../../uploads/icon/ticket.png';

// Components
import Icon from '../Icon/Icon';
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import DoctorProfileBox from '../DoctorProfileBox/DoctorProfileBox';
import TextBox from '../TextBox/TextBox';
import ConfirmBox from '../ConfirmBox/ConfirmBox';

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
    const t14m1 = 'Trực tuyến';
    const t14 = 'Xem';
    const t16 = 'Xem bác sĩ';
    const t17 = 'Hủy lịch';
    const t18 = 'Số thứ tự';
    const t19 = 'Bạn có muốn hủy lịch hẹn?';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const [reload, setReload] = useState(0);
    const [openNote, setOpenNote] = useState(null);
    const [openReason, setOpenReason] = useState(null);
    const [confirmCancel, setConfirmCancel] = useState(null);

    const {
        appointments
    } = useLoadAppointments({ user, setError, setLoading, reload });

    const {
        getDoctorById, doctor, setDoctor
    } = useGetDoctorById({ setError, setLoading });

    const {
        cancelAppointment
    } = useCancelAppointment({ setError, setLoading, setReload });

    return (
        <div className="appointment-list">
            {loading && <SkeletonUI />}
            {error && <ErrorBox error={error} setError={setError} />}
            <div className="appointment-table-wrapper">
                <table className="appointment-table">
                    <thead className='header'>
                        <tr>
                            <th>
                                <div className='title'>
                                    <Icon src={PersonIcon} alt="person-icon" /> {t1}
                                </div>
                            </th>
                            <th>
                                <div className='title'>
                                    <Icon src={ScheduleIcon} alt="schedule-icon" /> {t3}
                                </div>
                            </th>
                            <th>
                                <div className='title'>
                                    <Icon src={TimeIcon} alt="time-icon" /> {t4}
                                </div>
                            </th>
                            <th>
                                <div className='title'>
                                    <Icon src={TicketIcon} alt="ticket-icon" /> {t18}
                                </div>
                            </th>
                            <th>
                                <div className='title'>
                                    <Icon src={LocationIcon} alt="location-icon" /> {t5}
                                </div>
                            </th>
                            <th>
                                <div className='title'>
                                    <Icon src={ZoomIcon} alt="zoom-icon" /> {t6}
                                </div>
                            </th>
                            <th>{t11}</th>
                            <th>{t12}</th>
                            <th>{t14}/{t17}</th>
                        </tr>
                    </thead>
                    <tbody className='body'>
                        {appointments?.map(appointment => (
                            <tr key={appointment.appointmentID}>
                                <td>{t9} {appointment.doctorName}</td>
                                <td>{appointment.date?.split("T")[0]}</td>
                                <td>{appointment.startTime} {t7} {appointment.endTime}</td>
                                <td>{appointment.queueNumber}</td>
                                <td>{appointment.zoomLink ? t14m1 : appointment.location}</td>
                                <td>
                                    {appointment.zoomLink &&
                                        <a href={appointment.zoomLink} target="_blank" rel="noopener noreferrer" className="zoom-link">
                                            {t10}
                                        </a>
                                    }
                                </td>
                                <td>
                                    {appointment.notes && (
                                        <button className='view-note' onClick={() =>
                                            setOpenNote(appointment.notes)
                                        }>
                                            {t14}
                                        </button>
                                    )}
                                </td>
                                <td>
                                    {appointment.reason && (
                                        <button className='view-reason' onClick={() =>
                                            setOpenReason(appointment.reason)
                                        }>
                                            {t14}
                                        </button>
                                    )}
                                </td>
                                <td>
                                    <button
                                        className="view"
                                        onClick={() => getDoctorById({ doctorId: appointment.doctorID })}
                                    >
                                        {t16}
                                    </button>
                                    <button
                                        className="cancel"
                                        onClick={() => setConfirmCancel({ appointmentId: appointment.appointmentID })}
                                    >
                                        {t17}
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {confirmCancel && (
                <ConfirmBox action={() => cancelAppointment(confirmCancel)} text={t19} setOpenConfirm={setConfirmCancel} />
            )}

            {openNote && (
                <TextBox setText={setOpenNote} text={openNote} title={t11} />
            )}

            {openReason && (
                <TextBox setText={setOpenReason} text={openReason} title={t12} />
            )}

            {doctor && (
                <DoctorProfileBox setViewDoctor={setDoctor} viewDoctor={doctor} setChoosenDoctor={null} />
            )}
        </div>
    );

}

export default AppointmentList;