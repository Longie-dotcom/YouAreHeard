import { useEffect, useState } from "react";
import axios from "axios";

function useGetPatientProfile({ setError, setLoading, userId }) {
    const [profile, setPatientProfile] = useState(null);
    const [finish, setFinish] = useState(false);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const patientProfilePlanControllerApi = process.env.REACT_APP_PATIENT_PROFILE_CONTROLLER_API;

    const getPatientProfile = async () => {
        if (!userId){
            return;
        }

        setLoading(true);
        await axios.get(
            `${serverApi}${patientProfilePlanControllerApi}/profile/${userId}`
        )
            .then((response) => {
                setPatientProfile(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
                setFinish(true);
            })
    };

    useEffect(() => {
        getPatientProfile();
    }, [userId])

    return ({
        finish,
        profile
    })
}

export default useGetPatientProfile;