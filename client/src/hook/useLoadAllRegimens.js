import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllRegimens({ setError, setLoading }) {
    const [regimens, setRegimens] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const getAllRegimens = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${treatmentPlanControllerApi}/ARVRegimen/all`
        )
            .then((response) => {
                setRegimens(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getAllRegimens();
    }, [])

    return ({
        regimens
    })
}

export default useLoadAllRegimens;