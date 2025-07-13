// Modules

// Styling sheet
import './PatientProfileInfoBox.css';

// Assets
import BigPillIcon from '../../uploads/icon/big-pill.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useGetPatientProfile from '../../hook/useGetPatientProfile';

function PatientProfileInfoBox({ selectedTreatmentOf, setError, setLoading }) {
    const t18 = 'Bệnh nhân chưa tạo hồ sơ';
    const patientName = 'Họ và Tên';
    const ageLabel = 'Tuổi';
    const hivLabel = 'Tình trạng HIV';
    const pregnancyLabel = 'Tình trạng thai sản';
    const genderLabel = 'Giới tính';
    const heightLabel = 'Chiều cao';
    const weightLabel = 'Cân nặng';
    const conditionLabel = 'Bệnh nền';
    const title = 'Thông tin chung';

    const {
        profile
    } = useGetPatientProfile({
        userId: selectedTreatmentOf?.patientID, setError, setLoading
    })

    const getAge = (dobString) => {
        if (!dobString) return null;
        const dob = new Date(dobString);
        const today = new Date();
        let age = today.getFullYear() - dob.getFullYear();
        const m = today.getMonth() - dob.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
            age--;
        }
        return age;
    };

    return (
        <div className='patient-profile-info-box'>
            {selectedTreatmentOf && (
                <div className='patient-profile-info'>
                    {profile ? (
                        <>
                            <div className='patient-box'>
                                <div className='title'>{title}</div>
                                <div className='box'>
                                    <div className='title'>{patientName}</div>
                                    <div className='info'>{profile.patientName}</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{ageLabel}</div>
                                    <div className='info'>{getAge(profile.patientDob)}</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{hivLabel}</div>
                                    <div className='info'>{profile.hivStatusName}</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{pregnancyLabel}</div>
                                    <div className='info'>{profile.pregnancyStatusName}</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{genderLabel}</div>
                                    <div className='info'>{profile.gender}</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{heightLabel}</div>
                                    <div className='info'>{profile.height} cm</div>
                                </div>
                                <div className='box'>
                                    <div className='title'>{weightLabel}</div>
                                    <div className='info'>{profile.weight} kg</div>
                                </div>
                            </div>

                            <div className='patient-box'>
                                <div className='title'>{conditionLabel}</div>
                                {profile.conditions?.length > 0 ? (
                                    <div className='condition-list'>
                                        {profile.conditions.map((c, idx) => (
                                            <div className='box' key={idx}>
                                                <Icon src={BigPillIcon} alt='condition-icon' />
                                                <div className='info'>{c.conditionName}</div>
                                            </div>
                                        ))}
                                    </div>
                                ) : (
                                    <div className='info'>Không có</div>
                                )}
                            </div>
                        </>
                    ) : (
                        <div className='empty'>
                            {t18}
                        </div>
                    )}
                </div>
            )}
        </div>
    )
}

export default PatientProfileInfoBox;