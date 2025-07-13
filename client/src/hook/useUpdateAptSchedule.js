import axios from "axios";

function useUpdateAptSchedule({ setError, setLoading, setFinish, setReload }) {
    const t1 = 'Chưa chọn lịch cần cập nhật';
    const t2 = 'Chưa chọn lịch mới';
    const t3 = 'Chưa chọn bác sĩ thay thế';
    const t4 = 'Lịch hẹn đã được thay đổi thành công';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const updateAppointmentSchedule = async ({ appointmentID, doctorScheduleID, doctorID }) => {
        console.log(appointmentID);
        
        if (!appointmentID) {
            setError(t1);
            return;
        }

        if (!doctorScheduleID) {
            setError(t2);
            return;
        }

        if (!doctorID) {
            setError(t3);
            return;
        }

        const payload = {
            appointmentID,
            doctorScheduleID,
            doctorID
        }

        setLoading(true);
        await axios.post(
            `${serverApi}${appointmentControllerApi}/updateSchedule`,
            payload,
            {
                headers: {
                    'Content-Type': 'application/json',
                }
            }
        )
            .then((response) => {
                setFinish(t4);
                setReload(prev => prev + 1);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        updateAppointmentSchedule
    })
}

export default useUpdateAptSchedule;