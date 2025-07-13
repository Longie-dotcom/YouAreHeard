// Modules
import { useState, useRef, useEffect } from 'react';

// Styling sheet
import './PostBlogForm.css';

// Assets
import UploadImageIcon from '../../uploads/icon/upload-image.png';
import UploadIcon from '../../uploads/icon/upload.png';
import ExitIcon from '../../uploads/icon/exit.png';
import Blog from '../../uploads/icon/album.png';
import LogoBlog from '../../uploads/logo-blog.png';
import LogoText from '../../uploads/logo-text.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import usePostBlog from '../../hook/usePostBlog';

function PostBlogForm({ user, setOpenPostForm, setError, setLoading, setReload, setFinish }) {
    const t1 = 'Tiêu đề của bài blog';
    const t2 = 'Nội dung của bài blog';
    const t3 = 'Mẫu thông tin tạo bài viết (blog)';
    const t4 = 'Tải ảnh lên';
    const t5 = 'Đăng bài viết';

    const [title, setTitle] = useState(null);
    const [details, setDetails] = useState(null);
    const [imagePreview, setImagePreview] = useState(null);
    const [selectedFile, setSelectedFile] = useState(null);

    const titleRef = useRef(null);
    const detailsRef = useRef(null);

    const {
        postBlog
    } = usePostBlog({ setError, setLoading, setReload, setFinish });

    useEffect(() => {
        autoResize(titleRef);
        autoResize(detailsRef);
    }, []);

    const autoResize = (ref) => {
        if (ref?.current) {
            ref.current.style.height = 'auto';
            ref.current.style.height = `${ref.current.scrollHeight}px`;
        }
    };

    return (
        <div
            className='post-blog-form-overlap'
            onClick={(e) => {
                if (!e.target.closest('.post-blog-form')) {
                    setOpenPostForm(false);
                    e.stopPropagation();
                }
            }}
        >
            <div className='section post-blog-form'>
                <div className='section picture'>
                    {imagePreview ? (
                        <div className='preview-wrapper'>
                            <img src={imagePreview} alt='preview' className='image-preview' />
                            <button
                                className='remove-btn'
                                onClick={() => {
                                    setImagePreview(null);
                                    setSelectedFile(null);
                                }}
                            >
                                <Icon src={ExitIcon} alt={'exit-icon'} />
                            </button>
                        </div>
                    ) : (
                        <label className='upload-label'>
                            <input
                                type='file'
                                accept='image/*'
                                onChange={(e) => {
                                    const file = e.target.files[0];
                                    if (file) {
                                        setSelectedFile(file);

                                        const reader = new FileReader();
                                        reader.onloadend = () => setImagePreview(reader.result);
                                        reader.readAsDataURL(file);
                                    }
                                }}
                                hidden
                            />
                            <span>
                                <Icon src={UploadImageIcon} alt={'upload-image-icon'} />
                                {t4}
                            </span>
                        </label>
                    )}
                </div>


                <div className='section content'>
                    <div className='header'>
                        <div className='main-title'>
                            <Icon src={Blog} alt={'logo-blog'} />
                            {t3}
                        </div>

                        <button
                            onClick={() => setOpenPostForm(false)}
                            className='exit'
                        >
                            <Icon src={ExitIcon} alt={'exit-icon'} />
                        </button>
                    </div>

                    <div className='input-group title'>
                        <div className='input-title'>
                            {t1}
                        </div>
                        <textarea
                            ref={titleRef}
                            minLength={5}
                            maxLength={140}
                            className='title'
                            value={title}
                            onChange={(e) => {
                                setTitle(e.target.value);
                                autoResize(titleRef);
                            }}
                            onInput={() => autoResize(titleRef)}
                        />
                    </div>

                    <div className='input-group details'>
                        <div className='input-title'>
                            {t2}
                        </div>
                        <textarea
                            ref={detailsRef}
                            minLength={5}
                            maxLength={1000}
                            className='details'
                            value={details}
                            onChange={(e) => {
                                setDetails(e.target.value);
                                autoResize(detailsRef);
                            }}
                            onInput={() => autoResize(detailsRef)}
                        />
                    </div>

                    <div className='footer'>
                        <button
                            onClick={async () => {
                                await postBlog({
                                    title,
                                    details,
                                    imageFile: selectedFile,
                                    userId: user?.UserId,
                                });

                                // Only reset if postBlog doesn't return an error
                                setTitle(null);
                                setDetails(null);
                                setSelectedFile(null);
                                setImagePreview(null);
                            }}
                            className='submit'
                        >
                            <Icon src={UploadIcon} alt={'upload-icon'} />
                            {t5}
                        </button>

                        <div className='logo'>
                            <img src={LogoBlog} alt='logo-picture' />
                            <img src={LogoText} alt='logo-text' />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default PostBlogForm;