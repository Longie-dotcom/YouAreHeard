import { useEffect, useState } from "react";
import axios from "axios";

function useLoadDoctorSchedule({ doctorId, setError, setLoading }) {
    const [schedules, setSchedules] = useState(null);
    
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
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    }

    useEffect(() => {
        getDoctorSchedule();
    }, [doctorId])

    return ({
        schedules, getDoctorSchedule
    })
}

export default useLoadDoctorSchedule;