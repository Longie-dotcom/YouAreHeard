// Modules
import { useNavigate } from 'react-router-dom';
import { useRef, useEffect } from 'react';
import { useState } from 'react';

// Style sheet
import './DoctorDashboardPage.css';

// Assets
import PillIcon from '../../../uploads/icon/pill.png';
import TreatmentIcon from '../../../uploads/icon/treatment.png';
import TestIcon from '../../../uploads/icon/test.png';
import HospitalIcon from '../../../uploads/icon/hospital.png';
import LogoText from '../../../uploads/logo-text.png';
import LogoPicture from '../../../uploads/logo-picture.png';

// Components
import Icon from '../../../component/Icon/Icon';
import SideMenu from '../../../component/SideMenu/SideMenu';
import DoctorAppointment from '../../../component/DoctorAppointment/DoctorAppointment';
import TreatmentBox from '../../../component/TreatmentBox/TreatmentBox';

// Hooks
import useLogout from '../../../hook/useLogout';
import useLoadDoctorAppointments from '../../../hook/useLoadDoctorAppointments';

function DoctorDashboardPage({ user, setReloadCookies }) {
    const t1 = 'Đăng xuất';
    const menuItems = [
        { id: 'appointment', label: 'Các cuộc hẹn', icon: HospitalIcon },
        { id: 'history', label: 'Lịch sử khám bệnh', icon: TreatmentIcon },
        { id: 'treatment', label: 'Kế hoạch điều trị', icon: PillIcon },
        { id: 'test', label: 'Xét nghiệm', icon: TestIcon },
    ];

    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const [openSection, setOpenSection] = useState('appointment');

    const { appointments } = useLoadDoctorAppointments({ doctorId: user?.UserId, setError, setLoading });
    const { logout } = useLogout({ setReloadCookies, setError, setLoading });
    return (
        <div id='doctor-dashboard-page'>
            <div className='header'>
                <div className='logo'>
                    <img src={LogoPicture} alt='logo' />
                    <img src={LogoText} alt='logo' />
                </div>

                <button
                    onClick={() => logout()}
                >
                    {t1}
                </button>
            </div>

            <div className='body'>
                <div className='menu'>
                    <SideMenu
                        menuItems={menuItems}
                        openSection={openSection}
                        setOpenSection={setOpenSection}
                        appointments={appointments}
                    />
                </div>

                <div className='main'>
                    {openSection === 'appointment' && (
                        <DoctorAppointment user={user} appointments={appointments} />
                    )}

                    {openSection === 'treatment' && (
                        <TreatmentBox appointments={appointments} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default DoctorDashboardPage;