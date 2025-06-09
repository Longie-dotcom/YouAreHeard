import { useEffect, useState } from "react";
import axios from "axios";

function useLoadDoctorSchedule({ doctorId }) {
    const [schedules, setSchedules] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorControllerApi = process.env.REACT_APP_DOCTOR_CONTROLLER_API;

    const getDoctorSchedule = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${doctorControllerApi}/schedule/${doctorId}`
        )
            .then((response) => {
                setSchedules(response.data);
            }).catch((error) => {
                console.log(error);
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    }

    useEffect(() => {
        getDoctorSchedule();
    }, [doctorId])

    return ({
        loading, 
        error, setError,
        schedules, getDoctorSchedule
    })
}

export default useLoadDoctorSchedule;