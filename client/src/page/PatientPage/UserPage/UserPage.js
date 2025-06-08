import './UserPage.css';
import useLogout from '../../../hook/useLogout';

function UserPage({ user, setReloadCookies }) {
    const { logout } = useLogout({ setReloadCookies }); 
    
    return (
        <div className='user-page'>
            <div className='background-image'></div>
            <div className='user'>
                <div className='user-info'>
                    <div className='detail'>
                        <div className='header'>
                            <div className='name'>
                                {user.name}
                            </div>
                            <div className='setting'>
                                <button>
                                    <i className="bi bi-gear"></i>
                                </button>
                                <button onClick={() => logout()}>
                                    <i className="bi bi-box-arrow-right"></i>
                                </button>
                            </div>
                        </div>

                        <div className='body'>
                            <div className='title'>
                                <div className='hiv-status'>
                                    HIV status
                                </div>
                                <div className='age'>
                                    Age
                                </div>
                                <div className='gender'>
                                    Gender
                                </div>
                                {user.gender === 'female' && (<div className='pregnancy'>
                                    Pregnancy status
                                </div>)}
                            </div>
                            
                            <div className='content'>
                                <div className='hiv-status'>
                                    unknown
                                </div>                                
                                <div className='age'>
                                    {user.age}
                                </div>
                                <div className='gender'>
                                    {user.gender}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className='remind-notification'>

                    </div>
                </div>
                <div className='user-post'>
                    aa
                </div>
            </div>
        </div>
    )
}

export default UserPage;