// Modules
import { useNavigate } from 'react-router-dom';
import { useRef, useEffect } from 'react';

// Style sheet
import './AppointmentPage.css';

// Assets
import FinishIcon from '../../../uploads/icon/finish.png';

// Components
import Icon from '../../../component/Icon/Icon';
import TabBar from '../../../component/TabBar/TabBar';
import DoctorList from '../../../component/DoctorList/DoctorList';
import CalendarSelection from '../../../component/CalendarSelection/CalendarSelection';
import ConfirmAppointmentBox from '../../../component/ConfirmAppointmentBox/ConfirmAppointmentBox';

// Hooks
import { useState } from 'react';

function AppointmentPage({ user }) {
    const t1 = 'Đặt lịch hẹn';
    const t2 = 'Chọn bác sĩ và thời gian phù hợp cho cuộc hẹn của bạn';
    const t3 = 'Chọn theo bác sĩ';
    const t4 = 'Chọn theo ngày';

    const t5 = 'Đặt lịch thành công';
    const t6 = 'Bác sĩ';
    const t7 = 'Ngày';
    const t8 = 'Thời gian';
    const t9 = 'Loại cuộc hẹn';
    const t10 = 'Tiếp tục đặt hẹn';
    const t11 = 'Về trang chủ';
    const t12 = 'Khám/Điều trị (Offline)';
    const t14m1 = 'Tư vấn trực tiếp (Online)';
    const t14 = 'BS.';
    const t15 = '-';
    const t16 = 'Trờ lại';
    const t17 = 'Đường dẫn của Zoom meeting đã được gửi tới';

    const [chooseByDoctor, setChooseByDoctor] = useState(true);
    const [chooseByDate, setChooseByDate] = useState(false);
    const [choosenDoctor, setChoosenDoctor] = useState(null);
    const [choosenAppointment, setChoosenAppointment] = useState(null);
    const [type, setType] = useState(null);
    const [openFinish, setOpenFinish] = useState(false);
    const navigate = useNavigate();
    const calendarRef = useRef(null);
    const confirmRef = useRef(null);

    useEffect(() => {
        if (choosenDoctor && calendarRef.current) {
            calendarRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [choosenDoctor]);

    useEffect(() => {
        if (choosenAppointment && confirmRef.current) {
            confirmRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [choosenAppointment]);

    const handleOpenChooseByDoctor = () => {
        setChooseByDate(false);
        setChooseByDoctor(true);
    }

    const handleOpenChooseByDate = () => {
        setChooseByDoctor(false);
        setChoosenDoctor(null);
        setChooseByDate(true);
    }

    const tabs = [
        { name: t3, action: handleOpenChooseByDoctor },
        { name: t4, action: handleOpenChooseByDate }
    ];


    return (
        <div id='appointment-page'>
            <div className='header'>
                <h1>
                    {t1}
                </h1>
                <p>
                    {t2}
                </p>
            </div>

            {!openFinish && !choosenAppointment && (
                <TabBar tabs={tabs} />
            )}

            {!openFinish && !choosenAppointment && chooseByDoctor && (
                <>
                    <DoctorList setChoosenDoctor={setChoosenDoctor} />

                    {choosenDoctor && (
                        <div ref={calendarRef}>
                            <CalendarSelection
                                doctor={choosenDoctor}
                                setChoosenAppointment={setChoosenAppointment}
                            />
                        </div>
                    )}
                </>
            )}

            {!openFinish && choosenAppointment && (
                <div
                    className='confirm'
                    ref={confirmRef}>
                    <div className='previous'>
                        <button
                            onClick={() => {
                                setChoosenAppointment(null);
                                setChoosenDoctor(null);
                            }}
                        >
                            {t16}
                        </button>
                    </div>
                    <ConfirmAppointmentBox
                        setChoosenAppointment={setChoosenAppointment}
                        choosenAppointment={choosenAppointment} user={user}
                        setOpenFinish={setOpenFinish}
                        type={type}
                        setType={setType}
                    />
                </div>
            )}

            {openFinish && (
                <div className='finish'>
                    <div className='header'>
                        <div className='title'>
                            {t5}
                        </div>

                        <div className='icon-holder'>
                            <Icon src={FinishIcon} alt={'confirm-icon'} />
                        </div>
                    </div>

                    <div className='body'>
                        <div className='doctor'>
                            <div className='title'>
                                {t6}
                            </div>

                            <div className='detail'>
                                <p className='name'>{t14}&nbsp;{choosenAppointment.doctor.name}</p>
                                <p className='specialties'>{choosenAppointment.doctor.specialties}</p>
                            </div>
                        </div>

                        <div className='date'>
                            <div className='title'>
                                {t7}
                            </div>

                            <div className='detail'>
                                {choosenAppointment.schedule.date}
                            </div>
                        </div>

                        <div className='time'>
                            <div className='title'>
                                {t8}
                            </div>

                            <div className='detail'>
                                {choosenAppointment.schedule.startTime}
                                &nbsp;{t15}&nbsp;
                                {choosenAppointment.schedule.endTime}
                            </div>
                        </div>

                        <div className='date'>
                            <div className='title'>
                                {t9}
                            </div>

                            <div className='detail'>
                                {type === 'offline' ? t12 : t14m1}
                            </div>
                        </div>

                        { type === 'online' && (
                            <div className='note'>
                                <p>
                                    <span>{t17}</span>&nbsp;<span className='email'>{user.Email}</span>
                                </p>
                            </div>
                        )}
                    </div>

                    <div className='footer'>
                        <button
                            onClick={() => {
                                setOpenFinish(false);
                                setChoosenDoctor(null);
                                setChoosenAppointment(null);
                                navigate('/appointmentPage');
                            }}
                            className='new-request'
                        >
                            {t10}
                        </button>

                        <button
                            onClick={() => navigate('/homePage')}
                            className='back'
                        >
                            {t11}
                        </button>
                    </div>
                </div>
            )}
        </div>
    )
}

export default AppointmentPage;