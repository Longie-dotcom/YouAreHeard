// Modules

// Styling sheet
import './ConfirmTestLabBox.css';

// Assets
import PillIcon from '../../uploads/icon/pill.png';
import BeakerIcon from '../../uploads/icon/beaker.png';
import RulerIcon from '../../uploads/icon/ruler.png';
import LogoText from '../../uploads/logo-text.png';
import LogoPicture from '../../uploads/logo-picture.png';

// Components
import Icon from '../Icon/Icon';

// Hooks
import useCreateTestLab from '../../hook/useCreateTestLab';

function ConfirmTestLabBox({ labResult, setLabResult, setError, setLoading, setFinish }) {
    const t1 = 'Thông tin của bệnh nhân';
    const t2 = 'Thông tin xét nghiệm';
    const t3 = 'Giai đoạn xét nghiệm';
    const t4 = 'Loại xét nghiệm';
    const t5 = 'Thông số xét nghiệm';
    const t6 = 'Tên chỉ số';
    const t7 = 'Giá trị';
    const t8 = 'Đơn vị';
    const t9 = '-';
    const t10 = 'Ghi chú chung';
    const t11 = 'Xác nhận mẫu thông tin xét nghiệm';
    const t12 = 'Xác nhận';
    const t14m1 = 'Hủy';
    const t14 = 'Điều chỉnh';
    const t15 = 'Có điều chỉnh';
    const t16 = 'Không có điều chỉnh';

    const {
        createTestLab
    } = useCreateTestLab({ setError, setLoading, setFinish });

    return (
        <div
            className='confirm-test-lab-overlap'
            onClick={(e) => {
                if (!e.target.closest('.confirm-test-lab')) {
                    setLabResult(null);
                    e.stopPropagation();
                }
            }}
        >
            <div className='confirm-test-lab'>
                <div className='header'>
                    <div className='header-title'>
                        {t11}
                    </div>

                    <div className='logo'>
                        <img src={LogoPicture} alt='logo-picture' />
                        <img src={LogoText} alt='logo-text' />
                    </div>
                </div>
                <div className='body'>
                    <div className='patient-info'>
                        <div className='title'>
                            {t1}
                        </div>
                    </div>
                    <div className='test-lab'>
                        <div className='title'>
                            {t2}
                        </div>

                        <div className='stage'>
                            <div className='sub-title'>
                                {t3}
                            </div>
                            <div className='detail'>
                                {labResult.testStageName}
                            </div>
                        </div>

                        <div className='type'>
                            <div className='sub-title'>
                                {t4}
                            </div>
                            <div className='detail'>
                                {labResult.testTypeName}
                            </div>
                        </div>

                        <div className='is-customize'>
                            <div className='sub-title'>
                                {t14}
                            </div>
                            <div className={labResult.isCustomized ? 'detail customized' : 'detail non-customized'}>
                                {labResult.isCustomized ? t15 : t16}
                            </div>
                        </div>

                        <div className='title'>
                            {t5}
                        </div>
                        <div className="test-metric-values-grid">
                            <div className="test-metric-header">
                                <div>
                                    <Icon src={PillIcon} alt={'icon'} />
                                    {t6}
                                </div>
                                <div>
                                    <Icon src={BeakerIcon} alt={'beaker-icon'} />
                                    {t7}
                                </div>
                                <div>
                                    <Icon src={RulerIcon} alt={'ruler-icon'} />
                                    {t8}
                                </div>
                            </div>

                            {labResult.testMetricValues
                                ?.filter(m => m.value !== null && m.value !== '')
                                .map((metric, idx) => (
                                    <div key={idx} className="test-metric-row">
                                        <div>{metric.testMetricName}</div>
                                        <div>{metric.value}</div>
                                        <div>{metric.unitName || t9}</div>
                                    </div>
                                ))}
                        </div>

                        <div className='title'>
                            {t10}
                        </div>
                        <div className='note'>
                            {labResult.note}
                        </div>
                    </div>
                </div>

                <div className='footer'>
                    <button
                        onClick={() => {
                            createTestLab({ labResult: labResult })
                            setLabResult(null);
                        }}
                        className='submit-button'
                    >
                        {t12}
                    </button>
                    <button
                        onClick={() =>
                            setLabResult(null)
                        }
                        className='cancel-button'
                    >
                        {t14m1}
                    </button>
                </div>
            </div>
        </div>
    )
}

export default ConfirmTestLabBox;