// Modules

// Styling sheet
import './TextBox.css';

// Assets

// Components

// Hooks

function TextBox({ text, setText, title }) {

    return (
        <div
            className='text-box-overlap'
            onClick={(e) => {
                if (!e.target.closest('.text-box')) {
                    setText(null);
                    e.stopPropagation();
                }
            }}
        >
            <div className='text-box'>
                <div className='title'>
                    {title}
                </div>
                {text}
            </div>
        </div>
    )
}

export default TextBox;