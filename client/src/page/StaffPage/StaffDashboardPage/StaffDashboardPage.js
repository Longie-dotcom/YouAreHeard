// Modules
import { useState } from 'react';

// Style sheet
import './StaffDashboardPage.css';

// Assets
import ConfirmIcon from '../../../uploads/icon/confirm.png';
import ScheduleIcon from '../../../uploads/icon/schedule.png';
import PersonIcon from '../../../uploads/icon/person.png';
import NoteIcon from '../../../uploads/icon/note.png';
import LogoText from '../../../uploads/logo-text.png';
import LogoPicture from '../../../uploads/logo-picture.png';

// Components
import Icon from '../../../component/Icon/Icon';
import SideMenu from '../../../component/SideMenu/SideMenu';
import PostBlogBox from '../../../component/PostBlogBox/PostBlogBox';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import TextBox from '../../../component/TextBox/TextBox';
import ManageAppointmentBox from '../../../component/ManageAppoinmentBox/ManageAppointmentBox';
import NextAppointment from '../../../component/NextAppointment/NextAppointment';

// Hooks
import useLogout from '../../../hook/useLogout';
import useGetAllAppointments from '../../../hook/useGetAllAppointments';
import useLoadDoctorAppointments from '../../../hook/useLoadDoctorAppointments';

function StaffDashboardPage({ user, setReloadCookies }) {
    const t1 = 'Đăng xuất';
    const t2 = '⬆ Lên đầu';
    const t3 = 'Hiện tư vấn viên chưa có lịch tư vấn';
    const menuItems = [
        { id: 'blog', label: 'Tạo bài viết', icon: NoteIcon },
        { id: 'appointment', label: 'Quản lý lịch hẹn', icon: ScheduleIcon },
        { id: 'consultant', label: 'Tư vấn trực tuyến', icon: PersonIcon }
    ];

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const [finish, setFinish] = useState(false);

    const [openSection, setOpenSection] = useState('appointment');
    const [reload, setReload] = useState(0);

    const { appointments } = useGetAllAppointments({ setError, setLoading, reload });
    const { appointments: nextAppointment } = useLoadDoctorAppointments({ setError, setLoading, doctorId: user.UserId });
    const { logout } = useLogout({ setReloadCookies, setError, setLoading });
    return (
        <div id='staff-dashboard-page' className={openSection === 'appointment' ? 'expand' : '' }>
            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {loading && (
                <SkeletonUI />
            )}

            {finish && (
                <TextBox setText={setFinish} text={finish} title={<Icon src={ConfirmIcon} alt={'confirm-icon'} />} />
            )}

            <button
                className="scroll-to-top"
                onClick={() => window.scrollTo({ top: 0, behavior: 'smooth' })}
            >
                {t2}
            </button>

            <div className='header'>
                <div className='logo'>
                    <img src={LogoPicture} alt='logo' />
                    <img src={LogoText} alt='logo' />
                </div>

                <button
                    onClick={() => logout()}
                >
                    {t1}
                </button>
            </div>

            <div className='body'>
                <div className='menu'>
                    <SideMenu
                        menuItems={menuItems}
                        openSection={openSection}
                        setOpenSection={setOpenSection}
                        appointments={appointments}
                    />
                </div>

                <div className='main'>
                    {openSection === 'blog' && (
                        <PostBlogBox
                            user={user}
                            setError={setError}
                            setLoading={setLoading}
                            setFinish={setFinish}
                        />
                    )}

                    {openSection === 'appointment' && (
                        <ManageAppointmentBox
                            setError={setError}
                            setFinish={setFinish}
                            setLoading={setLoading}
                            user={user}
                            setReload={setReload}
                            appointments={appointments}
                        />
                    )}

                    {openSection === 'consultant' && (
                        <NextAppointment user={user} appointments={nextAppointment} emptyText={t3} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default StaffDashboardPage;