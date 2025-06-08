import {
  BrowserRouter as Router,
  Routes,
  Route
} from 'react-router-dom';

import useUser from './hook/useUser';

// Components
import NavBar from './component/NavBar/NavBar';

// Authentication
import LoginPage from './page/Authentication/LoginPage/LoginPage';
import RegisterPage from './page/Authentication/RegisterPage/RegisterPage';

// Pages
import AppointmentPage from './page/PatientPage/AppointmentPage/AppointmentPage';
import HomePage from './page/PatientPage/HomePage/HomePage';

function AppContent() {
  const { user, setReloadCookies } = useUser();

  return (
    <>
      <NavBar user={user} />
      <Routes>
        <Route path='/' element={<HomePage user={user} />} />

        <Route path="/login" element={<LoginPage setReloadCookies={setReloadCookies} />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route path="/homePage" element={<HomePage user={user} />} />
        <Route path="/appointmentPage" element={<AppointmentPage user={user} />} />
      </Routes>
    </>
  );
}

function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}

export default App;
