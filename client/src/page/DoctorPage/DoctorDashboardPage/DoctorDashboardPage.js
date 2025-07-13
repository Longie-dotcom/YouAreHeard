// Modules
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
import SideMenu from '../../../component/SideMenu/SideMenu';
import NextAppointment from '../../../component/NextAppointment/NextAppointment';
import TreatmentBox from '../../../component/TreatmentBox/TreatmentBox';
import TestLabBox from '../../../component/TestLabBox/TestLabBox';
import PatientMedicalHistory from '../../../component/PatientMedicalHistory/PatientMedicalHistory';

// Hooks
import useLogout from '../../../hook/useLogout';
import useLoadDoctorAppointments from '../../../hook/useLoadDoctorAppointments';

function DoctorDashboardPage({ user, setReloadCookies }) {
    const t1 = 'Đăng xuất';
    const t2 = '⬆ Lên đầu';
    const t3 = 'Hiện bác sĩ chưa có lịch khám/điều trị';
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
            <button
                className="scroll-to-top"
                onClick={() => window.scrollTo({ top: 0, behavior: 'smooth' })}
            >
                {t2}
            </button>

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
                        <NextAppointment user={user} appointments={appointments} emptyText={t3} />
                    )}

                    {openSection === 'treatment' && (
                        <TreatmentBox appointments={appointments} user={user} />
                    )}

                    {openSection === 'test' && (
                        <TestLabBox appointments={appointments} />
                    )}
                    {openSection === 'history' && (
                        <PatientMedicalHistory appointments={appointments} user={user} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default DoctorDashboardPage;