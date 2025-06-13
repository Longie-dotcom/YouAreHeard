// Modules

// Styling sheet
import './DoctorProfileBox.css';

// Assets
import LanguageIcon from '../../uploads/icon/language.png';
import AlbumIcon from '../../uploads/icon/album.png';
import ExitIcon from '../../uploads/icon/exit.png';
import NewsPaperIcon from '../../uploads/icon/newspaper.png';
import StarIcon from '../../uploads/icon/star.png';
import PersonIcon from '../../uploads/icon/person.png';
import LogoPicture from '../../uploads/logo-picture.png';
import LogoText from '../../uploads/logo-text.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function DoctorProfileBox({ viewDoctor, setViewDoctor, setChoosenDoctor }) {
    const t1 = 'Thông tin bác sĩ';
    const t2 = 'BS.';
    const t3 = 'Bác sĩ';
    const t4 = 'Ngôn ngữ giao tiếp';
    const t5 = 'Mô tả chung';
    const t6 = 'Đặt lịch';
    const t7 = 'năm kinh ngiệm';

    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorAvatarApi = process.env.REACT_APP_DOCTOR_AVATAR_ASSET_API;

    return (
        <div
            className='doctor-profile-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.doctor-profile-box')) {
                    setViewDoctor(null);
                    e.stopPropagation();
                }
            }}
        >

            <div className='doctor-profile-box'>
                <div className='doctor-image'>
                    <img src={`${serverApi}${doctorAvatarApi}/${viewDoctor.image}`} alt={viewDoctor.name} className="doctor-image" />
                </div>

                <div className='doctor-information'>
                    <div className='header'>
                        <div className='title'>
                            <Icon src={AlbumIcon} alt={'album-icon'} />
                            &nbsp;&nbsp;&nbsp;&nbsp;{t1}
                        </div>

                        <button
                            onClick={() => setViewDoctor(null)}
                            className='exit'
                        >
                            <Icon src={ExitIcon} alt={'exit'} />
                        </button>
                    </div>

                    <div className='body'>
                        <div className='info doctor'>
                            <Icon src={PersonIcon} alt={'user-icon'} />
                            <div className='detail'>
                                <div className='name'>
                                    {t2}{viewDoctor.name}
                                </div>
                                <div className='gender'>
                                    {t3}&nbsp;{viewDoctor.gender}
                                </div>
                            </div>
                        </div>

                        <div className='info special'>
                            <Icon src={StarIcon} alt={'star-icon'} />
                            <div className='detail'>
                                <div className='specialties'>
                                    {viewDoctor.specialties}
                                </div>
                                <div className='experience'>
                                    {viewDoctor.yearsOfExperience}&nbsp;{t7}
                                </div>
                            </div>
                        </div>

                        <div className='info language'>
                            <Icon src={LanguageIcon} alt={'language-icon'} />
                            <div className='detail'>
                                <div className='language-title'>
                                    {t4}
                                </div>
                                <div className='languages'>
                                    {viewDoctor.languagesSpoken}
                                </div>
                            </div>
                        </div>

                        <div className='info description'>
                            <Icon src={NewsPaperIcon} alt={'newspaper-icon'} />
                            <div className='detail'>
                                <div className='description-title'>
                                    {t5}
                                </div>
                                <div className='descriptions'>
                                    {viewDoctor.description}
                                </div>
                            </div>
                        </div>

                        <button
                            onClick={() => {
                                setViewDoctor(null);
                                setChoosenDoctor(viewDoctor);
                            }}
                            className='request'
                        >
                            {t6}
                        </button>
                    </div>

                    <div className='footer'>
                        <div className='logo-box'>
                            <img className='logo' src={LogoPicture} alt='logo-picture' />
                            <img className='logo' src={LogoText} alt='logo-text' />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default DoctorProfileBox;