import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllMedications({ setError, setLoading }) {
    const [medications, setMedications] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const getAllMedications = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${treatmentPlanControllerApi}/medication/all`
        )
            .then((response) => {
                setMedications(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllMedications();
    }, [])

    return ({
        medications
    })
}

export default useLoadAllMedications;