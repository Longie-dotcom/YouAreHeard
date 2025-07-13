import { useEffect, useState } from "react";
import axios from "axios";

function useGetPatientTestResult({ setError, setLoading, userId }) {
    const [testResults, setTestResults] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const labTestPlanControllerApi = process.env.REACT_APP_LAB_TEST_CONTROLLER_API;

    const getPatientTestResult = async () => {
        if (!userId) {
            return;
        }

        setLoading(true);
        await axios.get(
            `${serverApi}${labTestPlanControllerApi}/labResult/patient/${userId}`
        )
            .then((response) => {
                setTestResults(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getPatientTestResult();
    }, [userId])

    return ({
        testResults
    })
}

export default useGetPatientTestResult;