import { useState, useEffect } from 'react';
import { contentService } from '../services/api';
import ContentCard from '../components/ContentCard';
import './Home.css';

function Home() {
  const [contents, setContents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchContents();
  }, []);

  const fetchContents = async () => {
    try {
      setLoading(true);
      const response = await contentService.getAll();
      setContents(response.data);
    } catch (err) {
      setError('Failed to load content. Please try again later.');
      console.error('Error fetching contents:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Loading amazing content...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <p className="error-message">{error}</p>
        <button onClick={fetchContents} className="retry-button">
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="home-page">
      <div className="hero-section">
        <h1 className="hero-title">Welcome to Movies & Games</h1>
        <p className="hero-subtitle">
          Discover the latest movies, series, and games from around the world
        </p>
      </div>

      <div className="content-section">
        <h2 className="section-title">Featured Content</h2>
        <div className="content-grid">
          {contents.map((content) => (
            <ContentCard key={content.id} content={content} />
          ))}
        </div>
        
        {contents.length === 0 && (
          <div className="empty-state">
            <p>No content available yet. Check back soon!</p>
          </div>
        )}
      </div>
    </div>
  );
}

export default Home;
