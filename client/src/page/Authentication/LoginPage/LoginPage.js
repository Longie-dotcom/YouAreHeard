import './LoginPage.css';
import { Link } from 'react-router-dom';
import Icon from '../../../uploads/logo-nobg.png';

import useLogin from '../../../hook/useLogin';

function LoginPage({ setReloadCookies }) {
    const {
        error,
        setEmail,
        setPassword,
        handleSubmit
    } = useLogin({ setReloadCookies });

    return (
        <div className='login-page'>
            <div className='background-image'></div>
            <div className='login'>
                <div className="logo">
                    <Link to="/home"><img src={Icon} alt='Logo of You are heard' /></Link>
                </div>
                <form onSubmit={handleSubmit}>
                    <div className='input-group'>
                        <input type="email" placeholder="User email" required onChange={(e) => setEmail(e.target.value)} />
                        <i className="bi bi-person-fill"></i>
                    </div>
                    <div className='input-group'>
                        <input type="password" placeholder="Password" required onChange={(e) => setPassword(e.target.value)} />
                        <i className="bi bi-lock-fill"></i>
                    </div>
                    <button type="submit">Submit</button>
                </form>
                <div className='error'>{error}</div>
                <div className="register-directing">
                    <Link to="/register">You dont have an account? Register here</Link>
                </div>
            </div>
        </div>
    )
}

export default LoginPage;