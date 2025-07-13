import { useEffect, useState } from "react";
import axios from "axios";

function useLoadStaffBlog({ setError, setLoading, reload, userId }) {
    const [blogs, setBlogs] = useState(null);
    
    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogControllerApi = process.env.REACT_APP_BLOG_CONTROLLER_API;

    const loadStaffBlog = async () => {
        setLoading(true);
        await axios.get(
            `${serverApi}${blogControllerApi}/userblogs/${userId}`
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
        loadStaffBlog();
    }, [reload, userId])

    return ({
        blogs
    })
}

export default useLoadStaffBlog;