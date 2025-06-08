import './HomePage.css'
import Logo from '../../../uploads/logo-nobg.png';

function HomePage({ user }) {
    return (
        <div className='home-page'>
            <div className="background-image" />
            <div className="content">
                <div className='greeting'>
                    <div className='greeting-word'>
                        Hi!
                    </div>
                    <div className='greeting-logo'>
                        <img src={Logo}/>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default HomePage;