import axios from "axios";

function useRequestReAppointment({ setError, setLoading, setFinish }) {
    const t1 = 'Tạo lịch tái khám thành công';
    const t2 = 'Server xảy ra lỗi';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const handleRequest = async ({ choosenAppointment }) => {
        const appointmentDetail = {
            doctorScheduleID: choosenAppointment.doctorScheduleID,
            notes: choosenAppointment.notes ?? "",
            doctorNotes: choosenAppointment.doctorNotes ?? "",
            patientID: choosenAppointment.patientID,
            doctorID: choosenAppointment.doctorID
        }

        try {
            setLoading(true);

            const response = await axios.post(
                `${serverApi}${appointmentControllerApi}/reAppointment`,
                appointmentDetail,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );

            setFinish(t1);
        } catch (error) {
            const response = error.response?.data;
            const message = response?.message || t2;

            const rawErrors = response?.errors || {};
            const errorList = Array.isArray(rawErrors)
                ? rawErrors
                : Object.values(rawErrors).flat();

            setError(errorList.join('') || message);
        } finally {
            setLoading(false);
        }
    }

    return ({
        handleRequest
    })
}

export default useRequestReAppointment;