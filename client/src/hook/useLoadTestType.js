import { useEffect, useState } from "react";
import axios from "axios";

function useLoadTestType({ setError, setLoading }) {
    const [testTypes, setTestTypes] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const labTestPlanControllerApi = process.env.REACT_APP_LAB_TEST_CONTROLLER_API;

    const getTestTypes = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${labTestPlanControllerApi}/testType/all`
        )
            .then((response) => {
                setTestTypes(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        getTestTypes();
    }, [])

    return ({
        testTypes
    })
}

export default useLoadTestType;