// Modules

// Styling sheet
import './DoctorList.css';

// Assets
import StarIcon from '../../uploads/icon/star.png';
import UserIcon from '../../uploads/icon/user.png';

// Components
import Icon from '../Icon/Icon';
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import CalendarSelection from '../CalendarSelection/CalendarSelection';

// Hooks
import useLoadAllDoctor from '../../hook/useLoadAllDoctor';
import { useState } from 'react';
import DoctorProfileBox from '../DoctorProfileBox/DoctorProfileBox';

function DoctorList({ type, setChoosenDoctor, choosenDate, setChoosenAppointment }) {
    const t1 = 'Năm kinh nghiệm';
    const t2 = 'Có sẵn: ';
    const t3 = 'Đánh giá: ';
    const t4 = 'Đặt lịch';
    const t5 = 'Xem thêm';
    const t6 = 'BS.';

    const weekdayTranslations = {
        Monday: "Thứ Hai",
        Tuesday: "Thứ Ba",
        Wednesday: "Thứ Tư",
        Thursday: "Thứ Năm",
        Friday: "Thứ Sáu",
        Saturday: "Thứ Bảy",
        Sunday: "Chủ Nhật"
    };

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);  
    const [viewDoctor, setViewDoctor] = useState(null);

    const serverApi = process.env.REACT_APP_SERVER_API;
    const doctorAvatarApi = process.env.REACT_APP_DOCTOR_AVATAR_ASSET_API;
    
    const {
        doctors
    } = useLoadAllDoctor({ setError, setLoading, roleId: type });

    return (
        <div className="doctor-list" style={{ height: 'fit-content' }}>

            {!choosenDate && doctors && (doctors.map((doctor) => (
                <div key={doctor.userID} className="doctor-card">
                    <div className='header'>
                        <div className='avatar'>
                            <img src={`${serverApi}${doctorAvatarApi}/${doctor.image}`} alt={doctor.name} className="doctor-image" />
                        </div>
                        <div className='detail'>
                            <h1 className='name'>
                                <Icon src={UserIcon} alt={'user-icon'} />
                                {t6}{doctor.name}
                            </h1>
                            <p className='specialties'>
                                <Icon src={StarIcon} alt={'star-icon'} />
                                {doctor.specialties}
                            </p>
                            <p className='experience'>
                                {doctor.yearsOfExperience}&nbsp;{t1}
                            </p>
                        </div>
                    </div>
                    <div className='body'>
                        <p className='available-days'>
                            {t2}{
                                [...new Set(doctor.availableDays.split(',').map(day => day.trim()))]
                                    .map(day => weekdayTranslations[day] || day) // translate
                                    .join(', ')
                            }
                        </p>
                        <p className='rating'>
                            {t3}
                        </p>
                    </div>
                    <div className='footer'>
                        <button onClick={() => setViewDoctor(doctor)} className='view'>
                            {t5}
                        </button>

                        <button onClick={() => setChoosenDoctor(doctor)} className='book'>
                            {t4}
                        </button>
                    </div>
                </div>
            )))}

            {choosenDate && (choosenDate.map((schedule) => (
                <div key={schedule.doctorProfile.userID} className="doctor-card" style={{ padding: '6%' }}>
                    <div className='header'>
                        <div className='avatar'>
                            <img src={`${serverApi}${doctorAvatarApi}/${schedule.doctorProfile.image}`} alt={schedule.doctorProfile.name} className="doctor-image" />
                        </div>
                        <div className='detail'>
                            <h1 className='name'>
                                <Icon src={UserIcon} alt={'user-icon'} />
                                {t6}{schedule.doctorProfile.name}
                            </h1>
                            <p className='specialties'>
                                <Icon src={StarIcon} alt={'star-icon'} />
                                {schedule.doctorProfile.specialties}
                            </p>
                            <p className='experience'>
                                {schedule.doctorProfile.yearsOfExperience}&nbsp;{t1}
                            </p>
                        </div>
                    </div>
                    <div className='body'>
                        <p className='rating'>
                            {t3}
                        </p>
                    </div>
                    <div className='footer'>
                        <button onClick={() => setViewDoctor(schedule.doctorProfile)} className='view'>
                            {t5}
                        </button>

                        <button onClick={() => setChoosenAppointment({ doctor: schedule.doctorProfile, schedule: schedule })} className='book'>
                            {t4}
                        </button>
                    </div>
                </div>
            )))}

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {viewDoctor && (
                <DoctorProfileBox viewDoctor={viewDoctor} setViewDoctor={setViewDoctor} setChoosenDoctor={setChoosenDoctor}/>
            )}
        </div>
    )
}

export default DoctorList;