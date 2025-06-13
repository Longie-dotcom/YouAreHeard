// Modules
import { useRef, useState } from 'react';

// Styling sheet
import './ConfirmAppointmentBox.css';

// Assets
import PersonIcon from '../../uploads/icon/person.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';

// Components
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import Icon from '../Icon/Icon';
import ConfirmBox from '../ConfirmBox/ConfirmBox';

// Hooks
import useRequestAppointment from '../../hook/useRequestAppointment';

function ConfirmAppointmentBox({ user, choosenAppointment, setOpenFinish, setType, type }) {
    const t1 = 'Chi tiết cuộc hẹn';
    const t2 = 'Chọn loại cuộc hẹn';
    const t3 = 'Lý do cho cuộc hẹn (thêm)';
    const t4 = 'Ghi chú (thêm)';
    const t5 = 'Ẩn danh tính trong quá trình tư vấn';
    const t6 = 'Xác nhận thông tin';
    const t7 = 'Nhập lý do';
    const t8 = 'Nhập ghi chú';
    const t9 = 'Khám/Điều trị';
    const t10 = 'Tư vấn trực tuyến';
    const t11 = 'Offline';
    const t12 = 'Online';
    const t14m1 = 'Xác nhận thông tin';
    const t14 = 'Trở lại';
    const t15 = 'BS.';
    const t16 = '-';
    const t17 = 'Xác nhận gửi yêu cầu';
    const t18 = 'Zoom meeting';

    const reasonRef = useRef(null);
    const noteRef = useRef(null);
    const [openConfirm, setOpenConfirm] = useState(false);
    const [activeButton, setActiveButton] = useState('');

    const autoGrow = (element) => {
        element.style.height = "auto";
        element.style.height = `${element.scrollHeight}px`;
    };

    const {
        loading, setLoading,
        error, setError,
        anonymous, setAnonymous,
        setNote, setReason,
        handleRequest
    } = useRequestAppointment({ user, choosenAppointment, setOpenFinish, type });

    return (
        <div className='confirm-appointment-box'>
            <div className='header'>

            </div>

            <div className='body'>
                <div className='detail'>
                    <div className='title'>
                        {t1}
                    </div>

                    <div className='type'>
                        <label>{t2}</label>
                        <div className='type-selection'>
                            <button
                                className={activeButton == 'type-1' ? 'active-button' : ''}
                                onClick={() => {
                                    setType('offline');
                                    setActiveButton('type-1')
                                }}
                            >
                                <p className='type-name'>{t9}</p>
                                <p className='type'>{t11}</p>
                            </button>
                            <button
                                className={activeButton == 'type-2' ? 'active-button' : ''}
                                onClick={() => {
                                    setType('online');
                                    setActiveButton('type-2')
                                }}
                            >
                                <p className='type-name'>{t10}</p>
                                <p className='type'>{t12}</p>
                            </button>
                        </div>
                    </div>

                    <div className='reason'>
                        <label>{t3}</label>
                        <textarea
                            onChange={(e) => setReason(e.target.value)}
                            ref={reasonRef}
                            onInput={(e) => autoGrow(e.target)}
                            rows={2}
                            placeholder={t7}
                        />
                    </div>

                    <div className='note'>
                        <label>{t4}</label>
                        <textarea
                            onChange={(e) => setNote(e.target.value)}
                            ref={noteRef}
                            onInput={(e) => autoGrow(e.target)}
                            rows={2}
                            placeholder={t8}
                        />
                    </div>

                    <div className='anonymous'>
                        <input 
                            type='checkbox' 
                            checked={anonymous}
                            onChange={(e) => setAnonymous(e.target.checked)}
                        />
                        <label>{t5}</label>
                    </div>
                </div>

                <div className='confirm'>
                    <div className='title'>
                        {t6}
                    </div>

                    <div className='name'>
                        <div className='icon-holder'>
                            <Icon src={PersonIcon} alt={'person-icon'} />
                        </div>
                        <div className='doctor-detail'>
                            <p>
                                {t15}&nbsp;{choosenAppointment.doctor.name}
                            </p>
                            <p className='specialties'>
                                {choosenAppointment.doctor.specialties}
                            </p>
                        </div>
                    </div>

                    <div className='date'>
                        <div className='icon-holder'>
                            <Icon src={ScheduleIcon} alt={'schedule-icon'} />
                        </div>
                        <p>
                            {new Intl.DateTimeFormat('vi-VN').format(new Date(choosenAppointment.schedule.date))}
                        </p>
                    </div>

                    <div className='time'>
                        <div className='icon-holder'>
                            <Icon src={TimeIcon} alt={'time-icon'} />
                        </div>
                        <p>
                            {choosenAppointment.schedule.startTime}&nbsp;{t16}&nbsp;
                            {choosenAppointment.schedule.endTime}
                        </p>
                    </div>

                    <div className='location'>
                        <div className='icon-holder'>
                            <Icon src={LocationIcon} alt={'time-icon'} />
                        </div>
                        <p>
                            {type === 'online' ? t18 : choosenAppointment.schedule.location}
                        </p>
                    </div>

                    <button
                        onClick={() => setOpenConfirm(true)}
                        className='submit-button'
                    >
                        {t14m1}
                    </button>
                </div>
            </div>

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {loading && (
                <SkeletonUI loading={loading} />
            )}

            {openConfirm && (
                <ConfirmBox text={t17} setOpenConfirm={setOpenConfirm} action={handleRequest} />
            )}
        </div>
    )
}

export default ConfirmAppointmentBox;