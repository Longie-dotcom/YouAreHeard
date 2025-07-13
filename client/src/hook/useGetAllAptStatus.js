import axios from "axios";
import { useEffect, useState } from "react";

function useGetAllAptStatus({ setError, setLoading }) {
    const [statuses, setStatuses] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const getAllAppointmentStatuses = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/appointmentStatus/all`
        )
            .then((response) => {
                setStatuses(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllAppointmentStatuses();
    }, [])

    return ({
        statuses
    })
}

export default useGetAllAptStatus;