import { Link } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">
          Movies & Games
        </Link>
        <ul className="navbar-menu">
          <li className="navbar-item">
            <Link to="/" className="navbar-link">Home</Link>
          </li>
          <li className="navbar-item">
            <Link to="/movies" className="navbar-link">Movies</Link>
          </li>
          <li className="navbar-item">
            <Link to="/series" className="navbar-link">Series</Link>
          </li>
          <li className="navbar-item">
            <Link to="/games" className="navbar-link">Games</Link>
          </li>
          <li className="navbar-item">
            <Link to="/categories" className="navbar-link">Categories</Link>
          </li>
        </ul>
      </div>
    </nav>
  );
}

export default Navbar;
