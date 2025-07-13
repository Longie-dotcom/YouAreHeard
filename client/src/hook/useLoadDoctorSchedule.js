import { useEffect, useState } from "react";
import axios from "axios";

function useLoadDoctorSchedule({ setError, setLoading, doctorId, roleId }) {
    const [schedules, setSchedules] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorControllerApi = process.env.REACT_APP_DOCTOR_CONTROLLER_API;

    const getDoctorSchedule = async () => {
        setLoading(true);

        if (doctorId) {
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
        } else {
            if (!roleId) {
                return;
            }

            await axios.get(
                `${serverApi}${doctorControllerApi}/schedule/all/${roleId}`
            )
                .then((response) => {
                    setSchedules(response.data);
                }).catch((error) => {
                    setError(error.response?.data?.message);
                }).finally(() => {
                    setLoading(false);
                })
        }
    }

    useEffect(() => {
        getDoctorSchedule();
    }, [doctorId, roleId]);

    return ({
        schedules
    })
}

export default useLoadDoctorSchedule;