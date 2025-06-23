// Modules

// Styling sheet
import './MedicationInfoBox.css';

// Assets
import ExitIcon from '../../uploads/icon/exit.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function MedicationInfoBox({ medicationInfo, setMedicationInfo }) {
    const t1 = 'Tên thuốc';
    const t2 = 'Chỉ định';
    const t3 = 'Chống chỉ định';
    const t4 = 'Tác dụng phụ';

    return (
        <div
            className='medication-info-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.medication-info-box')) {
                    setMedicationInfo(null);
                    e.stopPropagation();
                }
            }}
        >
            <div className='medication-info-box'>
                <div className='header'>
                    <div className='logo'>
                        <img src={LogoPicture} alt='logo-picture' />
                        <img src={LogoText} alt='logo-text' />
                    </div>
                    <button
                        onClick={() => setMedicationInfo(null)} 
                        className='exit'
                    >
                        <Icon src={ExitIcon} alt={'exit-icon'} />
                    </button>
                </div>

                <div className='body'>
                    <div className='name'>
                        <div className='medication-title'>
                            {t1}
                        </div>
                        <div className='medication-detail'>
                            {medicationInfo.medicationName}
                        </div>
                    </div>

                    <div className='indication'>
                        <div className='medication-title'>
                            {t2}
                        </div>
                        <div className='medication-detail'>
                            {medicationInfo.indications}
                        </div>
                    </div>

                    <div className='contraindication'>
                        <div className='medication-title'>
                            {t3}
                        </div>
                        <div className='medication-detail'>
                            {medicationInfo.contraindications}
                        </div>
                    </div>

                    <div className='side-effect'>
                        <div className='medication-title'>
                            {t4}
                        </div>
                        <div className='medication-detail'>
                            {medicationInfo.sideEffect}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default MedicationInfoBox;