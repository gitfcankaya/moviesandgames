# API Documentation
## Movies and Games Platform

Base URL: `https://localhost:7215/api` (Development)

### Authentication

Most endpoints require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

---

## Content Endpoints

### Get All Content

Retrieve a list of all content items with optional filtering.

**Endpoint:** `GET /api/content`

**Query Parameters:**
- `type` (optional): Filter by content type (1=Movie, 2=Series, 3=Game)
- `language` (optional): Filter by language code (e.g., "en", "tr")

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "The Matrix",
    "originalTitle": "The Matrix",
    "description": "A computer hacker learns from mysterious rebels...",
    "slug": "the-matrix",
    "type": 1,
    "status": 2,
    "language": "en",
    "releaseDate": "1999-03-31T00:00:00Z",
    "posterImageUrl": null,
    "trailerUrl": null,
    "rating": 8.7,
    "duration": 136,
    "director": "The Wachowskis",
    "platform": "Warner Bros",
    "sourceUrl": null,
    "isOriginalContent": false,
    "ticketmasterEventId": null,
    "createdAt": "2025-11-01T08:00:00Z",
    "updatedAt": null,
    "isDeleted": false
  }
]
```

### Get Content by ID

Retrieve detailed information about a specific content item.

**Endpoint:** `GET /api/content/{id}`

**Parameters:**
- `id` (path): Content ID

**Response:** `200 OK`
```json
{
  "id": 1,
  "title": "The Matrix",
  "originalTitle": "The Matrix",
  "description": "A computer hacker learns from mysterious rebels...",
  "slug": "the-matrix",
  "type": 1,
  "status": 2,
  "language": "en",
  "releaseDate": "1999-03-31T00:00:00Z",
  "rating": 8.7,
  "duration": 136,
  "director": "The Wachowskis",
  "platform": "Warner Bros"
}
```

**Error Responses:**
- `404 Not Found`: Content not found

### Get Content by Slug

Retrieve content using its URL-friendly slug.

**Endpoint:** `GET /api/content/slug/{slug}`

**Parameters:**
- `slug` (path): Content slug (e.g., "the-matrix")

**Response:** `200 OK` (same as Get by ID)

**Error Responses:**
- `404 Not Found`: Content not found

### Create Content

Create a new content item.

**Endpoint:** `POST /api/content`

**Authorization:** Required

**Request Body:**
```json
{
  "title": "Inception",
  "originalTitle": "Inception",
  "description": "A thief who steals corporate secrets...",
  "slug": "inception",
  "type": 1,
  "status": 2,
  "language": "en",
  "releaseDate": "2010-07-16",
  "rating": 8.8,
  "duration": 148,
  "director": "Christopher Nolan",
  "platform": "Warner Bros"
}
```

**Response:** `201 Created`
```json
{
  "id": 7,
  "title": "Inception",
  ...
}
```

**Error Responses:**
- `400 Bad Request`: Invalid data
- `401 Unauthorized`: Missing or invalid token

### Update Content

Update an existing content item.

**Endpoint:** `PUT /api/content/{id}`

**Authorization:** Required

**Parameters:**
- `id` (path): Content ID

**Request Body:** (same as Create)

**Response:** `204 No Content`

**Error Responses:**
- `400 Bad Request`: Invalid data or ID mismatch
- `404 Not Found`: Content not found
- `401 Unauthorized`: Missing or invalid token

### Delete Content

Soft delete a content item.

**Endpoint:** `DELETE /api/content/{id}`

**Authorization:** Required

**Parameters:**
- `id` (path): Content ID

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found`: Content not found
- `401 Unauthorized`: Missing or invalid token

### Crawl Content

Trigger web crawling for content aggregation.

**Endpoint:** `POST /api/content/crawl`

**Authorization:** Required

**Query Parameters:**
- `source` (required): Source URL to crawl
- `type` (required): Content type (1=Movie, 2=Series, 3=Game)
- `count` (optional, default=10): Number of items to crawl

**Response:** `200 OK`
```json
[
  {
    "title": "Extracted Movie Title",
    "originalTitle": "Extracted Movie Title",
    "description": "Extracted description...",
    "type": 1,
    "status": 1,
    "slug": "extracted-movie-title"
  }
]
```

**Error Responses:**
- `400 Bad Request`: Invalid parameters
- `401 Unauthorized`: Missing or invalid token

### Enhance Content

Use AI to enhance content description.

**Endpoint:** `POST /api/content/{id}/enhance`

**Authorization:** Required

**Parameters:**
- `id` (path): Content ID

**Query Parameters:**
- `provider` (optional, default="openai"): AI provider ("openai" or "gemini")

**Response:** `200 OK`
```json
{
  "enhancedDescription": "Enhanced description with AI improvements..."
}
```

**Error Responses:**
- `404 Not Found`: Content not found
- `401 Unauthorized`: Missing or invalid token

---

## Category Endpoints

### Get All Categories

Retrieve all categories.

