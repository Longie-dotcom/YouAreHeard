import axios from "axios";
import { useEffect, useState } from "react";

function useGetAllAppointments({ setError, setLoading, reload }) {
    const [appointments, setAppointments] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const getAllAppointments = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/all`
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
        getAllAppointments();
    }, [reload])

    return ({
        appointments
    })
}

export default useGetAllAppointments;