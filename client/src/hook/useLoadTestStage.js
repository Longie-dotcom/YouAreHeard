import { useEffect, useState } from "react";
import axios from "axios";

function useLoadTestStage({ setError, setLoading }) {
    const [testStages, setTestStages] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const labTestPlanControllerApi = process.env.REACT_APP_LAB_TEST_CONTROLLER_API;

    const getTestStages = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${labTestPlanControllerApi}/testStage/all`
        )
            .then((response) => {
                setTestStages(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getTestStages();
    }, [])

    return ({
        testStages
    })
}

export default useLoadTestStage;