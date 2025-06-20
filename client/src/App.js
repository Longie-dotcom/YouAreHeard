import {
  BrowserRouter as Router,
  Routes,
  Route
} from 'react-router-dom';

import useUser from './hook/useUser';

// Components
import NavBar from './component/NavBar/NavBar';
import RequireRole from './component/ProtectPage/RequireRole';

// Authentication
import LoginPage from './page/Authentication/LoginPage/LoginPage';
import RegisterPage from './page/Authentication/RegisterPage/RegisterPage';

// Pages
import AppointmentPage from './page/PatientPage/AppointmentPage/AppointmentPage';
import HomePage from './page/PatientPage/HomePage/HomePage';
import InfoPage from './page/PatientPage/InfoPage/InfoPage';

import DoctorDashboardPage from './page/DoctorPage/DoctorDashboardPage/DoctorDashboardPage';

function AppContent() {
  const { user, setReloadCookies } = useUser();

  return (
    <>
      {user?.RoleId !== Number(process.env.REACT_APP_ROLE_DOCTOR_ID) && (
        <NavBar user={user} setReloadCookies={setReloadCookies} />
      )}
      <Routes>
        <Route path='/' element={<LoginPage setReloadCookies={setReloadCookies} />} />

        <Route path="/login" element={<LoginPage setReloadCookies={setReloadCookies} />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route path="/homePage" element={<HomePage user={user} />} />
        <Route path="/appointmentPage" element={<AppointmentPage user={user} />} />
        <Route path="/infoPage" element={<InfoPage user={user} />} />

        <Route
          path="/doctorDashboardPage"
          element={
            <RequireRole user={user} allowedRoles={[Number(process.env.REACT_APP_ROLE_DOCTOR_ID)]}>
              <DoctorDashboardPage user={user} setReloadCookies={setReloadCookies} />
            </RequireRole>
          }
        />
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
