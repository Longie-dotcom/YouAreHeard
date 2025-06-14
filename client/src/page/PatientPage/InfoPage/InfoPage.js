// Modules
import { useNavigate } from 'react-router-dom';
import { useRef, useEffect } from 'react';

// Style sheet
import './InfoPage.css';

// Assets

// Components
import Icon from '../../../component/Icon/Icon';
import TabBar from '../../../component/TabBar/TabBar';
import AppointmentList from '../../../component/AppointmentList/AppointmentList';

// Hooks
import { useState } from 'react';

function InfoPage({ user }) {
    const t1 = 'Lịch hẹn sắp tới ';
    const t2 = 'Kết quả xét nghiệm';
    const t3 = 'Phác đồ điều trị';
    const t4 = 'Thông tin của người dùng';
    const t5 = 'Trang thông tin thể hiện các lịch hẹn sắp tới, kết quả xét nghiệm và phác đồ điều trị hiện tại của người dùng trong hiện tại';

    const [openAppointment, setOpenAppointment] = useState(true);
    const [openTestResult, setOpenTestResult] = useState(false);
    const [openTreatment, setOpenTreatment] = useState(false);

    const handleOpenAppointment = () => {
        setOpenTestResult(false);
        setOpenTreatment(false);
        setOpenAppointment(true);
    }

    const handleOpenTestResult = () => {
        setOpenAppointment(false);
        setOpenTreatment(false);
        setOpenTestResult(true);
    }

    const handleOpenTreatment = () => {
        setOpenAppointment(false);
        setOpenTestResult(false);
        setOpenTreatment(true);
    }

    const tabs = [
        { name: t1, action: handleOpenAppointment },
        { name: t2, action: handleOpenTestResult },
        { name: t3, action: handleOpenTreatment }
    ];

    return (
        <div id='info-page'>
            <div className='header'>
                <h1>
                    {t4}
                </h1>
                <p>
                    {t5}
                </p>
            </div>

            <TabBar tabs={tabs} />

            {openAppointment && (
                <AppointmentList user={user} />
            )}
        </div>
    )
}

export default InfoPage;