import { useState } from "react";
import axios from "axios";

function useCancelAppointment({ setError, setLoading }) {
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const cancelAppointment = async ({ appointmentId }) => {
        setLoading(true);
        await axios.put(
            `${serverApi}${appointmentControllerApi}/cancel/${appointmentId}`
        )
            .then((response) => {
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        cancelAppointment
    })
}

export default useCancelAppointment;