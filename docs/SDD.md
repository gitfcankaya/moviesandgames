# System Design Document (SDD)
## Movies and Games Platform

### 1. System Overview

The Movies and Games Platform is a full-stack web application built using .NET Core for the backend and React for the frontend. The system aggregates entertainment content from multiple sources, enhances it with AI, and provides users with a comprehensive platform for discovery and engagement.

### 2. Architecture

#### 2.1 High-Level Architecture

```
┌─────────────────┐
│   React SPA     │
│   (Frontend)    │
└────────┬────────┘
         │ HTTPS/JSON
         ↓
┌─────────────────┐
│  .NET Core API  │
│   (Backend)     │
└────────┬────────┘
         │
    ┌────┴─────┬──────────┬───────────┐
    ↓          ↓          ↓           ↓
┌────────┐ ┌────────┐ ┌─────────┐ ┌─────────┐
│SQL     │ │OpenAI  │ │Gemini   │ │Ticket   │
│Server  │ │API     │ │API      │ │master   │
└────────┘ └────────┘ └─────────┘ └─────────┘
```

#### 2.2 Clean Architecture Layers

**1. Core Layer (Domain)**
- Entities
- Interfaces
- DTOs
- Enums
- Business logic

**2. Infrastructure Layer**
- Data Access (EF Core)
- External Services
- Repository Implementations
- Third-party API integrations

**3. API Layer (Presentation)**
- Controllers
- Middleware
- Configuration
- Startup logic

### 3. Component Design

#### 3.1 Backend Components

##### 3.1.1 API Controllers
- **ContentController:** CRUD operations for movies, series, games
- **CategoryController:** Category management
- **CommentController:** Comment system
- **UserController:** User management (future)
- **AuthController:** Authentication (future)

##### 3.1.2 Services
- **CrawlerService:** Web scraping implementation
- **ContentEnhancementService:** AI integration for content improvement
- **TicketmasterService:** Event and ticketing integration
- **AuthService:** JWT token generation (future)

##### 3.1.3 Repositories
- **Repository<T>:** Generic repository pattern
- **ContentRepository:** Specialized content operations (future)
- **UserRepository:** User-specific operations (future)

##### 3.1.4 Data Context
- **ApplicationDbContext:** EF Core database context
- **SeedData:** Database initialization and seeding

#### 3.2 Frontend Components

##### 3.2.1 Pages
- **Home:** Landing page with featured content
- **ContentDetail:** Detailed view of single content item
- **Category:** Category-specific content listings
- **Search:** Search results page
- **Profile:** User profile management

##### 3.2.2 Components
- **Navbar:** Navigation menu
- **ContentCard:** Content display card
- **CommentSection:** Comments display and input
- **Filter:** Search and filter controls
- **Modal:** Reusable modal dialogs

##### 3.2.3 Services
- **api.js:** Axios-based API client
- **auth.js:** Authentication utilities
- **storage.js:** Local storage management

### 4. Data Model

#### 4.1 Entity Relationship Diagram

```
User ──┬──> Comment ──> Content
       │
       └──> UserFavorite ──> Content
       
Content ──┬──> ContentCategory ──> Category
          │
          ├──> ContentActor ──> Actor
          │
          └──> ContentTranslation

Category ──> Category (parent-child)
Comment ──> Comment (parent-child replies)
```

#### 4.2 Key Entities

##### User
- Id, Username, Email, PasswordHash
- FirstName, LastName, ProfileImageUrl
- PreferredLanguage, IsActive
- CreatedAt, UpdatedAt, IsDeleted

##### Content
- Id, Title, OriginalTitle, Description, Slug
- Type (Movie/Series/Game), Status
- Language, ReleaseDate, Rating, Duration
- Director, Platform, SourceUrl
- PosterImageUrl, TrailerUrl
- IsOriginalContent, TicketmasterEventId

##### Category
- Id, Name, Slug, Description
- ParentCategoryId (self-referencing)

##### Actor
- Id, Name, Slug, Biography
- BirthDate, Nationality, ProfileImageUrl

##### Comment
- Id, UserId, ContentId, ParentCommentId
- Text, IsApproved

##### ContentTranslation
- Id, ContentId, Language
- Title, Description

### 5. API Design

#### 5.1 RESTful Endpoints

**Content Endpoints**
```
GET    /api/content              - List all content (with filters)
GET    /api/content/{id}         - Get content by ID
GET    /api/content/slug/{slug}  - Get content by slug
POST   /api/content              - Create new content
PUT    /api/content/{id}         - Update content
DELETE /api/content/{id}         - Delete content
POST   /api/content/crawl        - Crawl content from source
POST   /api/content/{id}/enhance - Enhance content with AI
```

**Category Endpoints**
```
GET    /api/category             - List all categories
GET    /api/category/{id}        - Get category by ID
GET    /api/category/slug/{slug} - Get category by slug
POST   /api/category             - Create category
```

**Comment Endpoints**
```
GET    /api/comment/content/{id} - Get comments for content
POST   /api/comment              - Create comment
PUT    /api/comment/{id}/approve - Approve comment
DELETE /api/comment/{id}         - Delete comment
```

#### 5.2 Request/Response Format

