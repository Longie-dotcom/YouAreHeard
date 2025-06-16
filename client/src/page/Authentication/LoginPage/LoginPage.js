// Modules
import { useNavigate } from 'react-router-dom';
import { useState, useRef } from 'react';

// Styling sheet
import './LoginPage.css';

// Assets
import LogoPicture from '../../../uploads/logo-picture.png';

// Components
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';

// Hooks
import useLogin from '../../../hook/useLogin';

function LoginPage({ setReloadCookies }) {

    // Texts
    const t1 = 'Đăng nhập vào tài khoản';
    const t2 = 'Hoặc';
    const t3 = 'tạo tài khoản mới';
    const t4 = 'Địa chỉ email';
    const t5 = 'your@email.com';
    const t6 = 'Mật khẩu';
    const t7 = 'Nhập mật khẩu';
    const t8 = 'Ghi nhớ đăng nhập';
    const t9 = 'Quên mật khẩu?';
    const t10 = 'Đăng nhập';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const navigate = useNavigate();
    const passwordRef = useRef(null);

    const {
        handleSubmit, setEmail, setPassword
    } = useLogin({ setReloadCookies, setError, setLoading });

    return (
        <div id='login-page'>
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
                        <span onClick={() => navigate('/register')} className='register'>
                            {t3}
                        </span>
                    </p>
                </div>
            </div>

            <div className='body'>
                <div className='input-group'>
                    <label htmlFor="email">{t4}</label>
                    <input
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') {
                                e.preventDefault();
                                passwordRef.current?.focus();
                            }
                        }}
                        onChange={(e) => setEmail(e.target.value)}
                        type="email" id="email" name="email" placeholder={t5}
                    />
                </div>
                <div className='input-group'>
                    <label htmlFor="password">{t6}</label>
                    <input
                        ref={passwordRef}
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') {
                                handleSubmit();
                            }
                        }}
                        onChange={(e) => setPassword(e.target.value)}
                        type="password" id="password" name="password" placeholder={t7}
                    />
                </div>
            </div>

            <div className='footer'>
                <button
                    onClick={() => handleSubmit()}
                    className='submit-button'
                >
                    {t10}
                </button>
            </div>

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}
        </div>
    )
}

export default LoginPage;