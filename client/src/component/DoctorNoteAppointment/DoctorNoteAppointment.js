// Modules
import { useRef, useState } from 'react';

// Styling sheet
import './DoctorNoteAppointment.css';

// Assets
import NoteIcon from '../../uploads/icon/note.png';
import UserIcon from '../../uploads/icon/user.png';
import TimeIcon from '../../uploads/icon/time.png';
import ScheduleIcon from '../../uploads/icon/schedule.png';
import LocationIcon from '../../uploads/icon/location.png';
import ZoomIcon from '../../uploads/icon/zoom.png';
import TicketIcon from '../../uploads/icon/ticket.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useUpdateDoctorNote from '../../hook/useUpdateDoctorNote';

function DoctorNoteAppointment({ appointment, setOpenNoteAppointment, setFinish, setError, setLoading }) {
    const t1 = 'Ghi chú cho cuộc hẹn';
    const t2 = 'Thông tin cuộc hẹn';
    const t3 = 'Người hẹn';
    const t4 = 'Loại cuộc hẹn';
    const t5 = 'Ngày';
    const t6 = 'Thời gian';
    const t7 = 'Số thứ tự';
    const t8 = 'Địa điểm';
    const t9 = 'Ghi chú của bác sĩ';
    const t10 = 'Tư vấn trực tuyến';
    const t11 = 'Khám/Điều trị trực tiếp';
    const t12 = 'Zoom meeting';
    const t14m1 = 'Điền ghi chú';
    const t14 = 'Lưu ghi chú';

    const textareaRef = useRef(null);
    const [notes, setNotes] = useState(null);

    const {
        handleRequest
    } = useUpdateDoctorNote({ appointmentID: appointment.appointmentID, notes: notes, setFinish, setError, setLoading })

    return (
        <div
            className='doctor-note-appointment-overlap'
            onClick={(e) => {
                if (!e.target.closest('.doctor-note-appointment')) {
                    setOpenNoteAppointment(null);
                    e.stopPropagation();
                }
            }}
        >
            <div className='doctor-note-appointment'>
                <div className='main-title'>
                    <Icon src={NoteIcon} alt={'note-icon'} />
                    {t1}
                </div>

                <div className='appointment-info'>
                    <div className='sub-title'>
                        {t2}
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={UserIcon} alt={'user-icon'} />
                            {t3}
                        </div>
                        <div className='info'>
                            {appointment.patientName}
                        </div>
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={ScheduleIcon} alt={'schedule-icon'} />
                            {t4}
                        </div>
                        <div className='info'>
                            {appointment.isOnline ? (
                                <>
                                    <Icon src={ZoomIcon} alt={'zoom-icon'} /> {t10}
                                </>
                            ) : t11}
                        </div>
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={TimeIcon} alt={'time-icon'} />
                            {t5}
                        </div>
                        <div className='info'>
                            {appointment.scheduleDate?.split("T")[0]}
                        </div>
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={TimeIcon} alt={'time-icon'} />
                            {t6}
                        </div>
                        <div className='info'>
                            {appointment.startTime}-{appointment.endTime}
                        </div>
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={TicketIcon} alt={'ticket-icon'} />
                            {t7}
                        </div>
                        <div className='info'>
                            {appointment.queueNumber}
                        </div>
                    </div>
                    <div className='box'>
                        <div className='title'>
                            <Icon src={LocationIcon} alt={'location-icon'} />
                            {t8}
                        </div>
                        <div className='info'>
                            {appointment.isOnline ? t12 : appointment.location}
                        </div>
                    </div>
                </div>

                <div className='note'>
                    <div className='sub-title'>
                        {t9}
                    </div>
                    <textarea
                        ref={textareaRef}
                        value={notes}
                        maxLength={200}
                        placeholder={t14m1}
                        onInput={(e) => {
                            const textarea = textareaRef.current;
                            textarea.style.height = 'auto';
                            textarea.style.height = `${textarea.scrollHeight}px`;
                            setNotes(e.target.value);
                        }}
                    />
                </div>

                <div className='footer'>
                    <button
                        onClick={() => handleRequest()}
                        className='submit'
                    >
                        {t14}                       
                    </button>
                </div>
            </div>
        </div>
    )
}

export default DoctorNoteAppointment;