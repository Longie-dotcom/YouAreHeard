import { useEffect, useState } from "react";
import axios from "axios";

function useLoadConditions({ setError, setLoading }) {
    const [conditions, setConditions] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const loadConditions = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${patientProfilePlanControllerApi}/conditions`
        )
            .then((response) => {
                setConditions(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        loadConditions();
    }, [])

    return ({
        conditions
    })
}

export default useLoadConditions;