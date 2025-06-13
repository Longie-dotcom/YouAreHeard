import axios from "axios";
import { useNavigate } from "react-router-dom";

function useLogout({ setReloadCookies }) {
    const navigate = useNavigate();

    const serverApi = process.env.REACT_APP_SERVER_API;
    const authenticationControllerApi = process.env.REACT_APP_AUTHENTICATION_CONTROLLER_API;

    const logout = () => {
        axios.post(`${serverApi}${authenticationControllerApi}/logout`,
            {},
            {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true,
            }).then((response) => {
                setReloadCookies(prev => prev + 1);
                console.log(response.data.message);
                navigate('/login');
            }).catch((error) => {
                console.log(error.response?.data?.message || error.message);
            })
    }

    return ({
        logout
    })
}

export default useLogout;