**Standard Success Response**
```json
{
  "data": { ... },
  "message": "Success",
  "statusCode": 200
}
```

**Standard Error Response**
```json
{
  "error": "Error message",
  "statusCode": 400,
  "details": [...]
}
```

### 6. Security Design

#### 6.1 Authentication Flow

```
1. User submits credentials
2. API validates credentials
3. Generate JWT token
4. Return token to client
5. Client stores token
6. Client includes token in subsequent requests
7. API validates token on each request
```

#### 6.2 Authorization

- **Anonymous:** Read-only access to published content
- **Authenticated:** All anonymous + commenting + favorites
- **Admin:** Full access to all operations

#### 6.3 Security Measures

- HTTPS only in production
- JWT token with expiration
- Password hashing with BCrypt (work factor: 12)
- SQL injection prevention (EF Core parameterized queries)
- XSS protection (React escaping, Content Security Policy)
- CORS configuration
- Rate limiting (future)
- Input validation

### 7. Database Design

#### 7.1 Indexes

```sql
CREATE INDEX IX_User_Email ON Users(Email);
CREATE INDEX IX_User_Username ON Users(Username);
CREATE INDEX IX_Content_Slug ON Contents(Slug);
CREATE INDEX IX_Content_Type ON Contents(Type);
CREATE INDEX IX_Content_Status ON Contents(Status);
CREATE INDEX IX_Category_Slug ON Categories(Slug);
CREATE INDEX IX_Actor_Slug ON Actors(Slug);
```

#### 7.2 Relationships

- One-to-Many: User → Comments, Content → Comments
- Many-to-Many: Content ↔ Categories, Content ↔ Actors
- Self-Referencing: Category → Category, Comment → Comment

#### 7.3 Soft Delete

Soft delete implemented for main entities:
- User, Content, Category, Actor, Comment
- Uses `IsDeleted` flag and query filters

### 8. External Integration Design

#### 8.1 Web Crawling

**Flow:**
```
1. Receive crawl request with source URL
2. Fetch HTML content
3. Parse with HtmlAgilityPack
4. Extract relevant data
5. Map to Content entity
6. Store in database (draft status)
7. Return crawled items
```

**Error Handling:**
- Network timeouts
- Invalid HTML structure
- Missing required fields
- Duplicate content detection

#### 8.2 AI Enhancement (OpenAI/Gemini)

**Flow:**
```
1. Receive enhancement request
2. Select AI provider
3. Prepare prompt with content
4. Call AI API
5. Process response
6. Return enhanced content
```

**Considerations:**
- API rate limits
- Token usage optimization
- Fallback to alternative provider
- Cost tracking

#### 8.3 Ticketmaster Integration

**Flow:**
```
1. Search events by keyword
2. Filter by location (optional)
3. Retrieve event details
4. Generate purchase URL
5. Display to user
```

**Data Cached:**
- Event details (1 hour)
- Search results (30 minutes)

### 9. Performance Optimization

#### 9.1 Backend
- Async/await throughout
- Database connection pooling
- Query optimization with includes
- Pagination for large datasets
- Response caching (future)

#### 9.2 Frontend
- Code splitting
- Lazy loading components
- Image optimization
- Debounced search
- Virtual scrolling for long lists (future)

#### 9.3 Database
- Proper indexing
- Query execution plan analysis
- Stored procedures for complex queries (future)
- Read replicas for scaling (future)

### 10. Deployment Architecture

#### 10.1 Development Environment
```
Developer Machine
├── React Dev Server (port 5173)
├── .NET API (port 7215)
└── SQL Server LocalDB
```

#### 10.2 Production Environment (Proposed)
```
Load Balancer
├── Web Server 1 (React SPA)
├── Web Server 2 (React SPA)
│
API Servers (Behind Load Balancer)
├── API Server 1
├── API Server 2
│
Database Cluster
├── Primary SQL Server
└── Read Replica(s)
```

### 11. Error Handling

#### 11.1 Backend Error Handling
- Global exception handler middleware
- Specific exception types
- Logging with correlation IDs
- User-friendly error messages

#### 11.2 Frontend Error Handling
- Try-catch for async operations
- Error boundary components
- Toast notifications for errors
- Retry mechanisms

### 12. Monitoring & Logging

#### 12.1 Application Logs
- Request/response logging
- Error logging
- Performance metrics
- User activity tracking

#### 12.2 Metrics to Monitor
- API response times
- Database query performance
- Error rates
- User engagement
- System resources (CPU, memory)

### 13. Testing Strategy

#### 13.1 Unit Tests
- Business logic
- Service methods
- Utility functions
- Target: >70% coverage

#### 13.2 Integration Tests
- API endpoints
- Database operations
- External service mocks

#### 13.3 End-to-End Tests
- Critical user workflows
- Authentication flow
- Content browsing
- Comment submission

### 14. Future Enhancements

#### Technical Improvements
- GraphQL API layer
- Real-time updates with SignalR
- Elasticsearch for advanced search
- Redis for distributed caching
- Message queue for async operations

#### Feature Additions
- Mobile apps (React Native)
- Admin dashboard
- Analytics engine
- Recommendation system
- Social features
