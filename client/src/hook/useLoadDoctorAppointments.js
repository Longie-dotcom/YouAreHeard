import { useEffect, useState } from "react";
import axios from "axios";

function useLoadDoctorAppointments({ setError, setLoading, doctorId }) {
    const [appointments, setAppointments] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const getAllAppointmentByDoctorId = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/doctor/${doctorId}`
        )
            .then((response) => {
                setAppointments(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllAppointmentByDoctorId();
    }, [doctorId])

    return ({
        appointments
    })
}

export default useLoadDoctorAppointments;