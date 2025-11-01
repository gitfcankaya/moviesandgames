import { Link } from 'react-router-dom';
import './ContentCard.css';

function ContentCard({ content }) {
  const getTypeLabel = (type) => {
    switch (type) {
      case 1: return 'Movie';
      case 2: return 'Series';
      case 3: return 'Game';
      default: return 'Content';
    }
  };

  const getTypeClass = (type) => {
    switch (type) {
      case 1: return 'movie';
      case 2: return 'series';
      case 3: return 'game';
      default: return '';
    }
  };

  return (
    <div className="content-card">
      <div className="content-card-image">
        {content.posterImageUrl ? (
          <img src={content.posterImageUrl} alt={content.title} />
        ) : (
          <div className="content-placeholder">
            {content.title.charAt(0)}
          </div>
        )}
        <span className={`content-type-badge ${getTypeClass(content.type)}`}>
          {getTypeLabel(content.type)}
        </span>
      </div>
      <div className="content-card-body">
        <h3 className="content-title">{content.title}</h3>
        <p className="content-description">
          {content.description.length > 120
            ? content.description.substring(0, 120) + '...'
            : content.description}
        </p>
        <div className="content-card-footer">
          {content.rating && (
            <span className="content-rating">⭐ {content.rating}</span>
          )}
          {content.releaseDate && (
            <span className="content-date">
              {new Date(content.releaseDate).getFullYear()}
            </span>
          )}
        </div>
        <Link to={`/content/${content.slug}`} className="content-link">
          View Details →
        </Link>
      </div>
    </div>
  );
}

export default ContentCard;
