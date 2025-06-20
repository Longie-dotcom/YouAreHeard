// Modules
import { useState, useEffect, useRef } from 'react';

// Styling sheet
import './TreatmentBox.css';

// Assets
import ZoomIcon from '../../uploads/icon/zoom.png';
import BigPillIcon from '../../uploads/icon/big-pill.png';

// Components
import Icon from '../Icon/Icon';
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import ConfirmTreatmentPlan from '../ConfirmTreatmentPlan/ConfirmTreatmentPlan';

// Hooks
import useLoadAllRegimens from '../../hook/useLoadAllRegimens';
import useLoadAllPatientGroups from '../../hook/useLoadAllPatientGroups';
import useLoadAllMedications from '../../hook/useLoadAllMedications';

function TreatmentBox({ appointments }) {
    const t1 = 'Danh sách bệnh nhân';
    const t2 = 'Kết quả xét nghiệm gần nhất của bệnh nhân';
    const t3 = 'Tạo kế hoạch điều trị';
    const t4 = 'Lựa chọn phác đồ';
    const t5 = 'Tùy chỉnh phác đồ';
    const t6 = 'Loại phác đồ';
    const t7 = 'Thời gian sử dụng';
    const t8 = 'Liều lượng';
    const t9 = 'Tần suất';
    const t10 = 'Chỉ định';
    const t11 = 'Chống chỉ định';
    const t12 = 'Tác dụng phụ';
    const t14m1 = 'Thuốc';
    const t14 = 'Nhóm bệnh nhân';
    const t15 = 'Chọn nhóm bệnh nhân';
    const t16 = 'Tùy chỉnh thông tin phác đồ';
    const t17 = 'Chọn loại phác đồ';
    const t18 = 'Liều lượng điều chỉnh';
    const t19 = '/1 lần uống';
    const t20 = 'Chọn thuốc thay thế (sẵn có)';
    const t21 = 'Tạo kế hoạch điều trị';
    const t22 = '-';
    const t23 = 'Trực tuyến';
    const t24 = 'Tạo kế hoạch';
    const t25 = 'Số thứ tự: ';
    const t26 = 'Ghi chú';
    const t27 = 'Các loại thuốc thay thế đã chọn (Click vào thuốc đã chọn bên dưới để loại bỏ)';
    const t28 = 'Lịch nhắc nhở uống thuốc (trong 1 ngày)';
    const t29 = 'lần/ngày';
    const t31 = 'Thêm giờ';
    const t32 = 'Xóa lịch';
    const t33 = 'Thời gian';
    const t34 = 'Liều lượng';

    const [error, setError] = useState();
    const [loading, setLoading] = useState();
    const textareaRef = useRef(null);

    const [upcomingAppointments, setUpcomingAppointments] = useState(null);
    const [selectedTreatmentOf, setSelectedTreatmentOF] = useState(null);

    const [isCustomizing, setIsCustomizing] = useState(false);
    const [isSubmit, setIsSubmit] = useState(false);

    const [selectedRegimen, setSelectedRegimen] = useState(null);
    const [selectedMedications, setSelectedMedications] = useState(null);
    const [notes, setNotes] = useState('');
    const [patientGroupID, setPatientGroupID] = useState(null);

    const {
        medications
    } = useLoadAllMedications({ setError, setLoading });
    const {
        regimens
    } = useLoadAllRegimens({ setError, setLoading });
    const {
        patientGroups
    } = useLoadAllPatientGroups({ setError, setLoading });


    const formatTime = (timeStr) => {
        const [hours, minutes] = timeStr.split(':');
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}`;
    };

    useEffect(() => {
        if (!selectedRegimen) return;

        const updatedMeds = selectedRegimen.medications.map(med => ({
            ...med,
            remindTimes: Array(med.frequency).fill(null).map(() => ({
                time: '',
                dosage: med.dosage || 1,
                medicationName: med.medicationName
            }))
        }));

        setSelectedMedications(updatedMeds);
    }, [selectedRegimen, isCustomizing]);


    useEffect(() => {
        if (!appointments) return;

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const upcomingAppointments = appointments
            .map(app => {
                const dateStr = app.scheduleDate.split('T')[0];
                const dateTime = new Date(`${dateStr}T${app.startTime}`);
                return {
                    ...app,
                    dateTime,
                };
            })
            .filter(app => {
                const appointmentDate = new Date(app.dateTime);
                appointmentDate.setHours(0, 0, 0, 0);
                return appointmentDate >= today && !app.zoomLink;
            });

        setUpcomingAppointments(upcomingAppointments || []);
    }, [appointments]);

    useEffect(() => {
        setSelectedRegimen(null);
        setSelectedMedications(null);
        setNotes('');
        setPatientGroupID(null);
    }, [selectedTreatmentOf])

    return (
        <div className='treatment-box'>
            <div className='title'>
                {t1}
            </div>

            {upcomingAppointments && (
                <div className='patient-list'>
                    {upcomingAppointments.map((appointment) => (
                        <div className='appointment-later'>
                            <div className='name'>
                                {appointment.patientName}
                            </div>
                            <div className='date'>
                                {appointment.scheduleDate?.split("T")[0]}
                            </div>
                            <div className='time'>
                                {formatTime(appointment.startTime)}{t22}{formatTime(appointment.endTime)}
                            </div>
                            <div className='location'>
                                {appointment.zoomLink ? (
                                    <div>
                                        <Icon src={ZoomIcon} alt={'zoom-icon'} /> {t23}
                                    </div>
                                ) : appointment.location}
                            </div>
                            <div className='queue-number'>
                                {t25}{appointment.queueNumber}
                            </div>
                            <div className='patient-detail'>
                                <button
                                    onClick={() => setSelectedTreatmentOF(appointment)}
                                >
                                    {t24}
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {selectedTreatmentOf && (
                <>
                    <div className='title'>
                        {t2}
                    </div>
                    <div className='test-result'>
                        Kết quả xết nghiệm của {selectedTreatmentOf.patientName} sẽ nằm ở đây
                    </div>

                    <div className='title'>
                        {t3}
                    </div>
                    <div className='treatment-form'>
                        <div className='header'>
                            <div className='regimen'>
                                <label htmlFor="regimen-select">{t4}</label>
                                <select
                                    id="regimen-select"
                                    value={selectedRegimen?.regimenID || ''}
                                    onChange={(e) => {
                                        const selectedID = parseInt(e.target.value);
                                        const regimen = regimens.find(r => r.regimenID === selectedID);
                                        setSelectedRegimen(regimen);
                                        setSelectedMedications(regimen?.medications || []);
                                    }}
                                >
                                    <option value="" className='empty'>{t17}</option>
                                    {regimens?.map(regimen => (
                                        <option key={regimen.regimenID} value={regimen.regimenID}>
                                            {regimen.name}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            {selectedRegimen && (
                                <div className='customize-toggle'>
                                    <input
                                        type="checkbox"
                                        checked={isCustomizing}
                                        onChange={(e) => setIsCustomizing(e.target.checked)}
                                    />
                                    <div>{t5}</div>
                                </div>
                            )}
                        </div>

                        <div className='body'>
                            {selectedRegimen && (() => {
                                return (
                                    <>
                                        <div className="regimen-detail">
                                            <div>
                                                <div className='title'>
                                                    {t6}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.type}
                                                </div>
                                            </div>

                                            <div>
                                                <div className='title'>
                                                    {t7}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.duration}
                                                </div>
                                            </div>

                                            <div>
                                                <div className='title'>
                                                    {t10}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.regimenIndications}
                                                </div>
                                            </div>

                                            <div>
                                                <div className='title'>
                                                    {t11}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.regimenContraindications}
                                                </div>
                                            </div>

                                            <div>
                                                <div className='title'>
                                                    {t12}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.regimenSideEffects}
                                                </div>
                                            </div>

                                            <div>
                                                <div className='title'>
                                                    {t14m1}
                                                </div>
                                                <div className='detail'>
                                                    {selectedRegimen.medications?.map(m => m.medicationName).join(', ')}
                                                </div>
                                            </div>
                                        </div>

                                        <div className='patient-group'>
                                            <div className='title'>
                                                {t14}
                                            </div>
                                            <div className='detail'>
                                                <select
                                                    name="patientGroupID"
                                                    value={patientGroupID || ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value === 'null' ? null : parseInt(e.target.value, 10);
                                                        setPatientGroupID(value);
                                                    }}
                                                >
                                                    <option className='empty' value='null'>{t15}</option>
                                                    {patientGroups?.map(group => (
                                                        <option key={group.patientGroupID} value={group.patientGroupID}>
                                                            {group.patientGroupName}
                                                        </option>
                                                    ))}
                                                </select>
                                            </div>
                                        </div>

                                        {isCustomizing && (
                                            <div className='customized-fields'>
                                                <div className='title'>
                                                    {t16}
                                                </div>

                                                <div className='input-group'>
                                                    <label>{t20}</label>
                                                    <div className="medication-list">
                                                        {medications.map(med => {
                                                            const isSelected = selectedMedications.some(m => m.medicationID === med.medicationID);

                                                            return (
                                                                <button
                                                                    key={med.medicationID}
                                                                    type="button"
                                                                    className={`med-pill ${isSelected ? 'selected' : ''}`}
                                                                    onClick={() => {
                                                                        setSelectedMedications(prev => {
                                                                            if (isSelected) {
                                                                                return prev.filter(m => m.medicationID !== med.medicationID);
                                                                            } else {
                                                                                return [...prev, med];
                                                                            }
                                                                        });
                                                                    }}
                                                                >
                                                                    <Icon src={BigPillIcon} alt="big-pill-icon" />
                                                                    {med.medicationName}
                                                                </button>
                                                            );
                                                        })}
                                                    </div>

                                                    {selectedMedications.length > 0 && (
                                                        <>
                                                            <div className='medication-title'>{t27}</div>
                                                            <div className="selected-meds-list">
                                                                {selectedMedications.map(med => (
                                                                    <button
                                                                        key={med.medicationID}
                                                                        type="button"
                                                                        onClick={() =>
                                                                            setSelectedMedications(prev =>
                                                                                prev.filter(m => m.medicationID !== med.medicationID)
                                                                            )
                                                                        }
                                                                    >
                                                                        {med.medicationName}
                                                                    </button>
                                                                ))}
                                                            </div>
                                                        </>
                                                    )}
                                                </div>
                                            </div>
                                        )}

                                        <div className='pill-remind'>
                                            <div className='header'>{t28}</div>
                                            {selectedMedications?.map((m, medIdx) => (
                                                <div className='pill' key={m.medicationID}>
                                                    <div className='pill-name'>
                                                        <div>
                                                            <div className='pill-detail'>
                                                                {m.medicationName}
                                                            </div>
                                                            <div className='pill-frequency'>
                                                                {(m.remindTimes?.length || 0)}&nbsp;{t29}
                                                            </div>
                                                        </div>
                                                        <button
                                                            type="button"
                                                            className="add-time-button"
                                                            onClick={() => {
                                                                const newMeds = [...selectedMedications];
                                                                newMeds[medIdx].remindTimes = [
                                                                    ...(newMeds[medIdx].remindTimes || []),
                                                                    { time: '', dosage: newMeds[medIdx].dosage || 1, medicationName: m.medicationName }
                                                                ];
                                                                setSelectedMedications(newMeds);
                                                            }}
                                                        >
                                                            {t31}
                                                        </button>
                                                    </div>
                                                    <div className='drink-time'>
                                                        {(m.remindTimes || []).map((time, idx) => (
                                                            <div key={idx} className="pill-time-slot">
                                                                <div>
                                                                    <div className='pill-title'>
                                                                        {t33}
                                                                    </div>
                                                                    <input
                                                                        className='time-input'
                                                                        type='time'
                                                                        value={time.time}
                                                                        onChange={(e) => {
                                                                            const newMeds = [...selectedMedications];
                                                                            newMeds[medIdx].remindTimes[idx].time = e.target.value;
                                                                            setSelectedMedications(newMeds);
                                                                        }}
                                                                    />
                                                                </div>
                                                                <div>
                                                                    <div className='pill-title'>
                                                                        {t34}
                                                                    </div>
                                                                    <input
                                                                        className='dosage-input'
                                                                        type='number'
                                                                        min={1}
                                                                        defaultValue={1}
                                                                        value={time.dosage}
                                                                        onChange={(e) => {
                                                                            const newMeds = [...selectedMedications];
                                                                            newMeds[medIdx].remindTimes[idx].dosage = parseInt(e.target.value) || 1;
                                                                            setSelectedMedications(newMeds);
                                                                        }}
                                                                    />
                                                                </div>

                                                                <button
                                                                    type="button"
                                                                    onClick={() => {
                                                                        const newMeds = [...selectedMedications];
                                                                        newMeds[medIdx].remindTimes.splice(idx, 1);
                                                                        setSelectedMedications(newMeds);
                                                                    }}
                                                                >
                                                                    {t32}
                                                                </button>
                                                            </div>
                                                        ))}
                                                    </div>
                                                </div>
                                            ))}
                                        </div>

                                        <div className='note-form'>
                                            <div className='title'>
                                                {t26}
                                            </div>
                                            <div className='detail'>
                                                <textarea
                                                    ref={textareaRef}
                                                    value={notes}
                                                    onInput={(e) => {
                                                        const textarea = textareaRef.current;
                                                        textarea.style.height = 'auto';
                                                        textarea.style.height = `${textarea.scrollHeight}px`;
                                                        setNotes(e.target.value);
                                                    }}
                                                />
                                            </div>
                                        </div>

                                        <button
                                            className='submit'
                                            onClick={() => setIsSubmit(true)}
                                        >
                                            {t21}
                                        </button>
                                    </>
                                );
                            })()}
                        </div>

                    </div>
                </>
            )}


            {isSubmit && (
                <ConfirmTreatmentPlan
                    confirmTreatmentPlan={{
                        pillRemindTime: selectedMedications, treatmentDetail: {
                            patientID: selectedTreatmentOf.patientID,
                            doctorID: selectedTreatmentOf.doctorID,
                            patientGroupID: patientGroupID,
                            regimenID: selectedRegimen.regimenID,
                            date: new Date().toISOString(),
                            notes: notes
                        }
                    }} setIsSubmit={setIsSubmit}
                    setError={setError}
                    setLoading={setLoading}
                    selectedRegimen={selectedRegimen}
                    isAdjusted={isCustomizing}
                />
            )}

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}
        </div>
    )
}

export default TreatmentBox;