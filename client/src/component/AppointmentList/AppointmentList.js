// Modules
import { useState } from 'react';

// Styling sheet
import './AppointmentList.css';

// Assets
import PersonIcon from '../../uploads/icon/person.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import LinkIcon from '../../uploads/icon/link.png';
import TicketIcon from '../../uploads/icon/ticket.png';
import ConfirmIcon from '../../uploads/icon/confirm.png';

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
    const t2 = 'Trạng thái';
    const t3 = 'Ngày';
    const t4 = 'Thời gian';
    const t5 = 'Địa điểm';
    const t6 = 'Truy cập';
    const t7 = '-';
    const t8 = 'Người dùng chưa có cuộc hẹn nào sắp tới';
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
    const t20 = 'Mã QR';
    const t21 = 'Mã QR định danh của người dùng';
    const t22 = 'Mã định danh của người dùng:';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const [reload, setReload] = useState(0);
    const [openNote, setOpenNote] = useState(null);
    const [openReason, setOpenReason] = useState(null);
    const [confirmCancel, setConfirmCancel] = useState(null);
    const [openQR, setOpenQR] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const qrApi = process.env.REACT_APP_QRCODE_ASSET_API;

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
                {appointments && appointments.length > 0 ? (
                    <>
                        <table className="appointment-table">
                            <thead className='header'>
                                <tr>
                                    <th>
                                        <div className='title'>
                                            <Icon src={ConfirmIcon} alt="confirm-icon" /> {t2}
                                        </div>
                                    </th>
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
                                            <Icon src={LinkIcon} alt="zoom-icon" /> {t6}
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
                                        <td><span className={`status status-${appointment.appointmentStatusID}`}>{appointment.appointmentStatusName}</span></td>
                                        <td>{t9} {appointment.doctorName}</td>
                                        <td>{appointment.scheduleDate?.split("T")[0]}</td>
                                        <td>{appointment.startTime} {t7} {appointment.endTime}</td>
                                        <td>{appointment.queueNumber}</td>
                                        <td>{appointment.zoomLink ? t14m1 : appointment.location}</td>
                                        <td>
                                            {appointment.zoomLink ?
                                                (
                                                    <a href={appointment.zoomLink} target="_blank" rel="noopener noreferrer" className="zoom-link">
                                                        {t10}
                                                    </a>
                                                ) : (
                                                    <button
                                                        className='view-qr'
                                                        onClick={() => {
                                                            setOpenQR(appointment);
                                                        }}
                                                    >
                                                        {t20}
                                                    </button>
                                                )
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
                    </>
                ) : (
                    <div className='empty'>
                        {t8}
                    </div>
                )}
            </div>

            <div className="appointment-card-list">
                {appointments && appointments.length > 0 ? (
                    <>
                        {appointments?.map(appointment => (
                            <div className="appointment-card" key={appointment.appointmentID}>
                                <div className="row">
                                    <div className="title">
                                        <Icon src={ConfirmIcon} alt="confirm-icon" /> {t2}
                                    </div>
                                    <div className={`content status status-${appointment.appointmentStatusID}`}>
                                        {appointment.appointmentStatusName}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={PersonIcon} alt="person-icon" /> {t1}
                                    </div>
                                    <div className="content">
                                        {t9} {appointment.doctorName}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={ScheduleIcon} alt="schedule-icon" /> {t3}
                                    </div>
                                    <div className="content">
                                        {appointment.scheduleDate?.split("T")[0]}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={TimeIcon} alt="time-icon" /> {t4}
                                    </div>
                                    <div className="content">
                                        {appointment.startTime} {t7} {appointment.endTime}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={TicketIcon} alt="ticket-icon" /> {t18}
                                    </div>
                                    <div className="content">
                                        {appointment.queueNumber}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={LocationIcon} alt="location-icon" /> {t5}
                                    </div>
                                    <div className="content">
                                        {appointment.zoomLink ? t14m1 : appointment.location}
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="title">
                                        <Icon src={LinkIcon} alt="link-icon" /> {t6}
                                    </div>
                                    <div className="content">
                                        {appointment.zoomLink ? (
                                            <a href={appointment.zoomLink} target="_blank" rel="noopener noreferrer" className="zoom-link">
                                                {t10}
                                            </a>
                                        ) : (
                                            <button className="view-qr" onClick={() => setOpenQR(appointment)}>{t20}</button>
                                        )}
                                    </div>
                                </div>

                                {appointment.notes && (
                                    <div className="row">
                                        <div className="title">{t11}</div>
                                        <div className="content">
                                            <button className="view-note" onClick={() => setOpenNote(appointment.notes)}>{t14}</button>
                                        </div>
                                    </div>
                                )}

                                {appointment.reason && (
                                    <div className="row">
                                        <div className="title">{t12}</div>
                                        <div className="content">
                                            <button className="view-reason" onClick={() => setOpenReason(appointment.reason)}>{t14}</button>
                                        </div>
                                    </div>
                                )}

                                <div className="row">
                                    <div className="title">{t17}</div>
                                    <div className="content buttons">
                                        <button className="cancel" onClick={() => setConfirmCancel({ appointmentId: appointment.appointmentID })}>
                                            {t17}
                                        </button>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="title">{t16}</div>
                                    <div className="content buttons">
                                        <button className="view" onClick={() => getDoctorById({ doctorId: appointment.doctorID })}>
                                            {t16}
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </>
                ) : (
                    <div className='empty'>
                        {t8}
                    </div>
                )}
            </div>

            {confirmCancel && (
                <ConfirmBox action={() => cancelAppointment(confirmCancel)} title={t19} setOpenConfirm={setConfirmCancel} icon={ConfirmIcon} />
            )}

            {openNote && (
                <TextBox setText={setOpenNote} text={openNote} title={t11} />
            )}

            {openReason && (
                <TextBox setText={setOpenReason} text={openReason} title={t12} />
            )}

            {openQR && (
                <TextBox setText={setOpenQR} text={
                    <>
                        <img src={`${serverApi}${qrApi}/${openQR.orderCode}.png`} />
                        <div className='qr-code-detail'>
                            <span className='title'>{t22}</span>&nbsp;<span>{openQR.orderCode}</span>
                        </div>
                    </>
                } title={t21} />
            )}

            {doctor && (
                <DoctorProfileBox setViewDoctor={setDoctor} viewDoctor={doctor} setChoosenDoctor={null} />
            )}
        </div>
    );

}

export default AppointmentList;