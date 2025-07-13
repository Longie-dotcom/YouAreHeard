// Modules
import { useState } from 'react';

// Styling sheet
import './ManageAppointmentBox.css';

// Assets
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';
import PersonIcon from '../../uploads/icon/person.png';


// Components
import AppointmentForm from '../AppointmentForm/AppointmentForm';
import Icon from '../Icon/Icon';

// Hooks
import useVerifyAppointment from '../../hook/useVerifyAppointment';

function ManageAppointmentBox({ user, setLoading, setError, setFinish, appointments, setReload }) {
    const t1 = 'Không có cuộc hẹn nào';
    const t2 = 'ID';
    const t3 = 'Bệnh nhân';
    const t4 = 'Bác sĩ';
    const t5 = 'Ngày';
    const t6 = 'Giờ';
    const t7 = 'Số thứ tự';
    const t8 = 'Trực tuyến';
    const t9 = 'Trạng thái';
    const t10 = 'Có';
    const t11 = 'Không';
    const t12 = 'Xác minh';

    const TEXT = {
        identityInputHolder: 'Mã định danh của cuộc hẹn',
        title: 'Tìm kiếm thông tin lịch hẹn của bệnh nhân đăng ký',
        successTitle: 'Thông tin lịch của bệnh nhân',
        doctor: 'Bác sĩ',
        date: 'Ngày',
        time: 'Thời gian',
        type: 'Loại cuộc hẹn',
        queue: 'Số thứ tự',
        zoomInfo: 'Đường dẫn của Zoom meeting đã được gửi tới',
        offline: 'Khám/Điều trị (Offline)',
        online: 'Tư vấn trực tiếp (Online)',
        continue: 'Tiếp tục đặt hẹn',
        home: 'Về trang chủ',
        doctorPrefix: 'BS.',
        separator: '-',
        location: 'Địa điểm',
        qrCode: 'Mã QR',
        patientName: 'Tên của bệnh nhân',
        patientPhone: 'Số điện thoại'
    };

    const [orderCode, setOrderCode] = useState(null);
    const [openUpdateAppointment, setOpenUpdateAppointment] = useState(null);
    const {
        verify, result
    } = useVerifyAppointment({ setError, setLoading });

    return (
        <div className='manage-appointment-box'>
            {openUpdateAppointment && (
                <AppointmentForm
                    setError={setError}
                    setLoading={setLoading}
                    setFinish={setFinish}
                    setReload={setReload}
                    openUpdateAppointment={openUpdateAppointment}
                    setOpenUpdateAppointment={setOpenUpdateAppointment}
                />
            )}

            <div className='identity-search'>
                <div className='header'>
                    {TEXT.title}
                </div>
                <div className='input'>
                    <input
                        onChange={(e) => setOrderCode(e.target.value)}
                        placeholder={TEXT.identityInputHolder}
                    />
                    <button
                        onClick={() => verify({ orderCode })}
                        className='verify'>
                        {t12}
                    </button>
                </div>
                {result && (
                    <div class='verify-info'>
                        <div className='finish'>
                            <div className='header'>
                                <div className='header-title'>
                                    <Icon src={PersonIcon} alt='person-icon' />
                                    {TEXT.successTitle}
                                </div>

                                <div className='logo'>
                                    <img src={LogoPicture} alt='logo-picture' />
                                    <img src={LogoText} alt='logo-text' />
                                </div>
                            </div>

                            <div className='body'>
                                <div className='finish-detail'>
                                    <div className='doctor-name'>
                                        <div className='title'>{TEXT.doctor}</div>
                                        <div className='detail'>{TEXT.doctorPrefix} {result.doctorName}</div>
                                    </div>

                                    <div className='patient-name'>
                                        <div className='title'>{TEXT.patientName}</div>
                                        <div className='detail'>{result.patientName}</div>
                                    </div>

                                    <div className='patient-phone'>
                                        <div className='title'>{TEXT.patientPhone}</div>
                                        <div className='detail'>{result.patientPhone}</div>
                                    </div>

                                    <div className='date'>
                                        <div className='title'>{TEXT.date}</div>
                                        <div className='detail'>{result.scheduleDate?.split('T')[0]}</div>
                                    </div>

                                    <div className='time'>
                                        <div className='title'>{TEXT.time}</div>
                                        <div className='detail'>
                                            {result.startTime} {TEXT.separator} {result.endTime}
                                        </div>
                                    </div>

                                    <div className='location'>
                                        <div className='title'>{TEXT.location}</div>
                                        <div className='detail'>
                                            {result.location}
                                        </div>
                                    </div>

                                    <div className='date'>
                                        <div className='title'>{TEXT.type}</div>
                                        <div className='detail'>
                                            {result.isOnline ? TEXT.online : TEXT.offline}
                                        </div>
                                    </div>
                                </div>

                                <div className='queue-number'>
                                    <div className='detail'>
                                        {result.queueNumber}
                                    </div>
                                    <div className='title'>
                                        {TEXT.queue}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                )}
            </div>
            <table className='appointment-table'>
                <thead>
                    <tr>
                        <th>{t2}</th>
                        <th>{t3}</th>
                        <th>{t4}</th>
                        <th>{t5}</th>
                        <th>{t6}</th>
                        <th>{t7}</th>
                        <th>{t8}</th>
                        <th>{t9}</th>
                    </tr>
                </thead>
                <tbody>
                    {appointments?.length > 0 ? (
                        appointments.map((apt) => (
                            <tr key={apt.appointmentID}>
                                <td>{apt.appointmentID}</td>
                                <td
                                    onClick={() => setOpenUpdateAppointment(apt)}
                                    className='name'
                                >
                                    {apt.patientName}
                                </td>
                                <td>{apt.doctorName}</td>
                                <td>{apt.scheduleDate.split('T')[0]}</td>
                                <td>{apt.startTime} - {apt.endTime}</td>
                                <td>{apt.queueNumber ?? '-'}</td>
                                <td>{apt.isOnline ? t10 : t11}</td>
                                <td>{apt.appointmentStatusName}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan='8'>{t1}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default ManageAppointmentBox;