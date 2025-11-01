# Movies and Games Platform

A comprehensive multi-language web platform for discovering movies, TV series, and games from around the world. Features AI-powered content enhancement, web crawling for content aggregation, user engagement through comments, and Ticketmaster integration for ticket purchasing.

## 🚀 Features

### Core Functionality
- **Content Aggregation:** Automated web crawling from worldwide sources
- **Multi-Language Support:** English, Turkish, Spanish, French, German, and more
- **Content Types:** Movies, TV Series, and Games
- **Rich Categorization:** Action, Drama, Comedy, Horror, Sci-Fi, RPG, FPS, Strategy, etc.
- **Actor/Creator Database:** Comprehensive information about actors and game creators

### User Features
- **User Authentication:** Secure JWT-based authentication
- **Comment System:** Nested comments with moderation
- **Favorites:** Save and manage favorite content
- **Personalization:** Language preferences and custom profiles

### Advanced Features
- **AI Integration:** Content enhancement using OpenAI and Google Gemini
- **Ticketmaster Integration:** Event discovery and ticket purchasing
- **Responsive Design:** Modern, mobile-first UI
- **Search & Filter:** Advanced filtering and full-text search

## 🛠️ Technology Stack

### Backend
- **.NET 9.0** - ASP.NET Core Web API
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Primary database
- **JWT Authentication** - Secure token-based auth
- **BCrypt** - Password hashing
- **HtmlAgilityPack** - Web scraping

### Frontend
- **React 18+** - UI framework
- **Vite** - Build tool and dev server
- **React Router** - Client-side routing
- **Axios** - HTTP client
- **Modern CSS** - Responsive design

### External Integrations
- **OpenAI API** - Content enhancement and generation
- **Google Gemini API** - Alternative AI provider
- **Ticketmaster API** - Event and ticketing data

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB
- API Keys (optional but recommended):
  - OpenAI API Key
  - Google Gemini API Key
  - Ticketmaster API Key

## 🏗️ Project Structure

```
moviesandgames/
├── src/
│   ├── MoviesAndGames.API/          # Web API project
│   │   ├── Controllers/             # API controllers
│   │   ├── Program.cs               # Application entry point
│   │   └── appsettings.json         # Configuration
│   ├── MoviesAndGames.Core/         # Domain layer
│   │   ├── Entities/                # Domain entities
│   │   ├── Interfaces/              # Service interfaces
│   │   ├── DTOs/                    # Data transfer objects
│   │   └── Enums/                   # Enumerations
│   └── MoviesAndGames.Infrastructure/ # Infrastructure layer
│       ├── Data/                    # EF Core context
│       ├── Repositories/            # Repository implementations
│       └── Services/                # Service implementations
├── tests/
│   └── MoviesAndGames.Tests/        # Test project
├── client/                          # React frontend
│   ├── src/
│   │   ├── components/              # React components
│   │   ├── pages/                   # Page components
│   │   └── services/                # API services
│   └── package.json
├── docs/                            # Documentation
│   ├── PRD.md                       # Product Requirements
│   ├── SRS.md                       # Software Requirements
│   ├── SDD.md                       # System Design
│   └── TODO.md                      # Development roadmap
└── MoviesAndGames.sln               # Solution file
```

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/gitfcankaya/moviesandgames.git
cd moviesandgames
```

### 2. Configure the Backend

Update the connection string in `src/MoviesAndGames.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MoviesAndGamesDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Optionally add API keys:

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  },
  "Gemini": {
    "ApiKey": "your-gemini-api-key"
  },
  "Ticketmaster": {
    "ApiKey": "your-ticketmaster-api-key"
  }
}
```

### 3. Build and Run the Backend

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run database migrations
cd src/MoviesAndGames.API
dotnet ef database update --project ../MoviesAndGames.Infrastructure

# Run the API
dotnet run
```

The API will be available at `https://localhost:7215`

### 4. Set Up the Frontend

```bash
cd client

# Install dependencies
npm install

# Configure API URL (optional)
# Create .env file with:
# VITE_API_URL=https://localhost:7215/api

# Start development server
npm run dev
```

The React app will be available at `http://localhost:5173`

## 📚 API Documentation

Once the API is running, access the Swagger documentation at:
```
https://localhost:7215/swagger
```

### Key Endpoints

- `GET /api/content` - List all content
- `GET /api/content/{id}` - Get content by ID
- `POST /api/content` - Create new content
- `GET /api/category` - List categories
- `GET /api/comment/content/{id}` - Get comments
- `POST /api/comment` - Create comment

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## 📖 Documentation

Comprehensive documentation is available in the `docs/` directory:

- **[PRD.md](docs/PRD.md)** - Product Requirements Document
- **[SRS.md](docs/SRS.md)** - Software Requirements Specification
- **[SDD.md](docs/SDD.md)** - System Design Document
- **[TODO.md](docs/TODO.md)** - Development Roadmap

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 🙏 Acknowledgments

- IMDb, Rotten Tomatoes, and other content sources
- OpenAI and Google for AI capabilities
- Ticketmaster for event integration
- The open-source community

## 📧 Contact

For questions or support, please open an issue on GitHub.

---

**Built with ❤️ using .NET Core and React**
