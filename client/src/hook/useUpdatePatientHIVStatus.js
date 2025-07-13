import axios from "axios";

function useUpdatePatientHIVStatus({ setError, setLoading, setFinish }) {
    const t1 = 'Cập nhật trạng thái HIV thành công';
    const t2 = 'Không tồn tại bệnh nhân';
    const t3 = 'Không tồn tại trạng thái HIV';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const updatePatientHIVStatus = async ({ patientID, hivStatusID }) => {
        if (!patientID) {
            setError(t2);
            return;
        }

        if (!hivStatusID) {
            setError(t3);
            return;
        }

        setLoading(true);

        const payload = { patientID, hivStatusID }

        await axios.post(
            `${serverApi}${patientProfilePlanControllerApi}/updateHIVStatus`,
            payload,
            {
                headers: {
                    'Content-Type': 'application/json',
                }
            }
        )
            .then((response) => {
                setFinish(t1);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        updatePatientHIVStatus
    })
}

export default useUpdatePatientHIVStatus;