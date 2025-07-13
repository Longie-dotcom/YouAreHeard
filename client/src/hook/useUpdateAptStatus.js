import axios from "axios";

function useUpdateAptStatus({ setError, setLoading, setFinish, setReload }) {
    const t1 = 'Chưa có trạng thái để cập nhật';
    const t2 = 'Chưa chọn lịch cần cập nhật';
    const t3 = 'Cập nhật trạng thái của lịch thành công.';
    const t4 = ' ID của lịch là: ';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const updateAppointmentStatus = async ({ appointmentID, appointmentStatusID, scheduleID }) => {
        if (!appointmentID) {
            setError(t2);
            return;
        }

        if (!appointmentStatusID) {
            setError(t1);
            return;
        }

        if (!appointmentStatusID) {
            setError(t1);
            return;
        }

        const payload = {
            appointmentID,
            appointmentStatusID,
            scheduleID
        }
        console.log(payload);
        setLoading(true);
        await axios.post(
            `${serverApi}${appointmentControllerApi}/updateStatus`,
            payload,
            {
                headers: {
                    'Content-Type': 'application/json',
                }
            }
        )
            .then((response) => {
                setFinish(`${t3}${t4}${appointmentID}`);
                setReload(prev => prev + 1);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        updateAppointmentStatus
    })
}

export default useUpdateAptStatus;