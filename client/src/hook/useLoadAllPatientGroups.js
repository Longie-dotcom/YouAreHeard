import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllPatientGroups({ setError, setLoading }) {
    const [patientGroups, setPatientGroups] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const getAllPatientGroups = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${treatmentPlanControllerApi}/patientGroup/all`
        )
            .then((response) => {
                setPatientGroups(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllPatientGroups();
    }, [])

    return ({
        patientGroups
    })
}

export default useLoadAllPatientGroups;