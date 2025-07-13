// Modules
import { useState } from 'react';

// Styling sheet
import './BlogPage.css';

// Assets
import LogoBlog from '../../../uploads/logo-blog.png';

// Components
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';

// Hooks
import useLoadAllBlogs from '../../../hook/useLoadAllBlogs';

function BlogPage({ user }) {
    const t1 = 'Tất cả bài viết mới nhất';
    const t2 = 'Các bài viết này được đăng lên bởi đội ngũ nhân viên của cơ sở';

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const blogImageApi = process.env.REACT_APP_BLOG_ASSET_API;

    const { blogs } = useLoadAllBlogs({ setError, setLoading });

    return (
        <div className='blog-page'>
            {error && <ErrorBox error={error} setError={setError} />}
            {loading && <SkeletonUI />}

            <div className='section-1'>
                <div className='title'>
                    <img
                        src={LogoBlog}
                        alt='logo-blog'
                        className='blog-logo'
                    />
                    <div className='title-detail'>
                        <div className='title-content'>
                            {t1}
                        </div>
                        <div className='note'>
                            {t2}
                        </div>
                    </div>
                </div>

                <div className='show'>
                    {blogs?.length > 0 ? (
                        blogs.map((blog, index) => (
                            <div className='blog-card' key={index}>
                                <img
                                    src={`${serverApi}${blogImageApi}/${blog.image}`}
                                    alt={blog.title}
                                    className='blog-thumbnail'
                                />
                                <div className='blog-content'>
                                    <div className='blog-title'>
                                        <img
                                            src={LogoBlog}
                                            alt='logo-blog'
                                            className='blog-logo'
                                        />
                                        {blog.title}
                                    </div>
                                    <div className='blog-details'>{blog.details}</div>
                                    <div className='blog-meta'>
                                        <div>Người đăng: {blog.userName}</div>
                                        <div>Ngày đăng: {blog.date.split('T')[0]}</div>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className='no-blogs'>Không có bài viết nào</div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default BlogPage;
