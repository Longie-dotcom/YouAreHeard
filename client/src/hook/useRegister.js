import { useState } from "react";
import axios from 'axios';

function useRegister({ setOpenOTP }) {
    const t1 = 'Mật khẩu xác nhận không trùng khớp!';
    const t2 = 'Lỗi máy chủ không xác định được!';
    const t3 = 'Hãy điền đầy đủ thông tin';

    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmedPassword, setConfirmedPassword] = useState('');
    const [name, setName] = useState('');
    const [phone, setPhone] = useState('');
    const [dob, setDob] = useState('');
    const [roleId, setRoleId] = useState('4')
    const [loading, setLoading] = useState(false);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const authenticationControllerApi = process.env.REACT_APP_AUTHENTICATION_CONTROLLER_API;

    const handleSubmit = async () => {
        if (password !== confirmedPassword) {
            setError(t1);
            return;
        }

        if (!(email && password && name && phone && dob)) {
            setError(t3);
            return;
        }

        const userData = {
            email,
            password,
            name,
            dob,
            phone,
            roleId,
        };

        try {
            setLoading(true);

            const response = await axios.post(
                `${serverApi}${authenticationControllerApi}/register`,
                userData,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );

            setOpenOTP(true);
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

    return ({
        loading,
        error, setError,
        email, setEmail,
        password, setPassword,
        confirmedPassword, setConfirmedPassword,
        name, setName,
        dob, setDob,
        phone, setPhone,
        handleSubmit
    });
}

export default useRegister;