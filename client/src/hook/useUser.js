import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const getUserFromCookies = () => {
  const cookies = document.cookie.split(';').map(c => c.trim());
  const userCookie = cookies.find(c => c.startsWith('user='));
  if (!userCookie) return null;

  try {
    const encoded = userCookie.split('=')[1];
    const decoded = decodeURIComponent(encoded);
    return JSON.parse(decoded);
  } catch (err) {
    console.error('Error parsing user cookie:', err);
    return null;
  }
};

const useUser = () => {
  const [reloadCookies, setReloadCookies] = useState(0);
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const userData = getUserFromCookies();
    if (userData) {
      setUser(userData);
      if (userData.RoleId === Number(process.env.REACT_APP_ROLE_PATIENT_ID)) {
        navigate('/homePage')
      } else if (userData.RoleId === Number(process.env.REACT_APP_ROLE_DOCTOR_ID)) {
        navigate('/doctorDashboardPage');
      }
    } else {
      setUser(null);
    }
  }, [reloadCookies]);

  return { user, setReloadCookies };
};

export default useUser;
