import { useState } from "react";
import axios from "axios";

function useRequestAppointment({ user, choosenAppointment, type, setError, setLoading }) {
    const t1 = 'Vui lòng chọn loại cuộc hẹn';
    const t2 = 'Server xảy ra lỗi';

    const [reason, setReason] = useState(null);
    const [note, setNote] = useState(null);
    const [anonymous, setAnonymous] = useState(false);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;

    const handleRequest = async () => {
        if (!type) {
            setError(t1);
            return;
        }

        const appointmentDetail = {
            doctorScheduleID: choosenAppointment.schedule.doctorScheduleID,
            isOnline: type === process.env.REACT_APP_ROLE_DOCTOR_ID ? false : true,
            notes: note,
            reason: reason,
            isAnonymous: anonymous,
            patientID: user?.UserId,
            doctorID: choosenAppointment.doctor.userID
        }
        
        if (type === process.env.REACT_APP_ROLE_DOCTOR_ID) {
            appointmentDetail.isAnonymous = false;
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

            window.location.href = response.data;
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