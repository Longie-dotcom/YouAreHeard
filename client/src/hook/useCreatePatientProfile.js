import axios from "axios";

function useCreatePatientProfile({ setError, setLoading, acceptPolicy, setFinish }) {
    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const t1 = 'Bạn chưa đăng nhập, hãy đăng nhập lại';
    const t2 = 'Bạn chưa cung cấp thông tin về chiều cao';
    const t3 = 'Bạn chưa cung cấp thông tin về cân nặng';
    const t4 = 'Bạn chưa cung cấp thông tin về giới tính';
    const t5 = 'Bạn chưa cung cấp thông tin về tình trạng thai sản';
    const t7 = 'Người dùng không chấp thuận điều khoảng sử dụng, không thể tiếp tục';
    const t8 = 'Cập nhật hồ sơ hoàn tất, bạn có thể sử dụng các dịch vụ khác';

    const handleSubmit = async ({
        userID,
        pregnancyStatusID,
        height,
        weight,
        gender,
        conditions
    }) => {
        const requestBody = {
            userID,
            pregnancyStatusID,
            height,
            weight,
            gender,
            conditions
        };

        if (!acceptPolicy) {
            setError(t7);
            return;
        }

        if (!userID) {
            setError(t1);
            return
        }

        if (!height) {
            setError(t2);
            return
        }

        if (!weight) {
            setError(t3);
            return
        }

        if (!gender) {
            setError(t4);
            return
        }

        if (!pregnancyStatusID) {
            setError(t5);
            return
        }

        setLoading(true);
        await axios.post(
            `${serverApi}${patientProfilePlanControllerApi}/insert`,
            requestBody
        )
            .then((response) => {
                setFinish(t8);
            })
            .catch((error) => {
                setError(error.response?.data?.message);
            })
            .finally(() => {
                setLoading(false);
            });
    };

    return {
        handleSubmit
    };
}

export default useCreatePatientProfile;
