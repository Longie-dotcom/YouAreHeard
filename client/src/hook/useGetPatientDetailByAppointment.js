import { useState } from "react";
import axios from "axios";

function useGetPatientDetailByAppointment({ setError, setLoading, setAppointmentDetail }) {
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const getPatientProfileByAppointmentId = async ({ appointmentId }) => {
        if (!appointmentId) {
            return;
        }
        
        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/getDetail/${appointmentId}`
        )
            .then((response) => {
                setAppointmentDetail(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        getPatientProfileByAppointmentId
    })
}

export default useGetPatientDetailByAppointment;