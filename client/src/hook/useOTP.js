import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

function useOTP({ emailSentTo, setLoading, setError }) {
    const t1 = 'Không tìm thấy email!';
    const t2 = 'Không tìm thấy OTP!';
    const t3 = 'Lỗi máy chủ không xác định được!';
    const t4 = 'OTP mới vừa được gửi!';

    const [otp, setOtp] = useState(null);

    const navigate = useNavigate();

    const serverApi = process.env.REACT_APP_SERVER_API;
    const authenticationControllerApi = process.env.REACT_APP_AUTHENTICATION_CONTROLLER_API;

    const handleSubmit = async () => {
        if (!emailSentTo) {
            setError(t1);
            return;
        }

        if (!otp) {
            setError(t2);
            return;
        }

        const userData = {
            email: emailSentTo,
            otp
        };

        setLoading(true);
        try {
            const response = await axios.post(
                `${serverApi}${authenticationControllerApi}/verify-otp`,
                userData,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );

            navigate('/login');
        } catch (error) {
            const response = error.response?.data;
            const message = response?.message || t2;

            const rawErrors = response?.errors || {};
            const errorList = Array.isArray(rawErrors)
                ? rawErrors
                : Object.values(rawErrors).flat();

            setError(errorList.join('') || message);
        } finally {
            setLoading(false);
        }
    };    

    const handleRequestOTP = async () => {
        if (!emailSentTo) {
            setError(t1);
            return;
        }

        try {
            setLoading(true);
            const response = await axios.post(
                `${serverApi}${authenticationControllerApi}/request-otp`,
                emailSentTo,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );
            
            setError(response.data.message);
        } catch (error) {
            const response = error.response?.data;
            const message = response?.message || t3;

            const rawErrors = response?.errors || {};
            const errorList = Array.isArray(rawErrors)
                ? rawErrors
                : Object.values(rawErrors).flat();

            setError(errorList.join('') || message);
        } finally {
            setLoading(false);
        }
    };    
    
    return ({
        setOtp, handleSubmit, handleRequestOTP
    });
}

export default useOTP;