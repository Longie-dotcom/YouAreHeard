// Modules
import { useNavigate } from 'react-router-dom';

// Styling sheet
import './NavBar.css';

// Assets
import AppointmentIcon from '../../uploads/icon/appointment.png';
import BookIcon from '../../uploads/icon/book.png';
import HomeIcon from '../../uploads/icon/home.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function NavBar({ user }) {
    const t1 = 'Trang chủ';
    const t2 = 'Blog';
    const t3 = 'Đặt lịch hẹn';
    const t4 = 'Đăng nhập';
    const t5 = 'Đăng ký';

    const navigate = useNavigate();

    return (
        <div className='nav-bar'>
            <div className='logo'>
                <img src={LogoPicture} alt='logo-picture' />
                <img src={LogoText} alt='logo-text' />
            </div>

            <div className='navigation'>
                <div onClick={() => navigate('homePage')} className='home'>
                    <Icon src={HomeIcon} alt={'home-icon'} />
                    &nbsp;&nbsp;&nbsp;{t1}
                </div>

                <div onClick={() => navigate('blogPage')} className='blog'>
                    <Icon src={BookIcon} alt={'blog-icon'} />
                    &nbsp;&nbsp;&nbsp;{t2}
                </div>

                <div onClick={() => navigate('appointmentPage')} className='appointment'>
                    <Icon src={AppointmentIcon} alt={'appointment-icon'} />
                    &nbsp;&nbsp;&nbsp;{t3}
                </div>
            </div>

            {user ? (
                <div className='authentication'>
                    User name
                </div>
            ) : (
                <div className='authentication'>
                    <button onClick={() => navigate('login')} className='login'>
                        {t4}
                    </button>
                    <button onClick={() => navigate('register')}  className='register'>
                        {t5}
                    </button>
                </div>
            )}

        </div>
    )
}

export default NavBar;