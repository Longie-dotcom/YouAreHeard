import { useEffect, useState } from "react";
import axios from "axios";

function useLoadPregnancyStatus({ setError, setLoading }) {
    const [pregnancyStatus, setPregnancyStatus] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const loadPregnancyStatus = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${patientProfilePlanControllerApi}/pregnancystatuses`
        )
            .then((response) => {
                setPregnancyStatus(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        loadPregnancyStatus();
    }, [])

    return ({
        pregnancyStatus
    })
}

export default useLoadPregnancyStatus;