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

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const [menuOpen, setMenuOpen] = useState(true);
    const navigate = useNavigate();

    const {
        logout
    } = useLogout({ setReloadCookies, setError, setLoading });


    return (
        <div className='nav-bar'>
            <button className="menu-toggle" onClick={() => setMenuOpen(!menuOpen)}>
                ☰
            </button>

            {menuOpen && (
                <div className='nav-bar-detail'>
                    <div className='logo'>
                        <img src={LogoPicture} alt='logo-picture' />
                        <img src={LogoText} alt='logo-text' />
                    </div>

                    <div className='navigation'>
                        <div onClick={() => navigate('/homePage')} className='home'>
                            <Icon src={HomeIcon} alt={'home-icon'} />
                            <span className="nav-text">{t1}</span>
                        </div>

                        {user && (
                            <div onClick={() => navigate('/infoPage')} className='info'>
                                <Icon src={UserIcon} alt={'info-icon'} />
                                {/* &nbsp;&nbsp;&nbsp;{t6} */}
                                <span className="nav-text">{t6}</span>
                            </div>
                        )}

                        <div onClick={() => navigate('/blogPage')} className='blog'>
                            <Icon src={BookIcon} alt={'blog-icon'} />
                            <span className="nav-text">{t2}</span>
                        </div>

                        {user && (
                            <div onClick={() => navigate('/appointmentPage')} className='appointment'>
                                <Icon src={AppointmentIcon} alt={'appointment-icon'} />
                                <span className="nav-text">{t3}</span>
                            </div>
                        )}
                    </div>

                    {user ? (
                        <div className='authentication'>
                            <button onClick={() => logout()} className='logout'>
                                {t7}
                            </button>
                        </div>
                    ) : (
                        <div className='authentication'>
                            <button onClick={() => navigate('/login')} className='login'>
                                {t4}
                            </button>
                            <button onClick={() => navigate('/register')} className='register'>
                                {t5}
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
            )}
        </div>
    )
}

export default NavBar;