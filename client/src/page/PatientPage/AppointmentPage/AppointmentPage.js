// Modules
import { useRef, useEffect } from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

// Style sheet
import './AppointmentPage.css';

// Assets

// Components
import TabMenu from '../../../component/TabMenu/TabMenu';
import DoctorList from '../../../component/DoctorList/DoctorList';
import CalendarSelection from '../../../component/CalendarSelection/CalendarSelection';
import ConfirmAppointmentBox from '../../../component/ConfirmAppointmentBox/ConfirmAppointmentBox';
import TextBox from '../../../component/TextBox/TextBox';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';

// Hooks
import useGetPatientProfile from '../../../hook/useGetPatientProfile';

function AppointmentPage({ user }) {
    const t1 = 'Đặt lịch hẹn';
    const t2 = 'Chọn bác sĩ và thời gian phù hợp cho cuộc hẹn của bạn';
    const t3 = 'Chọn theo bác sĩ';
    const t4 = 'Chọn theo ngày';
    const t5 = 'Hãy cập nhật hồ sơ để đăng ký lịch'
    const t6 = 'Đang chuyển hướng đến hồ sơ trong';
    const t7 = 'Hãy chọn loại cuộc hẹn dựa trên mong muốn của bạn';
    const t9 = 'Khám/Điều trị';
    const t10 = 'Tư vấn trực tuyến';

    const t16 = 'Trờ lại';

    const t8 = 'Mô tả';
    const t14m1 = 'Hình thức';
    const t14 = 'Chi phí đăng ký';
    const t11 = 'Trực tiếp tại cơ sở (Offline)';
    const t12 = 'Trực tuyến (Online)';
    const t15 = 'Sau khi đăng ký dịch vụ này, người dùng sẽ nhận';
    const t19 = 'mã định danh (số và QR)';
    const t20 = 'chứa thông tin cá nhân và lịch hẹn để sử dụng khi đến khám tại cơ sở.';
    const t17 = 'Dịch vụ này cung cấp';
    const t21 = 'đường dẫn Zoom';
    const t22 = 'giúp kết nối người dùng với tư vấn viên để trao đổi trực tuyến.';
    const t18 = '30.000 VNĐ/lần';

    const t23 = 'Bác sĩ phụ trách khám và điều trị';
    const t24 = 'Các bác sĩ sẽ gặp trực tiếp cùng với bạn để hỗ trợ bạn trong việc khám và điều trị cho bạn'
    const t25 = 'Tư vấn viên phụ trách tư vấn trực tuyến';
    const t26 = 'Các tư vấn viên sẽ là người lắng nghe và hỗ trợ bạn trong quá trình điều trị';

    const t27 = 'Xác nhận thông tin';
    const t28 = 'Bạn hãy kiểm tra lại thông tin đăng ký lịch của mình trước khi xác nhận';
    const defaultState = 'byDoctor';

    const [chooseByDoctor, setChooseByDoctor] = useState(true);
    const [chooseByDate, setChooseByDate] = useState(false);

    const [choosenDate, setChoosenDate] = useState(null);
    const [choosenDoctor, setChoosenDoctor] = useState(null);

    const [choosenAppointment, setChoosenAppointment] = useState(null);
    const [type, setType] = useState(null);
    const calendarRef = useRef(null);
    const confirmRef = useRef(null);
    const navigate = useNavigate();
    const [activeTabId, setActiveTabId] = useState(defaultState);

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const {
        profile, finish
    } = useGetPatientProfile({ setError, setLoading, userId: user?.UserId })

    useEffect(() => {
        if (!profile && finish) {
            let seconds = 5;

            setError(
                <>
                    <div>{t5}</div>
                    <div>{t6}&nbsp;{seconds}s</div>
                </>
            );

            const interval = setInterval(() => {
                seconds -= 1;
                if (seconds > 0) {
                    setError(
                        <>
                            <div>{t5}</div>
                            <div>{t6}&nbsp;{seconds}s</div>
                        </>
                    );
                }
            }, 1000);

            const timeout = setTimeout(() => {
                clearInterval(interval);
                navigate('/patientProfile');
            }, 5000);

            return () => {
                clearTimeout(timeout);
                clearInterval(interval);
            };
        }
    }, [profile]);


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
        {
            id: 'byDoctor',
            label: t3,
            action: () => {
                handleOpenChooseByDoctor();
                setActiveTabId('byDoctor');
            }
        },
        {
            id: 'byDate',
            label: t4,
            action: () => {
                handleOpenChooseByDate();
                setActiveTabId('byDate');
            }
        }
    ];


    return (
        <div id='appointment-page'>
            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {loading && (
                <SkeletonUI />
            )}

            <div className='header'>
                <h1>
                    {t1}
                </h1>
                <p>
                    {type ? t2 : t7}
                </p>
            </div>

            {!type && (
                <div className='type'>
                    <div className='type-selection'>
                        <button
                            className='offline'
                            onClick={() => {
                                setType(process.env.REACT_APP_ROLE_DOCTOR_ID);
                            }}
                        >
                            <p className='type-name'>{t9}</p>
                        </button>
                        <div className='note'>
                            <div className='type-detail'>
                                <div className='type-detail-title'>
                                    {t8}
                                </div>
                                <div className='type-detail-content'>
                                    {t15}&nbsp;<span className='qr'>{t19}</span>&nbsp;{t20}
                                </div>
                            </div>
                            <div className='type-detail'>
                                <div className='type-detail-title'>
                                    {t14m1}
                                </div>
                                <div className='type-detail-content'>
                                    {t11}
                                </div>
                            </div>
                            <div className='type-detail cost'>
                                <div className='type-detail-title'>
                                    {t14}
                                </div>
                                <div className='type-detail-content'>
                                    {t18}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className='type-selection'>
                        <button
                            className='online'
                            onClick={() => {
                                setType(process.env.REACT_APP_ROLE_STAFF_ID);
                            }}
                        >
                            <p className='type-name'>{t10}</p>
                        </button>
                        <div className='note'>
                            <div className='type-detail'>
                                <div className='type-detail-title'>
                                    {t8}
                                </div>
                                <div className='type-detail-content'>
                                    {t17}&nbsp;<span className='zoom'>{t21}</span>&nbsp;{t22}
                                </div>
                            </div>
                            <div className='type-detail'>
                                <div className='type-detail-title'>
                                    {t14m1}
                                </div>
                                <div className='type-detail-content'>
                                    {t12}
                                </div>
                            </div>
                            <div className='type-detail cost'>
                                <div className='type-detail-title'>
                                    {t14}
                                </div>
                                <div className='type-detail-content'>
                                    {t18}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {!choosenAppointment && type && (
                <TabMenu
                    tabs={tabs}
                    activeTabId={activeTabId}
                    setActiveTabId={setActiveTabId}
                />
            )}

            {!choosenAppointment && chooseByDoctor && type && (
                <>
                    <div className='sub-header'>
                        <div className='previous'>
                            <button
                                onClick={() => {
                                    setChoosenAppointment(null);
                                    setChoosenDoctor(null);
                                    setChoosenDate(null);
                                    setActiveTabId(defaultState);
                                    setType(null);
                                }}
                            >
                                {t16}
                            </button>
                        </div>

                        <div className='title'>
                            <div className='main-title'>
                                {type === process.env.REACT_APP_ROLE_DOCTOR_ID ? t23 : t25}
                            </div>
                            <div className='sub-title'>
                                {type === process.env.REACT_APP_ROLE_DOCTOR_ID ? t24 : t26}
                            </div>
                        </div>
                    </div>


                    <DoctorList
                        choosenDate={null}
                        setChoosenDoctor={setChoosenDoctor}
                        type={type}
                    />

                    {choosenDoctor && (
                        <div ref={calendarRef}>
                            <CalendarSelection
                                type={type}
                                doctor={choosenDoctor}
                                setChoosenAppointment={setChoosenAppointment}
                            />
                        </div>
                    )}
                </>
            )}

            {!choosenAppointment && chooseByDate && type && (
                <>
                    <div className='sub-header'>
                        <div className='previous'>
                            <button
                                onClick={() => {
                                    setChoosenAppointment(null);
                                    setChoosenDoctor(null);
                                    setChoosenDate(null);
                                    setActiveTabId(defaultState);
                                    setType(null);
                                }}
                            >
                                {t16}
                            </button>
                        </div>

                        <div className='title'>
                            <div className='main-title'>
                                {t23}
                            </div>
                            <div className='sub-title'>
                                {t24}
                            </div>
                        </div>
                    </div>

                    <CalendarSelection
                        doctor={null}
                        type={type}
                        setChoosenDate={setChoosenDate}
                    />

                    {choosenDate && (
                        <div ref={calendarRef}>
                            <DoctorList
                                type={type}
                                choosenDate={choosenDate}
                                setChoosenAppointment={setChoosenAppointment}
                            />
                        </div>
                    )}
                </>
            )}


            {choosenAppointment && type && (
                <div
                    className='confirm'
                    ref={confirmRef}>
                    <div className='sub-header'>
                        <div className='previous'>
                            <button
                                onClick={() => {
                                    setChoosenAppointment(null);
                                    setChoosenDoctor(null);
                                    setChoosenDate(null);
                                    setActiveTabId(defaultState);
                                    setType(null);
                                }}
                            >
                                {t16}
                            </button>
                        </div>

                        <div className='title'>
                            <div className='main-title'>
                                {t27}
                            </div>
                            <div className='sub-title'>
                                {t28}
                            </div>
                        </div>
                    </div>

                    <ConfirmAppointmentBox
                        setChoosenAppointment={setChoosenAppointment}
                        choosenAppointment={choosenAppointment} user={user}
                        type={type}
                    />
                </div>
            )}
        </div>
    )
}

export default AppointmentPage;