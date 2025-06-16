// Modules

// Styling sheet
import './AppointmentDetailBox.css';

// Assets
import PersonIcon from '../../uploads/icon/person.png';
import AlbumIcon from '../../uploads/icon/album.png';
import BabyIcon from '../../uploads/icon/baby.png';
import CrossIcon from '../../uploads/icon/cross.png';
import RulerIcon from '../../uploads/icon/ruler.png';
import WeightIcon from '../../uploads/icon/weight.png';
import PhoneIcon from '../../uploads/icon/phone.png';
import ExitIcon from '../../uploads/icon/exit.png';
import CakeIcon from '../../uploads/icon/cake.png';
import LogoPicture from '../../uploads/logo-picture.png';
import LogoText from '../../uploads/logo-text.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function AppointmentDetailBox({ appointmentDetail, setAppointmentDetail }) {
    const t1 = 'Thông tin bệnh nhân';
    const t2 = 'Giới tính';
    const t3 = 'Số điện thoại';
    const t4 = 'Chiều cao';
    const t5 = 'Cân nặng';
    const t6 = 'Tình trạng HIV';
    const t7 = 'Tình trạng thai kỳ';
    const t8 = 'trống';
    const t9 = 'Lý do của cuộc hẹn';
    const t10 = 'Ghi chú của cuộc hẹn';
    const t11 = 'Ngày sinh';

    return (
        <div
            className='appointment-detail-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.appointment-detail-box')) {
                    setAppointmentDetail(null);
                    e.stopPropagation();
                }
            }}
        >

            <div className='appointment-detail-box'>
                <div className='header'>
                    <div className='title'>
                        <Icon src={AlbumIcon} alt={'album-icon'} />
                        {t1}
                    </div>

                    <button
                        onClick={() => setAppointmentDetail(null)}
                        className='exit'
                    >
                        <Icon src={ExitIcon} alt={'exit-icon'} />
                    </button>
                </div>
                <div className='body'>
                    <div className='patient-info'>
                        <div className='name'>
                            <Icon src={PersonIcon} alt={'person-icon'} />

                            <div className='detail'>
                                <div>
                                    {appointmentDetail.patientName}
                                </div>
                                <div className='sub-detail'>
                                    {t2}&nbsp;
                                    {
                                        appointmentDetail.patientProfile.gender ?
                                            appointmentDetail.patientProfile.gender : t8
                                    }
                                </div>
                            </div>
                        </div>
                        <div className='phone'>
                            <Icon src={PhoneIcon} alt={'phone-icon'} />

                            <div className='detail'>
                                <div>
                                    {t3}
                                </div>
                                <div className='sub-detail'>
                                    {appointmentDetail.patientPhone}
                                </div>
                            </div>
                        </div>

                        <div className='height'>
                            <Icon src={RulerIcon} alt={'ruler-icon'} />

                            <div className='detail'>
                                <div>
                                    {t4}
                                </div>
                                <div className='sub-detail'>
                                    {
                                        appointmentDetail.patientProfile.height ?
                                            appointmentDetail.patientProfile.height : t8
                                    }
                                </div>
                            </div>
                        </div>

                        <div className='weight'>
                            <Icon src={WeightIcon} alt={'weight-icon'} />

                            <div className='detail'>
                                <div>
                                    {t5}
                                </div>
                                <div className='sub-detail'>
                                    {
                                        appointmentDetail.patientProfile.weight ?
                                            appointmentDetail.patientProfile.weight : t8
                                    }
                                </div>
                            </div>
                        </div>

                        <div className='dob'>
                            <Icon src={CakeIcon} alt={'cake-icon'} />

                            <div className='detail'>
                                <div>
                                    {t11}
                                </div>
                                <div className='sub-detail'>
                                    {
                                        appointmentDetail.patientDob ?
                                            appointmentDetail.patientDob.split('T')[0] : t8
                                    }
                                </div>
                            </div>
                        </div>

                        <div className='hiv-status'>
                            <Icon src={CrossIcon} alt={'cross-icon'} />

                            <div className='detail'>
                                <div>
                                    {t6}
                                </div>
                                <div className='sub-detail'>
                                    {
                                        appointmentDetail.patientProfile.hivStatusName ?
                                            appointmentDetail.patientProfile.hivStatusName : t8
                                    }
                                </div>
                            </div>
                        </div>

                        <div className='pregnancy'>
                            <Icon src={BabyIcon} alt={'baby-icon'} />

                            <div className='detail'>
                                <div>
                                    {t7}
                                </div>
                                <div className='sub-detail'>
                                    {
                                        appointmentDetail.patientProfile.pregnancyStatusName ?
                                            appointmentDetail.patientProfile.pregnancyStatusName : t8
                                    }
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className='appointment-detail'>
                        <div className='reason'>
                            <div className='title'>
                                {t9}
                            </div>

                            <div className='content'>
                                {appointmentDetail.reason ? appointmentDetail.reason : t8}
                            </div>
                        </div>

                        <div className='note'>
                            <div className='title'>
                                {t10}
                            </div>

                            <div className='content'>
                                {appointmentDetail.notes ? appointmentDetail.notes : t8}
                            </div>
                        </div>
                    </div>
                </div>
                <div className='footer'>
                    <div className='logo'>
                        <img src={LogoPicture} alt='logo-picture' />
                        <img src={LogoText} alt='logo-text' />
                    </div>
                </div>
            </div>
        </div>
    )
}

export default AppointmentDetailBox;