import axios from "axios";

function usePostBlog({ setError, setLoading, setReload, setFinish }) {
    const t1 = 'Bài viết phải có tiêu đề, hãy điền tiêu đề';
    const t2 = 'Bài viết phải có nội dung, hãy điền nội dung';
    const t3 = 'Bài viết phải có 1 ảnh để minh họa, hãy tải một ảnh lên';
    const t4 = 'Không thể tải ảnh lên, hãy thử lại sau';
    const t5 = 'Đăng bài viết thành công';
    const t6 = 'Không tìm thấy tài khoản nhân viên, vui lòng đăng nhập lại';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogControllerApi = process.env.REACT_APP_BLOG_CONTROLLER_API;

    const postBlog = async ({ title, details, imageFile, userId }) => {
        if (!title) {
            setError(t1);
            return;
        }

        if (!details) {
            setError(t2);
            return;
        }

        if (!imageFile) {
            setError(t3);
            return;
        }
        
        if (!userId) {
            setError(t6);
            return;
        }

        setLoading(true);

        try {
            const formData = new FormData();
            formData.append("file", imageFile);

            const uploadResponse = await axios.post(
                `${serverApi}${blogControllerApi}/upload-image`,
                formData,
                {
                    headers: {
                        "Content-Type": "multipart/form-data",
                    },
                }
            );

            const imageName = uploadResponse.data.fileName || uploadResponse.data.url;
            
            if (!imageName) {
                setError(t4);
                return;
            };

            const blogDto = {
                title,
                details,
                image: imageName,
                userId,
                date: new Date().toISOString()
            };

            await axios.post(`${serverApi}${blogControllerApi}/postblog`, blogDto);
            
            setReload((prev) => prev + 1);
            setFinish(t5);
        } catch (error) {
            console.error(error);
            setError(error.response?.data?.message || "Blog post failed.");
        } finally {
            setLoading(false);
        }
    };

    return {
        postBlog,
    };
}

export default usePostBlog;
