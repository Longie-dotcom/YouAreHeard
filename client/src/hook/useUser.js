import { useState, useEffect } from 'react';

const guest = { 
  _id: '68282d903bfc920748af8058', 
  name: 'Guest', 
  profilePic: 'guest.png',
  role: 'patient'
};

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
  const [user, setUser] = useState(guest);

  useEffect(() => {
    const userData = getUserFromCookies();
    if (userData) {
      setUser(userData);
    } else {
      setUser(guest);
    }
  }, [reloadCookies]);

  return { user, setReloadCookies };
};

export default useUser;
