// Modules

// Style sheet
import './AppointmentPage.css';

// Assets

// Components
import Icon from '../../../component/Icon/Icon';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import TabBar from '../../../component/TabBar/TabBar';
import DoctorList from '../../../component/DoctorList/DoctorList';
import CalendarSelection from '../../../component/CalendarSelection/CalendarSelection';

// Hooks
import { useState } from 'react';

function AppointmentPage({ user }) {
    const t1 = 'Đặt lịch hẹn';
    const t2 = 'Chọn bác sĩ và thời gian phù hợp cho cuộc hẹn của bạn';
    const t3 = 'Chọn theo bác sĩ';
    const t4 = 'Chọn theo ngày';

    const [chooseByDoctor, setChooseByDoctor] = useState(true);
    const [chooseByDate, setChooseByDate] = useState(false);
    const [choosenDoctor, setChoosenDoctor] = useState(null);

    const handleOpenChooseByDoctor = () => {
        setChooseByDate(false);
        setChooseByDoctor(true);
    }

    const handleOpenChooseByDate = () => {
        setChooseByDoctor(false);
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
            <TabBar tabs={tabs} />

            {chooseByDoctor && (
                <>
                    <DoctorList setChoosenDoctor={setChoosenDoctor} />

                    {choosenDoctor && (
                        <CalendarSelection doctor={choosenDoctor} />
                    )}
                </>
            )}
        </div>
    )
}

export default AppointmentPage;