import { useEffect, useState } from "react";
import axios from "axios";

function useGetDoctorById({ setError, setLoading }) {
    const [doctor, setDoctor] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorControllerApi = process.env.REACT_APP_DOCTOR_CONTROLLER_API;

    const getDoctorById = async ({ doctorId }) => {
        setLoading(true);
        await axios.get(
            `${serverApi}${doctorControllerApi}/profile/${doctorId}`
        )
            .then((response) => {
                setDoctor(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        doctor, setDoctor,
        getDoctorById
    })
}

export default useGetDoctorById;