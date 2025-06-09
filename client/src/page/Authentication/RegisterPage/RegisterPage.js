// Modules
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

// Style sheet
import './RegisterPage.css';

// Assets
import LogoPicture from '../../../uploads/logo-picture.png';
import EyeIcon from '../../../uploads/icon/eye.png';
import EyeBlindIcon from '../../../uploads/icon/eye-blind.png';

// Components
import Icon from '../../../component/Icon/Icon';
import OTPBox from '../../../component/OTPBox/OTPBox';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';

// Hooks
import useRegister from '../../../hook/useRegister';

function RegisterPage() {

    // Texts
    const t1 = 'Tạo tài khoản mới'
    const t2 = 'Hoặc';
    const t3 = 'đăng nhập với tài khoản sẵn có';
    const t4 = 'Họ và tên';
    const t5 = 'Nguyễn Văn A';
    const t6 = 'Địa chỉ email';
    const t7 = 'your@email.com';
    const t8 = 'Mật khẩu';
    const t9 = 'Nhập mật khẩu';
    const t10 = 'Số điện thoại';
    const t11 = '0123456789';
    const t12 = 'Ngày sinh';
    const t14m1 = 'Xác nhận mật khẩu';
    const t14 = 'Nhập lại mật khẩu';
    const t15 = 'Tôi đồng ý với'
    const t16 = 'điều khoản sử dụng';
    const t17 = 'và';
    const t18 = 'chính sách bảo mật';
    const t19 = 'Tạo tài khoản'

    // States
    const [seeConfirmedPassword, setSeeConfirmedPassword] = useState(false);
    const [seePassword, setSeePassword] = useState(false);
    const [openOTP, setOpenOTP] = useState(false);
    const navigate = useNavigate();
    const {
        loading,
        name, setName,
        email, setEmail,
        dob, setDob,
        phone, setPhone,
        password, setPassword,
        confirmedPassword, setConfirmedPassword,
        error, setError,
        handleSubmit
    } = useRegister({ setOpenOTP });

    // Functions
    const togglePasswordVisibility = (setSee) => {
        setSee(prev => !prev);
    };

    return (
        <div id='register-page'>
            <div className='header'>
                <div
                    onClick={() => navigate('/homePage')}
                    className='logo'
                >
                    <img src={LogoPicture} />
                </div>
                <div className='title'>
                    <h1>
                        {t1}
                    </h1>
                    <p>
                        <span className='normal'>
                            {t2}
                        </span>
                        &nbsp;
                        <span onClick={() => navigate('/login')} className='login'>
                            {t3}
                        </span>
                    </p>
                </div>
            </div>

            <div className='body'>
                <div className='input-group'>
                    <label for="username">{t4}</label>
                    <input
                        value={name} onChange={(e) => setName(e.target.value)}
                        type="text" id="username" name="username" placeholder={t5}
                    />
                </div>
                <div className='input-group'>
                    <label for="email">{t6}</label>
                    <input
                        value={email} onChange={(e) => setEmail(e.target.value)}
                        type="email" id="email" name="email" placeholder={t7}
                    />
                </div>
                <div className='input-group'>
                    <label for="password">{t8}</label>
                    <div className='password-input'>
                        <input
                            value={password} onChange={(e) => setPassword(e.target.value)}
                            type={seePassword ? 'text' : 'password'} id="password" name="password" placeholder={t9}
                        />
                        <button onClick={() => togglePasswordVisibility(setSeePassword)}>
                            <Icon src={seePassword ? EyeIcon : EyeBlindIcon} alt={'eye-icon'} />
                        </button>
                    </div>
                </div>
                <div className='input-group'>
                    <label for="reConfirm-password">{t14m1}</label>
                    <div className='password-input'>
                        <input
                            value={confirmedPassword} onChange={(e) => setConfirmedPassword(e.target.value)}
                            type={seeConfirmedPassword ? 'text' : 'password'} id="reConfirm-password" name="reConfirm-password" placeholder={t14}
                        />
                        <button onClick={() => togglePasswordVisibility(setSeeConfirmedPassword)}>
                            <Icon src={seeConfirmedPassword ? EyeIcon : EyeBlindIcon} alt={'eye-icon'} />
                        </button>
                    </div>
                </div>
                <div className='input-group'>
                    <label for="phone">{t10}</label>
                    <input
                        value={phone} onChange={(e) => setPhone(e.target.value)}
                        type="number" id="phone" name="phone" placeholder={t11}
                    />
                </div>
                <div className='input-group'>
                    <label for="dob">{t12}</label>
                    <input
                        value={dob} onChange={(e) => setDob(e.target.value)}
                        type="date" id="dob" name="dob"
                    />
                </div>
            </div>

            <div className='footer'>
                <div className='policy'>
                    <input type='checkbox' />
                    <div className='policy-detail'>
                        &nbsp;{t15}&nbsp;
                        <span className='usability'>
                            {t16}
                        </span>
                        &nbsp;{t17}&nbsp;
                        <span className='security'>
                            {t18}
                        </span>
                    </div>
                </div>
                <button onClick={() => handleSubmit()} className='submit-button' >
                    {t19}
                </button>
            </div>

            {openOTP && (
                <OTPBox emailSentTo={email} setOpenOTP={setOpenOTP} />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {loading && (
                <SkeletonUI />
            )}
        </div>
    )
}

export default RegisterPage;