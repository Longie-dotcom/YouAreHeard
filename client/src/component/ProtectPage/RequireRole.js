import { Navigate } from 'react-router-dom';

const RequireRole = ({ user, allowedRoles, children }) => {
  if (!user) {
    // Not logged in, redirect to login
    return <Navigate to="/login" replace />;
  }

  if (!allowedRoles.includes(user.RoleId)) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default RequireRole;