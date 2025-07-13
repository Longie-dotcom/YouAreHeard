import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllDoctor({ setError, setLoading, roleId }) {
    const [doctors, setDoctors] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorControllerApi = process.env.REACT_APP_DOCTOR_CONTROLLER_API;

    const getAllDoctors = async () => {
        if (!roleId) {
            return;
        }


        setLoading(true);
        await axios.get(
            `${serverApi}${doctorControllerApi}/all/${roleId}`
        )
            .then((response) => {
                setDoctors(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllDoctors();
    }, [roleId])

    return ({
        doctors
    })
}

export default useLoadAllDoctor;