import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from 'axios';

function useLogin({ setReloadCookies }) {
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const serverApi = process.env.REACT_APP_SERVER_API;
    const userControllerApi = process.env.REACT_APP_USER_CONTROLLER_API;

    const handleSubmit = async (e) => {
        e.preventDefault();
        const loginToken = {
            email: email,
            password: password,
        }

        await axios.post(
            `${serverApi}${userControllerApi}/login`,
            loginToken,
            {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true,
            }
        )
            .then((response) => {
                setReloadCookies(prev => prev + 1);
                console.log(response.data.message);
                navigate('/userPage');
            }).catch((error) => {
                console.log(error.response.data.message);
                setError(error.response.data.message);
            });
    };

    return ({
        error, setError,
        email, setEmail,
        password, setPassword,
        handleSubmit
    });
}

export default useLogin;