import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllDoctor() {
    const [doctors, setDoctors] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorControllerApi = process.env.REACT_APP_DOCTOR_CONTROLLER_API;

    const getAllDoctors = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${doctorControllerApi}/all`
        )
            .then((response) => {
                setDoctors(response.data);
            }).catch((error) => {
                console.log(error);
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllDoctors();
    }, [])

    return ({
        doctors, loading, 
        error, setError
    })
}

export default useLoadAllDoctor;