import axios from "axios";

function useCreateTreatmentPlan({ setError, setLoading, setIsSubmit, setFinish }) {
    const t1 = "Bác sĩ chưa nhập ít nhất một thời gian uống thuốc.";
    const t2 = "Tạo phác đồ điều trị thành công";

    const serverApi = process.env.REACT_APP_SERVER_API;
    const treatmentPlanControllerApi = process.env.REACT_APP_TREATMENT_PLAN_CONTROLLER_API;

    const createTreatmentPlan = async ({ pillRemindTime, treatmentDetail }) => {
        if (pillRemindTime.length === 0) {
            setError(t1);
            return;
        }
        setLoading(true);
        const requestTreatmentPlan = {
            treatmentPlan: treatmentDetail,
            pillRemindTimes: pillRemindTime
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
                setFinish(t2);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
                setIsSubmit(false);
            })
    };

    return ({ createTreatmentPlan })
}

export default useCreateTreatmentPlan;