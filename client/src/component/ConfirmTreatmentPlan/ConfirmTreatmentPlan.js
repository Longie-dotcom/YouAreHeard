// Modules

// Styling sheet
import './ConfirmTreatmentPlan.css';

// Assets
import TimeIcon from '../../uploads/icon/time.png';
import PillIcon from '../../uploads/icon/pill.png';
import BeakerIcon from '../../uploads/icon/beaker.png';
import TreatmentIcon from '../../uploads/icon/treatment.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useCreateTreatmentPlan from '../../hook/useCreateTreatmentPlan';

function ConfirmTreatmentPlan({ confirmTreatmentPlan, setIsSubmit, setError, setLoading, selectedRegimen, isAdjusted, setFinish }) {
    const t1 = 'Xác nhận thông tin đơn thuốc/phác đồ điều trị';
    const t2 = 'Ghi chú cho thuốc'
    const t6 = 'Loại phác đồ';
    const t7 = 'Thời gian sử dụng';
    const t8 = 'Thông tin phác đồ có sẵn (gốc)';
    const t9 = 'Điều chỉnh';
    const t10 = 'Chỉ định';
    const t11 = 'Chống chỉ định';
    const t12 = 'Tác dụng phụ';
    const t14m1 = 'Có điều chỉnh';
    const t14 = 'Không có điều chỉnh';
    const t15 = 'Thông tin phác đồ điều trị';
    const t16 = 'Tên thuốc';
    const t17 = 'Thời gian uống';
    const t18 = 'Liều lượng';
    const t19 = 'Xác nhận';
    const t20 = 'Ghi chú chung';
    const t21 = 'Không có ghi chú';
    const t22 = 'Hủy';

    const {
        createTreatmentPlan
    } = useCreateTreatmentPlan({ setError, setLoading, setIsSubmit, setFinish });

    const transformedPillRemindTimes = confirmTreatmentPlan.pillRemindTime.flatMap(med =>
        Array.isArray(med.remindTimes)
            ? med.remindTimes
                .filter(rt => rt.time && rt.time.trim() !== "")
                .map(rt => ({
                    time: rt.time,
                    medicationID: med.medicationID,
                    drinkDosage: rt.dosage,
                    medicationName: rt.medicationName,
                    notes: med.notes,
                    dosageMetric: med.dosageMetric
                }))
            : []
    );

    return (
        <div
            className='confirm-treatment-plan-overlap'
            onClick={(e) => {
                if (!e.target.closest('.confirm-treatment-plan')) {
                    setIsSubmit(false);
                    e.stopPropagation();
                }
            }}
        >

            <div className='confirm-treatment-plan'>
                <div className='header'>
                    <div className='title'>
                        <Icon src={TreatmentIcon} alt={'treatment-icon'} />{t1}
                    </div>

                    <div className='logo'>
                        <img src={LogoPicture} />
                        <img src={LogoText} />
                    </div>
                </div>

                <div className='body'>
                    <div className='title'>
                        {t8}
                    </div>
                    <div className='regimen'>
                        <div>
                            <div className='sub-title'>
                                {t6}
                            </div>
                            <div className='detail'>
                                {selectedRegimen.type}
                            </div>
                        </div>

                        <div>
                            <div className='sub-title'>
                                {t7}
                            </div>
                            <div className='detail'>
                                {selectedRegimen.duration}
                            </div>
                        </div>

                        <div>
                            <div className='sub-title'>
                                {t10}
                            </div>
                            <div className='detail'>
                                {selectedRegimen.regimenIndications}
                            </div>
                        </div>

                        <div>
                            <div className='sub-title'>
                                {t11}
                            </div>
                            <div className='detail'>
                                {selectedRegimen.regimenContraindications}
                            </div>
                        </div>

                        <div>
                            <div className='sub-title'>
                                {t12}
                            </div>
                            <div className='detail'>
                                {selectedRegimen.regimenSideEffects}
                            </div>
                        </div>

                        <div>
                            <div className='sub-title'>
                                {t9}
                            </div>
                            <div className={isAdjusted ? 'detail is-adjusted' : 'detail is-not-adjusted'}>
                                {isAdjusted ? t14m1 : t14}
                            </div>
                        </div>
                    </div>

                    <div className='title'>
                        {t15}
                    </div>

                    <div className="pill-reminder-grid">
                        <div className="pill-grid-header">
                            <div><Icon src={PillIcon} alt={'pill-icon'} />{t16}</div>
                            <div><Icon src={TimeIcon} alt={'time-icon'} />{t17}</div>
                            <div><Icon src={BeakerIcon} alt={'ruler-icon'} />{t18}</div>
                        </div>

                        {transformedPillRemindTimes.map((pill, index) => (
                            <div key={index} className="pill-grid-row">
                                <div>{pill.medicationName}</div>
                                <div>{pill.time}</div>
                                <div>{pill.drinkDosage} {pill.dosageMetric}</div>
                            </div>
                        ))}
                    </div>

                    <div className='title'>
                        {t2}
                    </div>
                    <div className='pill-notes'>
                        {confirmTreatmentPlan.pillRemindTime
                            .filter(pill => pill.remindTimes?.some(rt => rt.time?.trim()))
                            .map((pill, idx) => (
                                <div key={idx} className="pill-note">
                                    <div className='pill-title'>{pill.medicationName}</div>
                                    <div className='pill-detail'>{pill.notes ? pill.notes : '(Không có ghi chú)'}</div>
                                </div>
                            ))}
                    </div>

                    <div className='title'>
                        {t20}
                    </div>
                    <div className='note'>
                        {confirmTreatmentPlan.treatmentDetail.notes ? confirmTreatmentPlan.treatmentDetail.notes : t21}
                    </div>
                </div>

                <div className='footer'>
                    <button
                        onClick={() => createTreatmentPlan({
                            pillRemindTime: transformedPillRemindTimes,
                            treatmentDetail: confirmTreatmentPlan.treatmentDetail
                        })}
                        className='submit-button'>
                        {t19}
                    </button>
                    <button
                        onClick={() => setIsSubmit(false)}
                        className='cancel-button'>
                        {t22}
                    </button>
                </div>
            </div>
        </div>
    )
}

export default ConfirmTreatmentPlan;