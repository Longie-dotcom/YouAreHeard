// Modules
import { useEffect, useState } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

// Styling sheet
import './PatientMedicalHistory.css';

// Assets
import UserIcon from '../../uploads/icon/user.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import ZoomIcon from '../../uploads/icon/zoom.png';
import TicketIcon from '../../uploads/icon/ticket.png';

// Components
import Icon from '../Icon/Icon';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import ErrorBox from '../ErrorBox/ErrorBox';
import PatientProfileInfoBox from '../PatientProfileInfoBox/PatientProfileInfoBox';

// Hooks
import useGetPatientTestResult from '../../hook/useGetPatientTestResult';
import useGetAppointmentWithDoctorNote from '../../hook/useGetAppointmentWithDoctorNote';

function PatientMedicalHistory({ appointments, user }) {
    const t1 = 'Danh sách bệnh nhân';
    const t2 = 'Ghi chú của bác sĩ';

    const t22 = '-';
    const t23 = 'Trực tuyến';
    const t24 = 'Xem lịch sử';
    const t25 = 'Số thứ tự: ';

    const t3 = 'Ngày';
    const t4 = 'Thời gian';
    const t5 = 'Địa điểm';
    const t6 = 'Đường dẫn';
    const t7 = '-';
    const t8 = 'Zoom meeting (Online)';
    const t10 = 'Vào phòng Zoom';
    const t11 = 'Ghi chú';
    const t12 = 'Lý do';
    const t14m1 = 'Trạng thái lịch hẹn';
    const t18 = 'Số thứ tự';
    const t19 = 'Người khám bệnh';
    const t20 = 'Ghi chú của bác sĩ';

    const [error, setError] = useState();
    const [loading, setLoading] = useState();

    const [upcomingAppointments, setUpcomingAppointments] = useState(null);
    const [selectedPatient, setSelectedPatient] = useState(null);

    const formatTime = (timeStr) => {
        const [hours, minutes] = timeStr.split(':');
        return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}`;
    };

    const {
        testResults
    } = useGetPatientTestResult({ userId: selectedPatient?.patientID, setError, setLoading });
    const {
        appointmentNote
    } = useGetAppointmentWithDoctorNote({ doctorId: user.UserId, setError, setLoading })

    const getMetricHistory = (testResults) => {
        const historyMap = {};

        testResults.forEach(result => {
            const date = new Date(result.date).toLocaleDateString();
            result.testMetricValues
                .filter(m => m.unitName && !isNaN(parseFloat(m.value))) // only numeric + has unit
                .forEach(m => {
                    if (!historyMap[m.testMetricName]) {
                        historyMap[m.testMetricName] = {
                            unit: m.unitName,
                            data: []
                        };
                    }

                    historyMap[m.testMetricName].data.push({
                        date,
                        value: parseFloat(m.value)
                    });
                });
        });

        return historyMap;
    };

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
        <div className='patient-medical-history'>
            <div className='title'>
                {t2}
            </div>
            {appointmentNote && (
                <div className='notes-list'>
                    {appointmentNote.map((appointment) => (
                        <div className='today'>
                            <div className="patient">
                                <div className="title">
                                    <Icon src={UserIcon} alt={'person-icon'} />
                                    {t19}
                                </div>
                                <div className="detail">
                                    {appointment.patientName}
                                </div>
                            </div>

                            <div className="date">
                                <div className="title">
                                    <Icon src={ScheduleIcon} alt={'person-icon'} />
                                    {t3}
                                </div>
                                <div className="detail">
                                    {appointment.scheduleDate?.split("T")[0]}
                                </div>
                            </div>

                            <div className="time">
                                <div className="title">
                                    <Icon src={TimeIcon} alt={'person-icon'} />
                                    {t4}
                                </div>
                                <div className="detail">
                                    {appointment.startTime}&nbsp;{t7}&nbsp;{appointment.endTime}
                                </div>
                            </div>

                            <div className="queue-number">
                                <div className="title">
                                    <Icon src={TicketIcon} alt={'person-icon'} />
                                    {t18}
                                </div>
                                <div className="detail">
                                    {appointment.queueNumber}
                                </div>
                            </div>

                            {!appointment.zoomLink && (
                                <div className="location">
                                    <div className="title">
                                        <Icon src={LocationIcon} alt={'person-icon'} />
                                        {t5}
                                    </div>
                                    <div className="detail">
                                        {appointment.zoomLink ? t8 : appointment.location}
                                    </div>
                                </div>
                            )}

                            {appointment.zoomLink && (
                                <div className="zoom">
                                    <div className="title">
                                        {t6}
                                    </div>
                                    <div className="detail">
                                        <a
                                            href={appointment.zoomLink} target="_blank" rel="noopener noreferrer"
                                            className='zoom-link'>
                                            <Icon src={ZoomIcon} alt={'zoom-icon'} />
                                            &nbsp;&nbsp;&nbsp;{t10}
                                        </a>
                                    </div>
                                </div>
                            )}

                            {appointment.notes && (
                                <div className="note">
                                    <div className="title">
                                        {t11}
                                    </div>
                                    <div className="detail">
                                        {appointment.notes}
                                    </div>
                                </div>
                            )}

                            {appointment.reason && (
                                <div className="reason">
                                    <div className="title">
                                        {t12}
                                    </div>
                                    <div className="detail">
                                        {appointment.reason}
                                    </div>
                                </div>
                            )}

                            {appointment.appointmentStatusName && (
                                <div className="status">
                                    <div className="title">
                                        {t14m1}
                                    </div>
                                    <div className="detail">
                                        {appointment.appointmentStatusName}
                                    </div>
                                </div>
                            )}

                            <div className="status">
                                <div className="title">
                                    {t20}
                                </div>
                                <div className="detail">
                                    {appointment.doctorNotes}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            <div className='title'>
                {t1}
            </div>

            {upcomingAppointments && (
                <div className='patient-list'>
                    {upcomingAppointments
                        .filter((appointment) => !appointment.isAnonymous)
                        .map((appointment) => (
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
                                        onClick={() => setSelectedPatient(appointment)}
                                    >
                                        {t24}
                                    </button>
                                </div>
                            </div>
                        ))}
                </div>
            )}

            {selectedPatient && (
                <PatientProfileInfoBox
                    selectedTreatmentOf={{ patientID: selectedPatient.patientID }}
                    setError={setError}
                    setLoading={setLoading}
                />
            )}

            {selectedPatient && testResults?.length > 0 && (() => {
                const metricHistory = getMetricHistory(testResults);

                return (
                    <div className='chart-container'>
                        <div className='chart-title'>
                            Biểu đồ theo dõi chỉ số xét nghiệm của {selectedPatient.patientName}
                        </div>

                        <div className='chart-grid'>
                            {Object.entries(metricHistory).map(([metricName, { unit, data }]) => (
                                <div key={metricName} className='metric-chart'>
                                    <h4>{metricName} ({unit})</h4>
                                    <ResponsiveContainer width="100%" height={200}>
                                        <LineChart data={data}>
                                            <CartesianGrid strokeDasharray="3 3" />
                                            <XAxis
                                                dataKey="date"
                                                tick={{ fontSize: 10 }}
                                            />
                                            <YAxis />
                                            <Tooltip />
                                            <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
                                        </LineChart>
                                    </ResponsiveContainer>
                                </div>
                            ))}
                        </div>
                    </div>
                );
            })()}

            {loading && (
                <SkeletonUI />
            )}

            {error && (
                <ErrorBox error={error} setError={setError} />
            )}
        </div>
    )
}

export default PatientMedicalHistory;