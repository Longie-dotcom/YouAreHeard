import { use, useState } from "react";
import axios from "axios";

function useRequestAppointment({ user, choosenAppointment, setOpenFinish, type, setError, setLoading }) {
    const t1 = 'Vui lòng chọn loại cuộc hẹn';
    const t2 = 'Server xảy ra lỗi';

    const [reason, setReason] = useState(null);
    const [note, setNote] = useState(null);
    const [anonymous, setAnonymous] = useState(true);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const handleRequest = async () => {
        if (!type) {
            setError(t1);
            return;
        }

        const appointmentDetail = {
            appointment: {
                doctorScheduleID: choosenAppointment.schedule.doctorScheduleID,
                appointmentStatusID: null,
                zoomLink: type === 'online' ? 'yes' : null,
                notes: note,
                reason: reason,
                isAnonymous: anonymous
            },
            medicalHistory: {
                dateTime: new Date().toISOString(),
                patientID: user.UserId,
                doctorID: choosenAppointment.doctor.userID,
            }
        }

        try {
            setLoading(true);

            const response = await axios.post(
                `${serverApi}${appointmentControllerApi}/request`,
                appointmentDetail,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
            );
            setOpenFinish(true);
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
        reason, setReason,
        note, setNote,
        anonymous, setAnonymous,
        handleRequest
    })
}

export default useRequestAppointment;