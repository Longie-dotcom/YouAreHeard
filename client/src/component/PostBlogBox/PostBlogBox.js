// Modules
import { useState } from 'react';

// Styling sheet
import './PostBlogBox.css';

// Components
import UploadIcon from '../../uploads/icon/upload.png';
import PostBlogForm from '../PostBlogForm/PostBlogForm';
import TextBox from '../TextBox/TextBox';
import Icon from '../Icon/Icon';

// Hooks
import useLoadStaffBlog from '../../hook/useLoadStaffBlog';
import useDeletePost from '../../hook/useDeletePost';

function PostBlogBox({ user, setLoading, setError, setFinish }) {
    const t1 = 'Tạo bài viết mới';
    const t2 = 'Người đăng:';
    const t3 = 'Ngày đăng:';
    const t4 = 'Không có bài viết nào';
    const t5 = 'Xóa bài viết';

    const [reload, setReload] = useState(0);
    const [openPostForm, setOpenPostForm] = useState(false);
    const [previewImage, setPreviewImage] = useState(null);
    const [previewTitle, setPreviewTitle] = useState('');

    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogImageApi = process.env.REACT_APP_BLOG_ASSET_API;

    const {
        blogs
    } = useLoadStaffBlog({ setError, setLoading, reload, userId: user?.UserId });

    const {
        deleteBlog
    } = useDeletePost({ setError, setLoading, setReload, setFinish });

    return (
        <div className='post-blog-box'>
            {previewImage && (
                <TextBox
                    text={<img src={previewImage} alt='Preview' className='preview-image' />}
                    setText={setPreviewImage}
                    title={previewTitle}
                />
            )}

            {openPostForm && (
                <PostBlogForm
                    setOpenPostForm={setOpenPostForm}
                    setError={setError}
                    setFinish={setFinish}
                    setLoading={setLoading}
                    setReload={setReload}
                    user={user}
                />
            )}

            <button
                className='create-blog'
                onClick={() => setOpenPostForm(true)}
            >
                <Icon src={UploadIcon} alt={'upload-icon'} />
                {t1}
            </button>

            <div className='post-list'>
                {blogs?.length > 0 ? (
                    blogs.map((blog, index) => (
                        <div className='blog-card' key={index}>
                            <img
                                src={`${serverApi}${blogImageApi}/${blog.image}`}
                                alt={blog.title}
                                className='blog-thumbnail'
                                onClick={() => {
                                    setPreviewImage(`${serverApi}${blogImageApi}/${blog.image}`);
                                    setPreviewTitle(blog.title);
                                }}
                            />
                            <div className='blog-content'>
                                <div className='blog-title'>{blog.title}</div>
                                <div className='blog-details'>{blog.details}</div>
                                <div className='blog-meta'>
                                    <div>
                                        {t2}&nbsp;{blog.userName}
                                    </div>
                                    <div>
                                        {t3}&nbsp;{blog.date.split('T')[0]}
                                    </div>
                                </div>
                            </div>
                            <div className='buttons'>
                                <button
                                    className='delete-blog'
                                    onClick={() => deleteBlog({ blogId: blog.blogId })}
                                >
                                    {t5}
                                </button>
                            </div>
                        </div>
                    ))
                ) : (
                    <div className='no-blogs'>{t4}</div>
                )}
            </div>
        </div>
    );
}

export default PostBlogBox;