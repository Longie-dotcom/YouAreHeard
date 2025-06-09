// Modules

// Styling sheet
import './ErrorBox.css';

// Assets
import ErrorIcon from '../../uploads/icon/error.png';

// Components
import Icon from '../Icon/Icon';

// Hooks

function ErrorBox({ error, setError }) {

    return (
        <div
            className='error-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.error-box')) {
                    setError(null);
                    e.stopPropagation();
                }
            }}
        >

            <div className='error-box'>
                <Icon src={ErrorIcon} alt={'error-icon'} />
                {error}
            </div>
        </div>
    )
}

export default ErrorBox;