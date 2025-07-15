// Modules
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';

// Styling sheet
import './NavBar.css';

// Assets
import AppointmentIcon from '../../uploads/icon/appointment.png';
import BookIcon from '../../uploads/icon/book.png';
import HomeIcon from '../../uploads/icon/home.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';
import UserIcon from '../../uploads/icon/user.png';
import ProfileIcon from '../../uploads/icon/profile.png';
import LogoutIcon from '../../uploads/icon/logout.png';
import SigninIcon from '../../uploads/icon/signin.png';
import LoginIcon from '../../uploads/icon/login.png';

// Components
import Icon from '../Icon/Icon';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import ErrorBox from '../ErrorBox/ErrorBox';

// Hooks
import useLogout from '../../hook/useLogout';

function NavBar({ user, setReloadCookies }) {
    const t1 = 'Trang chủ';
    const t2 = 'Blog';
    const t3 = 'Đặt lịch hẹn';
    const t4 = 'Đăng nhập';
    const t5 = 'Đăng ký';
    const t6 = 'Thông tin người dùng';
    const t7 = 'Đăng xuất';
    const t8 = 'Hồ sơ cá nhân';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const navigate = useNavigate();

    const {
        logout
    } = useLogout({ setReloadCookies, setError, setLoading });


    return (
        <div className='nav-bar'>
            <div className='nav-bar-detail'>
                <div className='logo'>
                    <img src={LogoPicture} alt='logo-picture' />
                    <img src={LogoText} alt='logo-text' />
                </div>

                <div className='navigation'>
                    <div onClick={() => navigate('/homePage')} className='home item'>
                        {/* <Icon src={HomeIcon} alt={'home-icon'} /> */}
                            <span>&nbsp;&nbsp;&nbsp;{t1}</span>
                    </div>

                    {user && (
                        <div onClick={() => navigate('/infoPage')} className='info item'>
                            {/* <Icon src={UserIcon} alt={'info-icon'} /> */}
                            <span>&nbsp;&nbsp;&nbsp;{t6}</span>
                        </div>
                    )}

                    <div onClick={() => navigate('/blogPage')} className='blog item'>
                        {/* <Icon src={BookIcon} alt={'blog-icon'} /> */}
                            <span>&nbsp;&nbsp;&nbsp;{t2}</span>
                    </div>

                    {user && (
                        <div onClick={() => navigate('/appointmentPage')} className='appointment item'>
                            {/* <Icon src={AppointmentIcon} alt={'appointment-icon'} /> */}
                            <span>&nbsp;&nbsp;&nbsp;{t3}</span>
                        </div>
                    )}
                </div>

                {user ? (
                    <div className='authentication'>
                        <button onClick={() => navigate('/patientProfile')} className='profile item'>
                            {/* <Icon src={ProfileIcon} alt={'profile-icon'} /> */}
                            <span>{t8}</span>
                        </button>
                        <button onClick={() => logout()} className='logout item'>
                            {/* <Icon src={LogoutIcon} alt={'logout-icon'} /> */}
                            <span>{t7}</span>
                        </button>
                    </div>
                ) : (
                    <div className='authentication non-login'>
                        <button onClick={() => navigate('/login')} className='login item'>
                            {/* <Icon src={LoginIcon} alt={'login-icon'} /> */}
                            <span>{t4}</span>
                        </button>
                        <button onClick={() => navigate('/register')} className='register item'>
                            {/* <Icon src={SigninIcon} alt={'signin-icon'} /> */}
                            <span>{t5}</span>
                        </button>
                    </div>
                )}

                {loading && (
                    <SkeletonUI />
                )}

                {error && (
                    <ErrorBox error={error} setError={setError} />
                )}
            </div>
        </div>
    )
}

export default NavBar;