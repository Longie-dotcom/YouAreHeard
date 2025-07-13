import { useEffect, useState } from "react";
import axios from "axios";

function useLoadAllBlogs({ setError, setLoading }) {
    const [blogs, setBlogs] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogControllerApi = process.env.REACT_APP_BLOG_CONTROLLER_API;

    const loadAllBlogs = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${blogControllerApi}/all`
        )
            .then((response) => {
                setBlogs(response.data);
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    useEffect(() => {
        loadAllBlogs();
    }, [])

    return ({
        blogs
    })
}

export default useLoadAllBlogs;