import { useEffect, useState } from "react";
import axios from "axios";

function useCreateTreatmentPlan({ setError, setLoading }) {
    const t1 = "Bác sĩ chưa nhập ít nhất một thời gian uống thuốc.";
    const t2 = "Bác sĩ chưa chọn nhóm bệnh nhân";

    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const createTreatmentPlan = async ({ pillRemindTime, treatmentDetail }) => {
        const transformedPillRemindTimes = pillRemindTime.flatMap(med =>
            Array.isArray(med.remindTimes) && med.dosage > 0
                ? med.remindTimes
                    .filter(time => time && time.trim() !== "")
                    .map(time => ({
                        time: time,
                        medicationID: med.medicationID,
                        drinkDosage: med.dosage
                    }))
                : []
        );
        if (transformedPillRemindTimes.length === 0) {
            setError(t1);
            return;
        }

        if (!treatmentDetail.patientGroupID) {
            setError(t2);
            return;
        }

        setLoading(true);
        const requestTreatmentPlan = {
            treatmentPlan: treatmentDetail,
            pillRemindTimes: transformedPillRemindTimes
        }

        console.log(requestTreatmentPlan);
        await axios.post(
            `${serverApi}${treatmentPlanControllerApi}/treatmentPlan/create`,
            requestTreatmentPlan,
            {
                headers: {
                    'Content-Type': 'application/json',
                }
            }
        )
            .then((response) => {
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({ createTreatmentPlan })
}

export default useCreateTreatmentPlan;