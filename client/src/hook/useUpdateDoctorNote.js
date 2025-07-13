import axios from "axios";

function useUpdateDoctorNote({ appointmentID, notes, setError, setLoading, setFinish }) {
    const t1 = 'Lưu ghi chú thành công';
    const t2 = 'Server xảy ra lỗi';
    const t3 = 'Bác sĩ chưa điền ghi chú';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const handleRequest = async () => {
        if (!notes) {
            setError(t3);
            return;
        }

        try {
            setLoading(true);
            const doctorNotes = {
                notes: notes,
                appointmentID: appointmentID
            }

            const response = await axios.post(
                `${serverApi}${appointmentControllerApi}/doctorNote`,
                doctorNotes,
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

export default useUpdateDoctorNote;