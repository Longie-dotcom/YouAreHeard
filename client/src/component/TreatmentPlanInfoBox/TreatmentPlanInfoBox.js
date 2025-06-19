// Modules
import { useState } from 'react';

// Styling sheet
import './TreatmentPlanInfoBox.css';

// Components
import ErrorBox from '../ErrorBox/ErrorBox';
import SkeletonUI from '../SkeletonUI/SkeletonUI';

// Hooks
import useGetLatestTreatmentPlanPatient from '../../hook/useGetLatestTreatmentPlanPatient';

function TreatmentPlanInfoBox({ user }) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const {
        treatmentPlan
    } = useGetLatestTreatmentPlanPatient({ setError, setLoading, patientId: user.UserId });

    return (
        <div className='treatment-plan-info-box'>
            {loading && <SkeletonUI />}
            {error && <ErrorBox error={error} setError={setError} />}

            {!loading && !error && treatmentPlan && (
                <>
                    <h2>📋 Thông tin phác đồ điều trị</h2>

                    <div className="section">
                        <h3>🧑‍⚕️ Thông tin chung</h3>
                        <p><strong>Ngày tạo:</strong> {new Date(treatmentPlan.date).toLocaleString('vi-VN')}</p>
                        <p><strong>Ghi chú:</strong> {treatmentPlan.notes || 'Không có'}</p>
                    </div>

                    <div className="section">
                        <h3>💊 Thông tin phác đồ</h3>
                        <p><strong>Tên phác đồ:</strong> {treatmentPlan.regimenName}</p>
                        <p><strong>Loại:</strong> {treatmentPlan.regimenType}</p>
                        <p><strong>Thời gian sử dụng:</strong> {treatmentPlan.regimenDuration}</p>
                        <p><strong>Chỉ định:</strong> {treatmentPlan.regimenIndications}</p>
                        <p><strong>Tác dụng phụ:</strong> {treatmentPlan.regimenSideEffects}</p>
                        <p><strong>Chống chỉ định:</strong> {treatmentPlan.regimenContraindications}</p>
                    </div>

                    <div className="section">
                        <h3>⏰ Nhắc uống thuốc</h3>

                        {treatmentPlan.pillRemindTimes?.length > 0 ? (
                            <>
                                <div className="pill-grid-header">
                                    <div>🕒 Giờ</div>
                                    <div>💊 Thuốc</div>
                                    <div>📦 Liều lượng</div>
                                </div>
                                {treatmentPlan.pillRemindTimes.map((pill, index) => (
                                    <div key={index} className="pill-grid-row">
                                        <div>{pill.time}</div>
                                        <div>{pill.medicationName}</div>
                                        <div>{pill.drinkDosage} {pill.dosageMetric}</div>
                                    </div>
                                ))}
                            </>
                        ) : (
                            <p>Không có nhắc uống thuốc nào.</p>
                        )}
                    </div>
                </>
            )}
        </div>
    );
}

export default TreatmentPlanInfoBox;
