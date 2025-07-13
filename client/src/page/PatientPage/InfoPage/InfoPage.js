// Modules

// Style sheet
import './InfoPage.css';

// Assets

// Components
import TabMenu from '../../../component/TabMenu/TabMenu';
import AppointmentList from '../../../component/AppointmentList/AppointmentList';
import TreatmentPlanInfoBox from '../../../component/TreatmentPlanInfoBox/TreatmentPlanInfoBox';
import TestResultInfoBox from '../../../component/TestResultInfoBox/TestResultInfoBox';

// Hooks
import { useState } from 'react';

function InfoPage({ user }) {
    const t1 = 'Lịch hẹn sắp tới ';
    const t2 = 'Kết quả xét nghiệm';
    const t3 = 'Phác đồ điều trị';
    const t4 = 'Thông tin của người dùng';
    const t5 = 'Trang thông tin thể hiện các lịch hẹn sắp tới, kết quả xét nghiệm và phác đồ điều trị hiện tại của người dùng trong hiện tại';

    const [activeTab, setActiveTab] = useState('appointment');

    const handleOpenAppointment = () => setActiveTab('appointment');
    const handleOpenTestResult = () => setActiveTab('test');
    const handleOpenTreatment = () => setActiveTab('treatment');

    const tabs = [
        { id: 'appointment', label: t1, action: handleOpenAppointment },
        { id: 'test', label: t2, action: handleOpenTestResult },
        { id: 'treatment', label: t3, action: handleOpenTreatment }
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

            <TabMenu
                tabs={tabs}
                setActiveTabId={setActiveTab}
                activeTabId={activeTab}
            />

            {activeTab === 'appointment' && <AppointmentList user={user} />}
            {activeTab === 'treatment' && <TreatmentPlanInfoBox user={user} />}
            {activeTab === 'test' && <TestResultInfoBox user={user} />}
        </div>
    )
}

export default InfoPage;