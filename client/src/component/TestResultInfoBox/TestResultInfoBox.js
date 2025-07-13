// Modules
import { useState } from 'react';

// Styling
import './TestResultInfoBox.css'

// Assets
import BeakerIcon from '../../uploads/icon/beaker.png';

// Components
import SkeletonUI from '../SkeletonUI/SkeletonUI';
import ErrorBox from '../ErrorBox/ErrorBox';
import Icon from '../Icon/Icon';

// Hooks
import useGetPatientTestResult from '../../hook/useGetPatientTestResult';

function TestResultInfoBox({ user }) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const { testResults } = useGetPatientTestResult({
        userId: user?.UserId,
        setLoading,
        setError
    });

    const title = 'Kết quả xét nghiệm';
    const noResult = 'Không có kết quả xét nghiệm.';
    const metricTitle1 = 'Chỉ số';
    const metricTitle2 = 'Giá trị';
    const metricTitle3 = 'Đơn vị';
    const t1 = 'Ngày xét nghiệm';
    const t2 = 'Giai đoạn';
    const t3 = 'Ghi chú';
    const t4 = 'Tùy chỉnh';
    const t5 = 'Không có';
    const t6 = 'Có';
    const t7 = 'Không';

    return (
        <div className="test-result-info-box">
            {loading && <SkeletonUI />}
            {error && <ErrorBox error={error} setError={setError} />}

            <div className='test-result-info'>
                <div className='title'>{title}</div>
                {testResults?.length > 0 ? (
                    (() => {
                        const latestResult = testResults.reduce((latest, current) =>
                            new Date(current.date) > new Date(latest.date) ? current : latest
                        );

                        return (
                            <div className='test-section'>
                                <div className='general'>
                                    <div className='test-header'>
                                        <Icon src={BeakerIcon} alt={'test-icon'} />
                                        <span>{latestResult.testTypeName}</span>
                                    </div>

                                    <div className='test-detail'>
                                        <div className='test-box'>
                                            <div className='title'>{t1}</div>
                                            <div className='info'>{new Date(latestResult.date).toLocaleString('vi-VN')}</div>
                                        </div>

                                        <div className='test-box'>
                                            <div className='title'>{t2}</div>
                                            <div className='info'>{latestResult.testStageName}</div>
                                        </div>

                                        <div className='test-box'>
                                            <div className='title'>{t3}</div>
                                            <div className='info'>{latestResult.note || t5}</div>
                                        </div>

                                        <div className='test-box'>
                                            <div className='title'>{t4}</div>
                                            <div className='info'>{latestResult.isCustomized ? t6 : t7}</div>
                                        </div>
                                    </div>
                                </div>



                                <div className='test-stat'>
                                    <div className='test-metric-header'>
                                        <div>{metricTitle1}</div>
                                        <div>{metricTitle2}</div>
                                        <div>{metricTitle3}</div>
                                    </div>
                                    <div className='metrics'>
                                        {latestResult.testMetricValues.map((metric, i) => (
                                            <div key={i} className='test-metric-row'>
                                                <div>{metric.testMetricName}</div>
                                                <div>{metric.value}</div>
                                                <div>{metric.unitName}</div>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            </div>
                        );
                    })()
                ) : (
                    <p className='empty'>{noResult}</p>
                )}
            </div>
        </div>
    );
}

export default TestResultInfoBox;
