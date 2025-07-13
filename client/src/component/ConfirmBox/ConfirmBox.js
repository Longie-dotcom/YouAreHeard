// Modules

// Styling sheet
import './ConfirmBox.css';

// Components
import Icon from '../Icon/Icon';

// Hooks

function ConfirmBox({ setOpenConfirm, action, title, detail, icon, alt }) {
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
                <div className='title'>
                    <Icon src={icon} alt={alt ? alt : 'icon'} />
                    {title}
                </div>

                <div className='detail'>
                    {detail}
                </div>
                
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