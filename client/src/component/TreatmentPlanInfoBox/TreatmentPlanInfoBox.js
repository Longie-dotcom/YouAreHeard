// Modules
import { useState } from 'react';

// Styling sheet
import './TreatmentPlanInfoBox.css';

// Assets
import TimeIcon from '../../uploads/icon/time.png';
import PillIcon from '../../uploads/icon/pill.png';
import BeakerIcon from '../../uploads/icon/beaker.png';

// Components
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import Icon from '../Icon/Icon';

// Hooks
import useGetLatestTreatmentPlanPatient from '../../hook/useGetLatestTreatmentPlanPatient';

function TreatmentPlanInfoBox({ user }) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const {
        treatmentPlan
    } = useGetLatestTreatmentPlanPatient({ setError, setLoading, patientId: user?.UserId });

    const t1 = "Thông tin phác đồ điều trị";
    const t2 = "Thông tin chung";
    const t3 = "Ngày tạo";
    const t4 = "Ghi chú chung";
    const t5 = "Tên phác đồ";
    const t6 = "Loại";
    const t7 = "Thời gian sử dụng";
    const t8 = "Chỉ định";
    const t9 = "Tác dụng phụ";
    const t10 = "Chống chỉ định";
    const t11 = "Điều chỉnh";
    const t12 = "Lịch nhắc uống thuốc";
    const t14m1 = "Giờ";
    const t14 = "Thuốc";
    const t15 = "Liều lượng";
    const t16 = "Thông tin phác đồ";
    const t17 = "Không có ghi chú"
    const t18 = "Ghi chú của thuốc uống"
    const t19 = "Người dùng chưa có phác đồ điều trị.";

    return (
        <div className='treatment-plan-info-box'>
            {loading && <SkeletonUI />}
            {error && <ErrorBox error={error} setError={setError} />}
            {(!loading && !error && treatmentPlan) ? (
                <>
                    <div className="section treatment-general-info">
                        <div className='title'>{t1}</div>

                        <div className="general-info">
                            <div className='header'>{t2}</div>
                            <div className='box'>
                                <div className='title'>
                                    {t3}
                                </div>
                                <div className='info'>
                                    {new Date(treatmentPlan.date).toLocaleString('vi-VN')}
                                </div>
                            </div>

                            <div className='box'>
                                <div className='title'>
                                    {t4}
                                </div>
                                <div className='info'>
                                    {treatmentPlan.notes || t17}
                                </div>
                            </div>
                        </div>

                        <div className="detail-info">
                            <div className='header'>{t16}</div>

                            <div className='box'>
                                <div className='title'>{t5}</div>
                                <div className='info'>{treatmentPlan.regimenName}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t6}</div>
                                <div className='info'>{treatmentPlan.regimenType}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t7}</div>
                                <div className='info'>{treatmentPlan.regimenDuration}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t8}</div>
                                <div className='info'>{treatmentPlan.regimenIndications}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t9}</div>
                                <div className='info'>{treatmentPlan.regimenSideEffects}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t10}</div>
                                <div className='info'>{treatmentPlan.regimenContraindications}</div>
                            </div>

                            <div className='box'>
                                <div className='title'>{t11}</div>
                                <div className='info'>{treatmentPlan.isCustomized ? 'Có' : 'Không'}</div>
                            </div>
                        </div>
                    </div>

                    <div className="section pill-table">
                        <div className='title'>{t12}</div>

                        {treatmentPlan.pillRemindTimes?.length > 0 ? (
                            <>
                                <div className="pill-grid-header">
                                    <div>
                                        <Icon src={TimeIcon} alt={'time-icon'} />
                                        {t14m1}
                                    </div>
                                    <div>
                                        <Icon src={PillIcon} alt={'pill-icon'} />
                                        {t14}
                                    </div>
                                    <div>
                                        <Icon src={BeakerIcon} alt={'beaker-icon'} />
                                        {t15}
                                    </div>
                                </div>

                                {treatmentPlan.pillRemindTimes.map((pill, index) => (
                                    <div key={index} className="pill-grid-row">
                                        <div>{pill.time}</div>
                                        <div>{pill.medicationName}</div>
                                        <div>{pill.drinkDosage} {pill.dosageMetric}</div>
                                    </div>
                                ))}

                                <div className='title'>{t18}</div>
                                <div className='pill-notes'>
                                    {[
                                        ...new Map(
                                            treatmentPlan.pillRemindTimes.map(pill => [pill.medicationID, pill])
                                        ).values()
                                    ].map((pill, index) => (
                                        <div key={pill.medicationID} className='pill-note'>
                                            <div className='name'>{pill.medicationName}</div>
                                            <div className='note'>{pill.notes ? pill.notes : t17}</div>
                                        </div>
                                    ))}
                                </div>
                            </>
                        ) : (
                            <p>{t19}</p>
                        )}
                    </div>
                </>
            ) : (
                <p className='empty'>{t19}</p>
            )}
        </div>
    );
}

export default TreatmentPlanInfoBox;
