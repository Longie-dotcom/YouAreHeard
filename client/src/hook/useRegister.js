import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { isValidPhoneNumber } from 'libphonenumber-js';

import axios from 'axios';

function useRegister() {
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmedPassword, setConfirmedPassword] = useState('');
    const [name, setName] = useState('');
    const [age, setAge] = useState(1);
    const [phone, setPhone] = useState('');
    const [sex, setSex] = useState('male');

    const navigate = useNavigate();

    const serverApi = process.env.REACT_APP_SERVER_API;
    const userControllerApi = process.env.REACT_APP_USER_CONTROLLER_API;

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (password !== confirmedPassword) {
            setError("Confirmed password does not matched");
            return;
        }

        if (!isValidPhoneNumber(phone)) {
            console.log(phone);
            setError("Invalid phone number");
            return;
        }
        const userData = {
            email,
            password,
            name,
            age: Number(age),
            gender: sex,
            phone,
        };

        try {
            const response = await axios.post(
                `${serverApi}${userControllerApi}/register`,
                {
                    email: email,
                    password: password,
                    name: name,
                    age: age,
                    gender: sex,
                    phone: phone
                },
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );

            console.log(response.data);
            navigate('/login');
        } catch (error) {
            const errorMessage = error.response?.data?.message || "An unexpected error occurred";
            console.error(errorMessage);
            setError(errorMessage);
        }
    };

    return ({
        error,
        email, setEmail,
        password, setPassword,
        confirmedPassword, setConfirmedPassword,
        name, setName,
        age, setAge,
        sex, setSex,
        phone, setPhone,
        handleSubmit
    });
}

export default useRegister;