**Endpoint:** `GET /api/category`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Action",
    "slug": "action",
    "description": "Action movies, series and games",
    "parentCategoryId": null,
    "createdAt": "2025-11-01T08:00:00Z"
  }
]
```

### Get Category by ID

Retrieve a specific category.

**Endpoint:** `GET /api/category/{id}`

**Parameters:**
- `id` (path): Category ID

**Response:** `200 OK` (same as list item)

**Error Responses:**
- `404 Not Found`: Category not found

### Get Category by Slug

Retrieve category by slug.

**Endpoint:** `GET /api/category/slug/{slug}`

**Parameters:**
- `slug` (path): Category slug

**Response:** `200 OK` (same as list item)

**Error Responses:**
- `404 Not Found`: Category not found

### Create Category

Create a new category.

**Endpoint:** `POST /api/category`

**Authorization:** Required

**Request Body:**
```json
{
  "name": "Mystery",
  "slug": "mystery",
  "description": "Mystery content",
  "parentCategoryId": null
}
```

**Response:** `201 Created`

**Error Responses:**
- `400 Bad Request`: Invalid data
- `401 Unauthorized`: Missing or invalid token

---

## Comment Endpoints

### Get Comments for Content

Retrieve all approved comments for a content item.

**Endpoint:** `GET /api/comment/content/{contentId}`

**Parameters:**
- `contentId` (path): Content ID

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "userId": 1,
    "contentId": 1,
    "parentCommentId": null,
    "text": "Great movie!",
    "isApproved": true,
    "createdAt": "2025-11-01T10:00:00Z"
  }
]
```

### Create Comment

Post a comment on content.

**Endpoint:** `POST /api/comment`

**Authorization:** Required

**Request Body:**
```json
{
  "userId": 1,
  "contentId": 1,
  "parentCommentId": null,
  "text": "This is an amazing movie!"
}
```

**Response:** `201 Created`
```json
{
  "id": 2,
  "userId": 1,
  "contentId": 1,
  "text": "This is an amazing movie!",
  "isApproved": false,
  "createdAt": "2025-11-01T12:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request`: Invalid data
- `401 Unauthorized`: Missing or invalid token

### Approve Comment

Approve a pending comment (Admin only).

**Endpoint:** `PUT /api/comment/{id}/approve`

**Authorization:** Required (Admin)

**Parameters:**
- `id` (path): Comment ID

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found`: Comment not found
- `401 Unauthorized`: Missing or invalid token
- `403 Forbidden`: Insufficient permissions

### Delete Comment

Delete a comment.

**Endpoint:** `DELETE /api/comment/{id}`

**Authorization:** Required

**Parameters:**
- `id` (path): Comment ID

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found`: Comment not found
- `401 Unauthorized`: Missing or invalid token

---

## Error Responses

All endpoints may return these error responses:

### 400 Bad Request
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["The Title field is required."]
  }
}
```

### 401 Unauthorized
```json
{
  "error": "Unauthorized",
  "message": "Invalid or missing authentication token"
}
```

### 404 Not Found
```json
{
  "error": "Not Found",
  "message": "The requested resource was not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "Internal Server Error",
  "message": "An unexpected error occurred"
}
```

---

## Data Models

### Content Types (Enum)
- `1` - Movie
- `2` - Series
- `3` - Game

### Content Status (Enum)
- `1` - Draft
- `2` - Published
- `3` - Archived

### Content Model
```typescript
interface Content {
  id: number;
  title: string;
  originalTitle: string;
  description: string;
  slug: string;
  type: ContentType;
  status: ContentStatus;
  language: string;
  releaseDate?: Date;
  posterImageUrl?: string;
  trailerUrl?: string;
  rating?: number;
  duration?: number;
  director?: string;
  platform?: string;
  sourceUrl?: string;
  isOriginalContent: boolean;
  ticketmasterEventId?: string;
  createdAt: Date;
  updatedAt?: Date;
  isDeleted: boolean;
}
```

### Category Model
```typescript
interface Category {
  id: number;
  name: string;
  slug: string;
  description: string;
  parentCategoryId?: number;
  createdAt: Date;
  updatedAt?: Date;
  isDeleted: boolean;
}
```

### Comment Model
```typescript
interface Comment {
  id: number;
  userId: number;
  contentId: number;
  parentCommentId?: number;
  text: string;
  isApproved: boolean;
  createdAt: Date;
  updatedAt?: Date;
  isDeleted: boolean;
}
```

---

## Rate Limiting

Currently not implemented. Future implementation will enforce:
- 100 requests per minute per IP for anonymous users
- 1000 requests per minute for authenticated users
- Special limits for crawling endpoints

---

## Pagination

Not yet implemented. Will be added in future versions with query parameters:
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 20, max: 100)

---

## Versioning

API version: v1 (Current)

Future versions will be accessible via:
- URL: `/api/v2/content`
- Header: `Accept: application/vnd.moviesandgames.v2+json`

---

## Swagger Documentation

Interactive API documentation available at:
```
https://localhost:7215/swagger
```

Features:
- Try out endpoints directly
- View request/response schemas
- Authentication support
- Error code documentation
