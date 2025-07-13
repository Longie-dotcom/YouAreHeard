import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

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
  const [user, setUser] = useState(undefined); // undefined = loading, null = no user
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const userData = getUserFromCookies();
    setUser(userData);

    // Only auto-redirect if user just landed at root
    if (userData) {
      const currentPath = location.pathname;

      if (currentPath === '/' || currentPath === '/login') {
        if (userData.RoleId === Number(process.env.REACT_APP_ROLE_PATIENT_ID)) {
          navigate('/homePage');
        } else if (userData.RoleId === Number(process.env.REACT_APP_ROLE_DOCTOR_ID)) {
          navigate('/doctorDashboardPage');
        } else if (userData.RoleId === Number(process.env.REACT_APP_ROLE_STAFF_ID)) {
          navigate('/staffDashboardPage');
        }
      }
    }
  }, [reloadCookies]);

  return { user, setReloadCookies };
};

export default useUser;
