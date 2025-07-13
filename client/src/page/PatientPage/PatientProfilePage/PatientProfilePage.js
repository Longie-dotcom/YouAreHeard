// Modules
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

// Style sheet
import './PatientProfilePage.css';

// Assets
import LogoPicture from '../../../uploads/logo-picture.png';

// Components
import ErrorBox from '../../../component/ErrorBox/ErrorBox';
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import TextBox from '../../../component/TextBox/TextBox';

// Hooks
import useLoadPregnancyStatus from '../../../hook/useLoadPregnancyStatus';
import useLoadConditions from '../../../hook/useLoadConditions';
import useCreatePatientProfile from '../../../hook/useCreatePatientProfile';
import useGetPatientProfile from '../../../hook/useGetPatientProfile';

function PatientProfilePage({ user }) {

    // Texts
    const t1 = 'Cập nhật hồ sơ'
    const t2 = 'Cập nhật hồ sơ';
    const t3 = 'để sử dụng nhiều dịch vụ hơn';
    const t4 = null;
    const t5 = 'Tình trạng thai kỳ';
    const t6 = 'Điều kiện bệnh lý';
    const t7 = 'Chiều cao (cm)';
    const t8 = 'Cân nặng (kg)';
    const t9 = 'Giới tính';
    const t10 = 'Họ và tên';
    const t11 = 'Ngày sinh';
    const t12 = 'Cập nhật thành công';
    const t14m1 = 'Email đăng ký';
    const t14 = 'Thông tin về danh tính';
    const t15 = 'Tôi đồng ý với'
    const t16 = 'điều khoản sử dụng';
    const t17 = 'và';
    const t18 = 'chính sách bảo mật';
    const t19 = 'Cập nhật hồ sơ';
    const t20 = 'Thông tin này sẽ được';
    const t21 = 'Thông tin này hỗ trợ bác sĩ tạo kế hoạch điều trị cho người dùng hiệu quả hơn, đây là';
    const t22 = null;
    const t23 = 'ẩn danh';
    const t24 = 'khi người dùng mong muốn được ẩn danh trong quá trình';
    const t25 = 'tư vấn trực tuyến';
    const t26 = 'Thông tin chung';
    const t27 = 'thông tin bắt buộc';
    const t28 = 'Thông tin hỗ trợ';
    const t29 = 'thông tin không bắt buộc';

    // States
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(null);

    const [pregnancyStatusID, setPregnancyStatusID] = useState(process.env.REACT_APP_NON_PREGNANCY_STATUS);
    const [height, setHeight] = useState('');
    const [weight, setWeight] = useState('');
    const [gender, setGender] = useState('');
    const [acceptPolicy, setAcceptPolicy] = useState(false);
    const [selectedConditions, setSelectedConditions] = useState([]);

    const [finish, setFinish] = useState(null);

    const navigate = useNavigate();
    
    const {
        pregnancyStatus
    } = useLoadPregnancyStatus({ setError, setLoading });
    const {
        conditions
    } = useLoadConditions({ setError, setLoading });
    const {
        handleSubmit
    } = useCreatePatientProfile({ setError, setLoading, acceptPolicy, setFinish });
    const {
        profile
    } = useGetPatientProfile({ setError, setLoading, userId: user?.UserId });

    useEffect(() => {
        if (profile) {
            setPregnancyStatusID(profile.pregnancyStatusID || process.env.REACT_APP_NON_PREGNANCY_STATUS);
            setHeight(profile.height || '');
            setWeight(profile.weight || '');
            setGender(profile.gender || '');
            setSelectedConditions(profile.conditions?.map(c => c.conditionID) || []);
        }
    }, [profile]);

    return (
        <div id='patient-profile-page'>
            <div className='header'>
                <div
                    onClick={() => navigate('/homePage')}
                    className='logo'
                >
                    <img src={LogoPicture} />
                </div>
                <div className='title'>
                    <h1>
                        {t1}
                    </h1>
                    <p>
                        <span className='normal'>
                            {t2}
                        </span>
                        &nbsp;
                        <span className='login'>
                            {t3}
                        </span>
                    </p>
                </div>
            </div>

            <div className='body'>
                <div className='section'>
                    <div className='title'>
                        <div className='main-title'>
                            {t14}
                        </div>
                        <div className='note'>
                            {t20}&nbsp;<span className='hightlight'>{t23}</span>&nbsp;{t24}&nbsp;<span className='hightlight'>{t25}</span>
                        </div>
                    </div>
                    <div className='input-group'>
                        <label htmlFor="userName">{t10}</label>
                        <input
                            type="text"
                            id="userName"
                            value={user?.Name}
                            readOnly
                        />
                    </div>

                    <div className='input-group'>
                        <label htmlFor="dob">{t11}</label>
                        <input
                            type="date"
                            id="dob"
                            value={user?.Dob?.substring(0, 10) || ''}
                            readOnly
                        />
                    </div>

                    <div className='input-group'>
                        <label htmlFor="dob">{t14m1}</label>
                        <input
                            type="text"
                            id="dob"
                            value={user?.Email}
                            readOnly
                        />
                    </div>
                </div>

                <div className='section'>
                    <div className='title'>
                        <div className='main-title'>
                            {t26}
                        </div>
                        <div className='note'>
                            {t21}&nbsp;<span className='hightlight'>{t27}</span>
                        </div>
                    </div>

                    {gender === 'Nữ' && (
                        <div className='input-group'>
                            <label>{t5}</label>
                            <select value={pregnancyStatusID} onChange={(e) => setPregnancyStatusID(parseInt(e.target.value))}>
                                <option value="">Chọn tình trạng</option>
                                {pregnancyStatus?.map(status => (
                                    <option key={status.pregnancyStatusID} value={status.pregnancyStatusID}>
                                        {status.pregnancyStatusName}
                                    </option>
                                ))}
                            </select>
                        </div>
                    )}

                    <div className='input-group'>
                        <label>{t7}</label>
                        <input
                            type="number"
                            value={height}
                            min={40}
                            onChange={(e) => setHeight(e.target.value)}
                            placeholder="170.5"
                        />
                    </div>

                    <div className='input-group'>
                        <label>{t8}</label>
                        <input
                            min={5}
                            type="number"
                            value={weight}
                            onChange={(e) => setWeight(e.target.value)}
                            placeholder="60.5"
                        />
                    </div>

                    <div className='input-group'>
                        <label>{t9}</label>
                        <select value={gender} onChange={(e) => setGender(e.target.value)}>
                            <option value="">Chọn giới tính</option>
                            <option value="Nam">Nam</option>
                            <option value="Nữ">Nữ</option>
                            <option value="Khác">Khác</option>
                        </select>
                    </div>
                </div>

                <div className='section'>
                    <div className='title'>
                        <div className='main-title'>
                            {t28}
                        </div>
                        <div className='note'>
                            {t21}&nbsp;<span className='hightlight'>{t29}</span>
                        </div>
                    </div>

                    <div className='input-group'>
                        <label>{t6}</label>
                        <div className='checkbox-group'>
                            {conditions?.map((condition) => {
                                const isSelected = selectedConditions.includes(condition.conditionID);
                                return (
                                    <div
                                        key={condition.conditionID}
                                        className={`checkbox-item condition-box ${isSelected ? 'selected' : ''}`}
                                        onClick={() => {
                                            const value = condition.conditionID;
                                            setSelectedConditions(prev =>
                                                prev.includes(value)
                                                    ? prev.filter(id => id !== value)
                                                    : [...prev, value]
                                            );
                                        }}
                                    >
                                        {condition.conditionName}
                                    </div>
                                );
                            })}
                        </div>
                    </div>
                </div>
            </div>

            <div className='footer'>
                <div className='policy'>
                    <input type='checkbox' onChange={(e) => setAcceptPolicy(e.target.checked)} />
                    <div className='policy-detail'>
                        &nbsp;{t15}&nbsp;
                        <span className='usability'>
                            {t16}
                        </span>
                        &nbsp;{t17}&nbsp;
                        <span className='security'>
                            {t18}
                        </span>
                    </div>
                </div>
                <div className='buttons'>
                    <button onClick={() => handleSubmit({
                        userID: user?.UserId,
                        pregnancyStatusID,
                        height,
                        weight,
                        gender,
                        conditions: conditions
                            .filter(condition => selectedConditions.includes(condition.conditionID))
                            .map(condition => ({
                                conditionID: condition.conditionID,
                                conditionName: condition.conditionName
                            }))
                    })} className='submit-button' >
                        {t19}
                    </button>
                </div>
            </div>

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}

            {loading && (
                <SkeletonUI />
            )}

            {finish && (
                <TextBox setText={setFinish} text={finish} title={t12} />
            )}
        </div>
    )
}

export default PatientProfilePage;