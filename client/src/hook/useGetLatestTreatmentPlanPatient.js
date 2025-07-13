import { useEffect, useState } from "react";
import axios from "axios";

function useGetLatestTreatmentPlanPatient({ setError, setLoading, patientId }) {
    const [treatmentPlan, setTreatmentPlan] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const getLatestTreatmentPlanByPatientId = async () => {
        if (!patientId) {
            return;
        }

        setLoading(true);
        await axios.get(
            `${serverApi}${treatmentPlanControllerApi}/patient/${patientId}`
        )
            .then((response) => {
                setTreatmentPlan(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getLatestTreatmentPlanByPatientId();
    }, [])

    return ({
        treatmentPlan
    })
}

export default useGetLatestTreatmentPlanPatient;