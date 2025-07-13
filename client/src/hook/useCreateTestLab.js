import axios from "axios";

function useCreateTestLab({ setError, setLoading, setFinish }) {
    const t1 = 'Chưa có thông số nào được khởi tạo';
    const t2 = 'Mẫu thông tin chưa được khởi tạo';
    const t3 = 'Chưa có thông số nào được điền';
    const t4 = 'Tạo mẫu thông tin xét nghiệm thành công';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const labTestPlanControllerApi = process.env.REACT_APP_LAB_TEST_CONTROLLER_API;

    const createTestLab = async ({ labResult }) => {
        if(!labResult?.testMetricValues) {
            setError(t2);
            return
        }
        
        if(labResult?.testMetricValues.length === 0) {
            setError(t1);
            return
        }

        if(labResult.testMetricValues.every(m => m.value === null || m.value === '')) {
            setError(t3);
            return
        }

        setLoading(true);
        await axios.post(
            `${serverApi}${labTestPlanControllerApi}/create`,
                labResult,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            )
            .then((response) => {
                setFinish(t4);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        createTestLab
    })
}

export default useCreateTestLab;