import './Icon.css';

function Icon({ src, alt }) {
    return (
        <img className='icon' src={src} alt={alt} />
    )
}

export default Icon;