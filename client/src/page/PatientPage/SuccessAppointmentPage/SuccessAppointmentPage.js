import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import './SuccessAppointmentPage.css';
import FinishIcon from '../../../uploads/icon/finish.png';
import Icon from '../../../component/Icon/Icon';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ZoomIcon from '../../../uploads/icon/zoom.png';

function SuccessAppointmentPage({ user }) {
    const navigate = useNavigate();
    const searchParams = useParams();
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;
    const serverApi = process.env.REACT_APP_SERVER_API;
    const qrApi = process.env.REACT_APP_QRCODE_ASSET_API;
    const [type, setType] = useState(null);
    const [result, setResult] = useState(null);
    const [error, setError] = useState(null);

    const TEXT = {
        successTitle: 'Đặt lịch thành công',
        doctor: 'Bác sĩ',
        date: 'Ngày',
        time: 'Thời gian',
        type: 'Loại cuộc hẹn',
        queue: 'Số thứ tự',
        zoomInfo: 'Đường dẫn của Zoom meeting đã được gửi tới',
        offline: 'Khám/Điều trị (Offline)',
        online: 'Tư vấn trực tiếp (Online)',
        continue: 'Tiếp tục đặt hẹn',
        home: 'Về trang chủ',
        doctorPrefix: 'BS.',
        separator: '-',
        location: 'Địa điểm',
        qrCode: 'Mã QR',
        zoom: 'Đường dẫn Zoom đã được gửi qua email'
    };

    useEffect(() => {
        const orderCode = searchParams.orderCode;
        if (!orderCode) return;

        axios.get(`${serverApi}${appointmentControllerApi}/success/${orderCode}`)
            .then((res) => {
                setResult(res.data);
                setType(res.data.isOnline ? 'online' : 'offline');
            })
            .catch((err) => {
                console.error("❌ Failed to fetch appointment:", err);
                setError("Không thể tải thông tin cuộc hẹn.");
            });
    }, [searchParams]);

    if (error) {
        return (
            <div id="success-appointment-page" className="error-message">
                <p>{error}</p>
                <button onClick={() => navigate('/appointmentPage')}>
                    {TEXT.continue}
                </button>
            </div>
        );
    }


    return (
        <div id='success-appointment-page'>
            {result ? (
                <div className='finish'>
                    <div className='header'>
                        <div className='title'>{TEXT.successTitle}</div>
                        <div className='icon-holder'>
                            <Icon src={FinishIcon} alt='confirm-icon' />
                        </div>
                    </div>

                    <div className='body'>
                        <div className='finish-detail'>
                            <div className='doctor'>
                                <div className='title'>{TEXT.doctor}</div>
                                <div className='detail'>
                                    <p className='name'>{TEXT.doctorPrefix} {result.doctorName}</p>
                                </div>
                            </div>

                            <div className='date'>
                                <div className='title'>{TEXT.date}</div>
                                <div className='detail'>{result.scheduleDate?.split('T')[0]}</div>
                            </div>

                            <div className='time'>
                                <div className='title'>{TEXT.time}</div>
                                <div className='detail'>
                                    {result.startTime} {TEXT.separator} {result.endTime}
                                </div>
                            </div>

                            <div className='location'>
                                <div className='title'>{TEXT.location}</div>
                                <div className='detail'>
                                    {result.location}
                                </div>
                            </div>

                            <div className='date'>
                                <div className='title'>{TEXT.type}</div>
                                <div className='detail'>
                                    {type === 'offline' ? TEXT.offline : TEXT.online}
                                </div>
                            </div>

                            {type === 'online' && (
                                <div className='note'>
                                    <p>
                                        <span>{TEXT.zoomInfo}</span>&nbsp;
                                        <span className='email'>{user?.Email}</span>
                                    </p>
                                </div>
                            )}
                        </div>
                        <div className='qr-code'>
                            {type === 'offline' ? (
                                <div className='qr'>
                                    <img src={`${serverApi}${qrApi}/${result.orderCode}.png`} />
                                    <div className='title'>
                                        {TEXT.qrCode}
                                    </div>
                                </div>
                            ) : (
                                <div className='zoom'>
                                    <Icon src={ZoomIcon} alt={'zoom-icon'} />
                                    <div className='title'>
                                        {TEXT.zoom}
                                    </div>
                                </div>
                            )}

                            <div className='queue-number'>
                                <div className='queue'>
                                    {result.queueNumber}
                                </div>
                                <div className='title'>
                                    {TEXT.queue}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className='footer'>
                        <button
                            onClick={() => {
                                setResult(null);
                                navigate('/appointmentPage');
                            }}
                            className='new-request'
                        >
                            {TEXT.continue}
                        </button>

                        <button
                            onClick={() => navigate('/homePage')}
                            className='back'
                        >
                            {TEXT.home}
                        </button>
                    </div>
                </div>
            ) : (
                <SkeletonUI />
            )}
        </div>
    );
}

export default SuccessAppointmentPage;