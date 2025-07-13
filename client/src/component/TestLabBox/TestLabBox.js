// Modules
import { useEffect, useState, useRef } from 'react';

// Styling sheet
import './TestLabBox.css';

// Assets
import ZoomIcon from '../../uploads/icon/zoom.png';
import ConfirmIcon from '../../uploads/icon/confirm.png';

// Components
import Icon from '../Icon/Icon';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import ErrorBox from '../ErrorBox/ErrorBox';
import ConfirmTestLabBox from '../ConfirmTestLabBox/ConfirmTestLabBox';
import PatientProfileInfoBox from '../PatientProfileInfoBox/PatientProfileInfoBox';
import TextBox from '../TextBox/TextBox';

// Hooks
import useLoadTestType from '../../hook/useLoadTestType';
import useLoadAllTestMetric from '../../hook/useLoadAllTestMetric';
import useLoadTestStage from '../../hook/useLoadTestStage';
import useLoadHIVStatus from '../../hook/useLoadHIVStatus';
import useUpdatePatientHIVStatus from '../../hook/useUpdatePatientHIVStatus';

function TestLabBox({ appointments }) {
    const t1 = 'Danh sách bệnh nhân';
    const t2 = 'Hồ sơ bệnh nhân';
    const t3 = 'Điền kết quả xét nghiệm';
    const t4 = 'Loại xét nghiệm';
    const t5 = 'Chọn loại xét nghiệm';
    const t6 = 'Điền thông tin xét nghiệm cho loại xét nghiệm';
    const t7 = 'Không có đơn vị';
    const t8 = 'Đơn vị:';
    const t9 = 'Tùy chỉnh mẫu thông tin';
    const t10 = 'Chọn thông số đo thay thế';
    const t11 = 'Xóa';
    const t12 = 'Giai đoạn kiểm tra';
    const t14m1 = 'Xác nhận';
    const t14 = 'Chọn giai đoạn kiểm tra';
    const t15 = 'Ghi chú chung';
    const t16 = 'Xác nhận trạng thái HIV mới';
    const t17 = 'Cập nhật trạng thái HIV mới';

    const t22 = '-';
    const t23 = 'Trực tuyến';
    const t24 = 'Xét nghiệm';
    const t25 = 'Số thứ tự: ';

    const [error, setError] = useState();
    const [loading, setLoading] = useState();

    const textareaRef = useRef(null);
    const [notes, setNotes] = useState('');
    const [labResult, setLabResult] = useState(null);
    const [selectedTestStage, setSelectedTestStage] = useState(null);
    const [selectedTestType, setSelectedTestType] = useState(null);
    const [selectedTestMetrics, setSelectedTestMetrics] = useState([]);
    const [isCustomized, SetIsCustomized] = useState(false);
    const [upcomingAppointments, setUpcomingAppointments] = useState(null);
    const [selectedTestOf, setSelectedTestOf] = useState(null);
    const [hivStatusID, setHivStatusID] = useState('');
    const [finish, setFinish] = useState(null);

    const {
        testTypes
    } = useLoadTestType({ setError, setLoading });
    const {
        testMetric
    } = useLoadAllTestMetric({ setError, setLoading });
    const {
        testStages
    } = useLoadTestStage({ setError, setLoading });
    const {
        updatePatientHIVStatus
    } = useUpdatePatientHIVStatus({ setError, setLoading, setFinish })
    const {
        hivStatus
    } = useLoadHIVStatus({ setError, setLoading });

    const formatTime = (timeStr) => {
        const [hours, minutes] = timeStr.split(':');
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}`;
    };

    const selectTestType = (e) => {
        const selectedId = parseInt(e.target.value);
        const selectedTestType = testTypes.find(testType => testType.testTypeId === selectedId)

        setSelectedTestType(selectedTestType);
        const metricWithValue = selectedTestType?.testMetrics?.map(metric => ({
            ...metric,
            value: ''
        })) || [];
        setSelectedTestMetrics(metricWithValue);
    }

    const addValueForMetric = (index, value) => {
        const update = [...selectedTestMetrics];
        update[index].value = value;
        setSelectedTestMetrics(update);
    }

    useEffect(() => {
        if (!isCustomized && selectedTestType) {
            const metricWithValue = selectedTestType.testMetrics.map(m => ({
                ...m,
                value: ''
            }));
            setSelectedTestMetrics(metricWithValue);
        }
    }, [isCustomized]);

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

    return (
        <div className='test-lab-box'>
            <div className='title'>
                {t1}
            </div>

            {upcomingAppointments && (
                <div className='patient-list'>
                    {upcomingAppointments
                        .filter((appointment) => !appointment.isOnline)
                        .map((appointment, key) => (
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
                                        onClick={() => setSelectedTestOf(appointment)}
                                    >
                                        {t24}
                                    </button>
                                </div>
                            </div>
                        ))}
                </div>
            )}

            {selectedTestOf && (
                <>
                    <div className='title'>
                        {t2}
                    </div>
                    <div className='patient-info'>
                        <PatientProfileInfoBox selectedTreatmentOf={selectedTestOf} setError={setError} setLoading={setLoading} />
                    </div>

                    <div className='title'>
                        {t3}
                    </div>
                    <div className='test-form'>
                        <div className='header'>
                            <div className='type-input'>
                                <div className='type'>
                                    <label htmlFor="test-type-select">{t4}</label>
                                    <select id="test-type-select" onChange={(e) => selectTestType(e)} defaultValue="">
                                        <option value="" disabled>{t5}</option>
                                        {testTypes.map((tt) => (
                                            <option key={tt.testTypeId} value={tt.testTypeId}>
                                                {tt.testTypeName}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                <div className='stage'>
                                    <label htmlFor="test-type-select">{t12}</label>
                                    <select id="test-type-select"
                                        onChange={(e) => {
                                            const selectedId = parseInt(e.target.value);
                                            const stage = testStages.find(ts => ts.testStageId === selectedId);
                                            setSelectedTestStage(stage || null);
                                        }}
                                        defaultValue="">

                                        <option value="" disabled>{t14}</option>
                                        {testStages.map((ts) => (
                                            <option key={ts.testStageId} value={ts.testStageId}>
                                                {ts.testStageName}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            {selectedTestType && (
                                <div className='customized-input'>
                                    <input type='checkbox'
                                        onChange={(e) => SetIsCustomized(e.target.checked)}
                                    />
                                    <label>
                                        {t9}
                                    </label>
                                </div>
                            )}
                        </div>

                        {selectedTestType && selectedTestStage && (
                            <>
                                <div className='body'>
                                    {isCustomized && (
                                        <>
                                            <div className='header'>
                                                {t10}
                                            </div>
                                            <div className='metrics'>
                                                {testMetric.map((metric, key) => {
                                                    const isSelected = selectedTestMetrics.some(m => m.testMetricID === metric.testMetricID);

                                                    return (
                                                        <button
                                                            key={key}
                                                            className={`metric ${isSelected ? 'selected' : ''}`}
                                                            onClick={() =>
                                                                setSelectedTestMetrics(prev => {
                                                                    const exists = prev.some(m => m.testMetricID === metric.testMetricID);
                                                                    if (exists) {
                                                                        return prev.filter(m => m.testMetricID !== metric.testMetricID);
                                                                    } else {
                                                                        return [...prev, { ...metric, value: '' }];
                                                                    }
                                                                })
                                                            }
                                                        >
                                                            {metric.testMetricName}
                                                        </button>
                                                    );
                                                })}
                                            </div>
                                        </>
                                    )}

                                    <div className='header'>
                                        {t6}
                                    </div>
                                    <div className='metric-inputs'>
                                        {selectedTestMetrics.map((metric, index) => (
                                            <div className='metric' key={metric.testMetricID}>
                                                <div className='metric-info'>
                                                    <div className='info'>
                                                        <div className='name'>
                                                            {metric.testMetricName}
                                                        </div>

                                                        <div className='type'>
                                                            {metric.testMetricTypeName}
                                                        </div>
                                                    </div>

                                                    <button
                                                        onClick={() =>
                                                            setSelectedTestMetrics(prev => {
                                                                const exists = prev.some(m => m.testMetricID === metric.testMetricID);
                                                                if (exists) {
                                                                    return prev.filter(m => m.testMetricID !== metric.testMetricID);
                                                                } else {
                                                                    return [...prev, { ...metric, value: '' }];
                                                                }
                                                            })
                                                        }
                                                        className='delete'
                                                    >
                                                        {t11}
                                                    </button>
                                                </div>

                                                <div className='input'>
                                                    <input
                                                        type={metric.testMetricTypeID === 1 ? 'number' : 'text'}
                                                        value={metric.value}
                                                        onChange={(e) => addValueForMetric(index, e.target.value)}
                                                        placeholder={`${metric.testMetricName}`}
                                                    />

                                                    <div className='unit'>
                                                        {t8}&nbsp;{metric.unitName ? metric.unitName : t7}
                                                    </div>
                                                </div>
                                            </div>
                                        ))}
                                    </div>

                                    <div className='header'>
                                        {t17}
                                    </div>
                                    <div className='update-hiv-status'>
                                        <select
                                            value={hivStatusID} onChange={(e) => setHivStatusID(parseInt(e.target.value))}>
                                            <option value="">Chọn trạng thái</option>
                                            {hivStatus?.map(status => (
                                                <option key={status.hivStatusID} value={status.hivStatusID}>
                                                    {status.hivStatusName}
                                                </option>
                                            ))}
                                        </select>

                                        <button
                                            onClick={() => updatePatientHIVStatus({ patientID: selectedTestOf.patientID, hivStatusID })}
                                        >
                                            {t16}
                                        </button>
                                    </div>


                                    <div className='header'>
                                        {t15}
                                    </div>
                                    <div className='note-form'>
                                        <textarea
                                            ref={textareaRef}
                                            value={notes}
                                            maxLength={90}
                                            onInput={(e) => {
                                                const textarea = textareaRef.current;
                                                textarea.style.height = 'auto';
                                                textarea.style.height = `${textarea.scrollHeight}px`;
                                                setNotes(e.target.value);
                                            }}
                                        />
                                    </div>
                                </div>

                                <div className='footer'>
                                    <button
                                        className='submit-button'
                                        onClick={() =>
                                            setLabResult({
                                                testStageId: selectedTestStage.testStageId,
                                                testStageName: selectedTestStage.testStageName,
                                                testTypeId: selectedTestType.testTypeId,
                                                testTypeName: selectedTestType.testTypeName,
                                                patientId: selectedTestOf.patientID,
                                                doctorId: selectedTestOf.doctorID,
                                                note: notes,
                                                isCustomized: isCustomized,
                                                testMetricValues: selectedTestMetrics.filter(m => m.value?.toString().trim() !== '')
                                            })
                                        }
                                    >
                                        {t14m1}
                                    </button>
                                </div>
                            </>
                        )}
                    </div>
                </>
            )}

            {finish && (
                <TextBox setText={setFinish} text={finish} title={<Icon src={ConfirmIcon} alt={'confirm-icon'} />} />
            )}

            {labResult && (
                <ConfirmTestLabBox labResult={labResult} setLabResult={setLabResult} setError={setError} setLoading={setLoading} setFinish={setFinish} />
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

export default TestLabBox;