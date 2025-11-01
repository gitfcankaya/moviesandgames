import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import './App.css';

function App() {
  return (
    <Router>
      <div className="app">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/movies" element={<Home />} />
            <Route path="/series" element={<Home />} />
            <Route path="/games" element={<Home />} />
            <Route path="/categories" element={<Home />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
