import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllTestMetric({ setError, setLoading }) {
    const [testMetric, setTestMetrics] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const labTestPlanControllerApi = process.env.REACT_APP_LAB_TEST_CONTROLLER_API;

    const getTestMetrics = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${labTestPlanControllerApi}/metric/all`
        )
            .then((response) => {
                setTestMetrics(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getTestMetrics();
    }, [])

    return ({
        testMetric
    })
}

export default useLoadAllTestMetric;