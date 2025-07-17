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
    const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
    const navigate = useNavigate();

    const {
        logout
    } = useLogout({ setReloadCookies, setError, setLoading });

    const toggleMobileMenu = () => {
        setMobileMenuOpen(!mobileMenuOpen);
    };

    const handleNavigation = (path) => {
        navigate(path);
        setMobileMenuOpen(false);
    };

    const handleLogout = () => {
        logout();
        setMobileMenuOpen(false);
    };

    return (
        <div className='nav-bar'>
            <div className={`nav-bar-detail ${mobileMenuOpen ? 'mobile-open' : ''}`}>
                <div className='logo'>
                    <img src={LogoPicture} alt='logo-picture' />
                    <img src={LogoText} alt='logo-text' />
                </div>

                <button 
                    className={`menu-toggle ${mobileMenuOpen ? 'active' : ''}`} 
                    onClick={toggleMobileMenu}
                >
                    {/* Remove the text content since we're using CSS pseudo-elements */}
                </button>

                <div className='navigation'>
                    <div onClick={() => handleNavigation('/homePage')} className='home item'>
                        <span>&nbsp;&nbsp;&nbsp;{t1}</span>
                    </div>

                    {user && (
                        <div onClick={() => handleNavigation('/infoPage')} className='info item'>
                            <span>&nbsp;&nbsp;&nbsp;{t6}</span>
                        </div>
                    )}

                    <div onClick={() => handleNavigation('/blogPage')} className='blog item'>
                        <span>&nbsp;&nbsp;&nbsp;{t2}</span>
                    </div>

                    {user && (
                        <div onClick={() => handleNavigation('/appointmentPage')} className='appointment item'>
                            <span>&nbsp;&nbsp;&nbsp;{t3}</span>
                        </div>
                    )}
                </div>

                {user ? (
                    <div className='authentication'>
                        <button onClick={() => handleNavigation('/patientProfile')} className='profile item'>
                            <span>{t8}</span>
                        </button>
                        <button onClick={handleLogout} className='logout item'>
                            <span>{t7}</span>
                        </button>
                    </div>
                ) : (
                    <div className='authentication non-login'>
                        <button onClick={() => handleNavigation('/login')} className='login item'>
                            <span>{t4}</span>
                        </button>
                        <button onClick={() => handleNavigation('/register')} className='register item'>
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