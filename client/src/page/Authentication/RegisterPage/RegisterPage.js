import './RegisterPage.css';
import { Link } from 'react-router-dom';
import PhoneInput from 'react-phone-input-2';
import 'react-phone-input-2/lib/style.css';
import Icon from '../../../uploads/logo-nobg.png';

import useRegister from '../../../hook/useRegister';

function RegisterPage() {
    const {
        error,
        setEmail,
        setPassword,
        setConfirmedPassword,
        setName,
        setAge,
        setSex,
        phone, setPhone,
        handleSubmit
    } = useRegister();

    return (
        <div className='register-page'>
            <div className='background-image'></div>
            <div className='register'>
                <div className="logo">
                    <Link to="/home"><img src={Icon} alt='Logo of You are heard' /></Link>
                </div>
                <form onSubmit={handleSubmit}>
                    <fieldset>
                        <legend>Account information</legend>
                        <div className='input-group'>
                            <label>User email</label>
                            <input
                                type="email"
                                placeholder="User email"
                                required
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>
                        <div className='input-group'>
                            <label>Password</label>
                            <input
                                type="password"
                                placeholder="Password"
                                required
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>
                        <div className='input-group'>
                            <label>Confirmed password</label>
                            <input
                                type="password"
                                placeholder="Confirmed password"
                                required
                                onChange={(e) => setConfirmedPassword(e.target.value)}
                            />
                        </div>
                    </fieldset>

                    <fieldset>
                        <legend>Personal information</legend>
                        <div className='input-group'>
                            <label>User full name</label>
                            <input
                                type="text"
                                placeholder="User full name"
                                required
                                minLength={10}
                                maxLength={99}
                                onChange={(e) => setName(e.target.value)}
                            />
                        </div>
                        <div className='input-group'>
                            <label>User age</label>
                            <input
                                type="number"
                                placeholder="Age"
                                required
                                min={1}
                                max={100}
                                onChange={(e) => setAge(e.target.value)}
                            />
                        </div>
                        <div className='input-group'>
                            <label>User gender</label>
                            <select
                                required
                                onChange={(e) => setSex(e.target.value)}
                            >
                                <option value='male'>Male</option>
                                <option value='female'>Female</option>
                                <option value='other'>Other</option>
                            </select>
                        </div>
                        <div className='input-group'>
                            <label>User phone</label>
                            <PhoneInput
                                country={'vn'} // default to Vietnam
                                onChange={(value, data) => setPhone(`+${value}`)}
                                enableSearch={true}
                                inputProps={{
                                    name: 'phone',
                                    required: true,
                                    autoFocus: true
                                }}
                                placeholder={phone}
                            />
                        </div>
                    </fieldset>
                    <button type="submit">Sign up</button>
                </form>

                <div className='error'>{error}</div>
                <div className="register-directing">
                    <Link to="/login">You already have an account? Login here</Link>
                </div>
            </div>
        </div>
    )
}

export default RegisterPage;