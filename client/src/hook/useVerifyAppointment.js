import axios from "axios";
import { useState } from "react";

function useVerifyAppointment({ setError, setLoading }) {
    const t1 = 'Bạn chưa nhập mã định danh';
    const t2 = 'Không tìm thấy thông tin của bệnh nhân';

    const [result, setResult] = useState(null);
    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const verify = async ({ orderCode }) => {
        if (!orderCode) {
            setError(t1);
            return;
        }

        setLoading(true);
        await axios.get(
            `${serverApi}${appointmentControllerApi}/verify-identity/${orderCode}`
        )
            .then((response) => {
                setResult(response.data);
            }).catch((error) => {
                setError(t2);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        verify, result
    })
}

export default useVerifyAppointment;