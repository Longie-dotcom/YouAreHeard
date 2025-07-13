import axios from "axios";

function useDeletePost({ setError, setLoading, setReload, setFinish }) {
    const t1 = 'Xóa bài viết thành công';
    const t2 = 'Bài viết không tồn tại';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogControllerApi = process.env.REACT_APP_BLOG_CONTROLLER_API;

    const deleteBlog = async ({ blogId }) => {
        if (!blogId) {
            setError(t2);
            return;
        }

        setLoading(true);
        await axios.delete(
            `${serverApi}${blogControllerApi}/deleteblog/${blogId}`
        )
            .then((response) => {
                setReload(prev => prev + 1);
                setFinish(t1)
            }).catch((error) => {
                setError(error.response?.data?.message);
            }).finally(() => {
                setLoading(false);
            })
    };

    return ({
        deleteBlog
    })
}

export default useDeletePost;