import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from 'axios';

function useLogin({ setReloadCookies }) {
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const serverApi = process.env.REACT_APP_SERVER_API;
    const authenticationControllerApi = process.env.REACT_APP_AUTHENTICATION_CONTROLLER_API;

    const handleSubmit = async () => {
        const loginToken = {
            email: email,
            password: password,
        }

        setLoading(true);
        await axios.post(
            `${serverApi}${authenticationControllerApi}/login`,
            loginToken,
            {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true,
            }
        )
            .then((response) => {
                setReloadCookies(prev => prev + 1);
                console.log(response.data.message);
                navigate('/home');
            }).catch((error) => {
                console.log(error.response.data.message);
                setError(error.response.data.message);
            }). finally(() => {
                setLoading(false);
            })
    };

    return ({
        loading,
        error, setError,
        email, setEmail,
        password, setPassword,
        handleSubmit
    });
}

export default useLogin;