// Modules

// Styling sheet
import './ConfirmBox.css';

// Assets
import ConfirmIcon from '../../uploads/icon/confirm.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function ConfirmBox({ setOpenConfirm, action, text }) {
    const t1 = 'Xác nhận';
    const t2 = 'Hủy';

    return (
        <div
            className='confirm-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.confirm-box')) {
                    setOpenConfirm(false);
                    e.stopPropagation();
                }
            }}
        >

            <div className='confirm-box'>
                <Icon src={ConfirmIcon} alt={'confirm-icon'} />
                {text}
                <div className='confirm-buttons'>
                    <button
                        onClick={() => {
                            action();
                            setOpenConfirm(false);
                        }}
                        className='accept'
                    >
                        {t1}
                    </button>

                    <button
                        onClick={() => setOpenConfirm(false)}
                        className='cancel'
                    >
                        {t2}
                    </button>
                </div>
            </div>
        </div>
    )
}

export default ConfirmBox;