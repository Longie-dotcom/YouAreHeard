import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import './VerifyPage.css';
import PersonIcon from '../../../uploads/icon/person.png';
import LogoText from '../../../uploads/logo-text.png';
import LogoPicture from '../../../uploads/logo-picture.png';

import Icon from '../../../component/Icon/Icon';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';

function VerifyPage() {
    const searchParams = useParams();
    const appointmentControllerApi = process.env.REACT_APP_APPOINTMENT_CONTROLLER_API;
    const serverApi = process.env.REACT_APP_SERVER_API;
    const [type, setType] = useState(null);
    const [result, setResult] = useState(null);
    const [error, setError] = useState(null);

    const TEXT = {
        successTitle: 'Thông tin lịch của bệnh nhân',
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
        patientName: 'Tên của bệnh nhân',
        patientPhone: 'Số điện thoại'
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
                setError("Không thể tải thông tin cuộc hẹn, mã QR hoặc server có vấn đề.");
            });
    }, [searchParams]);

    if (error) {
        return (
            <div id="verify-page" className="error-message">
                <ErrorBox error={error} setError={setError} />
            </div>
        );
    }


    return (
        <div id='verify-page'>
            {result ? (
                <div className='finish'>
                    <div className='header'>
                        <div className='header-title'>
                            <Icon src={PersonIcon} alt='person-icon' />
                            {TEXT.successTitle}
                        </div>

                        <div className='logo'>
                            <img src={LogoPicture} alt='logo-picture' />
                            <img src={LogoText} alt='logo-text' />
                        </div>
                    </div>

                    <div className='body'>
                        <div className='finish-detail'>
                            <div className='doctor-name'>
                                <div className='title'>{TEXT.doctor}</div>
                                <div className='detail'>{TEXT.doctorPrefix} {result.doctorName}</div>
                            </div>

                            <div className='patient-name'>
                                <div className='title'>{TEXT.patientName}</div>
                                <div className='detail'>{result.patientName}</div>
                            </div>

                            <div className='patient-phone'>
                                <div className='title'>{TEXT.patientPhone}</div>
                                <div className='detail'>{result.patientPhone}</div>
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
                        </div>

                        <div className='queue-number'>
                            <div className='detail'>
                                {result.queueNumber}
                            </div>
                            <div className='title'>
                                {TEXT.queue}
                            </div>
                        </div>
                    </div>
                </div>
            ) : (
                <SkeletonUI />
            )}
        </div>
    );
}

export default VerifyPage;