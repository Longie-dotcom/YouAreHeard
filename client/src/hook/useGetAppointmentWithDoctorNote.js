import axios from "axios";
import { useEffect, useState } from "react";

function useGetAppointmentWithDoctorNote({ setError, setLoading, doctorId }) {
    const [appointmentNote, setAppointment] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const getAppointmentWithDoctorNote = async () => {
        if(!doctorId){
            return;
        }

        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/appointmentsWithDoctorNote/${doctorId}`
        )
            .then((response) => {
                setAppointment(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAppointmentWithDoctorNote();
    }, [doctorId])

    return ({
        appointmentNote
    })
}

export default useGetAppointmentWithDoctorNote;