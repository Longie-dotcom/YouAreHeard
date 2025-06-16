// Modules
import React, { useState, useRef, useEffect } from 'react';

// Styling sheet
import './OTPBox.css';

// Assets
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import SkeletonUI from '../SkeletonUI/SkeletonUI';

// Hooks
import useOTP from '../../hook/useOTP';

function OTPBox({ emailSentTo, setOpenOTP }) {
    const t1 = 'Xác minh OTP';
    const t2 = 'Hãy điền mã OTP được gửi qua'
    const t3 = 'Xác minh';
    const t4 = 'Chưa nhận được mã ?';
    const t5 = 'Gửi lại';

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    const inputs = useRef([]);

    const {
        setOtp, handleSubmit, handleRequestOTP
    } = useOTP({ emailSentTo, setError, setLoading });

    if (inputs.current.length === 0) {
        inputs.current = Array(6).fill().map(() => React.createRef());
    }

    useEffect(() => {
        inputs.current[0].current.focus()
    }, []);

    const handleChange = (e, index) => {
        const value = e.target.value;
        if (!value) return;

        if (value.length > 1) {
            const chars = value.slice(0, 6).split('');
            chars.forEach((char, i) => {
                const inputRef = inputs.current[i];
                if (inputRef && inputRef.current) {
                    inputRef.current.value = char;
                }
            });

            const lastFilledIndex = Math.min(chars.length - 1, 5);
            if (inputs.current[lastFilledIndex]?.current) {
                inputs.current[lastFilledIndex].current.focus();
            }

            return;
        }

        // If typing single char
        if (value.length === 1 && index < 5) {
            inputs.current[index + 1].current.focus();
        }

        const otpValue = inputs.current.map(ref => ref.current.value).join('');
        setOtp(otpValue);
    };

    const handleKeyDown = (e, index) => {
        if (e.key === 'Backspace' && e.target.value === '' && index > 0) {
            inputs.current[index - 1].current.focus();
        }

        if (e.key === 'Enter') {
            handleRequestOTP();
        }

        const otpValue = inputs.current.map(ref => ref.current.value).join('');
        setOtp(otpValue);
    };

    return (
        <div
            className='otp-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.otp-box')) {
                    setOpenOTP(false);
                    e.stopPropagation();
                }
            }}
        >

            <div className='otp-box'>
                <div className='logo'>
                    <img src={LogoPicture} alt='logo' />
                </div>

                <div className='otp-detail'>
                    <div className='header'>
                        <h1>
                            {t1}
                        </h1>

                        <p>
                            <span>
                                {t2}
                            </span>
                            <span className='email'>
                                &nbsp;{emailSentTo}
                            </span>
                        </p>
                    </div>

                    <div className='body'>
                        <div className="otp-container">
                            {inputs.current.map((ref, index) => (
                                <input
                                    key={index}
                                    type="text"
                                    maxLength="1"
                                    ref={ref}
                                    className="otp-input"
                                    onChange={(e) => handleChange(e, index)}
                                    onKeyDown={(e) => handleKeyDown(e, index)}
                                />
                            ))}
                        </div>
                    </div>

                    <div className='footer'>
                        <button
                            onClick={() => handleSubmit()}
                            className='submit-otp'>
                            {t3}
                        </button>

                        <p className='error'>
                            {error}
                        </p>

                        <p className='resend-otp'>
                            <span>
                                {t4}
                            </span>
                            <span
                                onClick={() => handleRequestOTP()}
                                className='resend'
                            >
                                &nbsp;{t5}
                            </span>
                        </p>
                    </div>
                </div>
            </div>

            {loading && (
                <SkeletonUI />
            )}
        </div>
    )
}

export default OTPBox;