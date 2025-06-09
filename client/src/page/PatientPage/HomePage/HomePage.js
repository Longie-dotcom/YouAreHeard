// Modules
import { useNavigate } from 'react-router-dom';

// Styling sheet
import './HomePage.css'

// Assets
import Logo from '../../../uploads/logo-nobg.png';

// Components
import SkeletonUI from '../../../component/SkeletonUI/SkeletonUI';
import ErrorBox from '../../../component/ErrorBox/ErrorBox';

// Hooks
import useLogin from '../../../hook/useLogin';


function HomePage({ user }) {
    return (
        <div className='home-page'>
            <div className='section-1'>
                <div className='title'>
                    AAA
                </div>

                <div className='show'>
                </div>
            </div>
        </div>
    )
}

export default HomePage;