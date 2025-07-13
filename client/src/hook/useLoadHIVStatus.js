import { useEffect, useState } from "react";
import axios from "axios";

function useLoadHIVStatus({ setError, setLoading }) {
    const [hivStatus, setHivStatus] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const loadHIVStatus = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${patientProfilePlanControllerApi}/hivstatuses`
        )
            .then((response) => {
                setHivStatus(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        loadHIVStatus();
    }, [])

    return ({
        hivStatus
    })
}

export default useLoadHIVStatus;