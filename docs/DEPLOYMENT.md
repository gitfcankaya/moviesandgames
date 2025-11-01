# Deployment Guide
## Movies and Games Platform

### Prerequisites

#### Development Environment
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server Express
- IDE: Visual Studio 2022, VS Code, or JetBrains Rider

#### Production Environment
- Windows Server 2019+ or Linux Server
- .NET 9.0 Runtime
- SQL Server (any edition)
- Node.js for building frontend
- Web Server (IIS or Nginx)

### Environment Configuration

#### 1. Development Setup

**Backend Configuration** (`src/MoviesAndGames.API/appsettings.Development.json`):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MoviesAndGamesDB_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**Frontend Configuration** (`client/.env`):
```
VITE_API_URL=https://localhost:7215/api
```

#### 2. Staging Setup

**Backend Configuration** (`src/MoviesAndGames.API/appsettings.Staging.json`):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=staging-server;Database=MoviesAndGamesDB_Staging;User Id=staging_user;Password=***;Encrypt=true;TrustServerCertificate=false"
  },
  "Jwt": {
    "Key": "your-staging-secret-key-at-least-32-characters-long",
    "Issuer": "MoviesAndGamesAPI",
    "Audience": "MoviesAndGamesClient",
    "ExpiryInMinutes": 60
  }
}
```

**Frontend Configuration** (`client/.env.staging`):
```
VITE_API_URL=https://staging.yourdomain.com/api
```

#### 3. Production Setup

**Backend Configuration** (`src/MoviesAndGames.API/appsettings.Production.json`):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Use environment variable or Azure Key Vault"
  },
  "Jwt": {
    "Key": "Use environment variable or Azure Key Vault",
    "Issuer": "MoviesAndGamesAPI",
    "Audience": "MoviesAndGamesClient",
    "ExpiryInMinutes": 60
  },
  "OpenAI": {
    "ApiKey": "Use environment variable or Azure Key Vault"
  },
  "Gemini": {
    "ApiKey": "Use environment variable or Azure Key Vault"
  },
  "Ticketmaster": {
    "ApiKey": "Use environment variable or Azure Key Vault"
  }
}
```

**Frontend Configuration** (`client/.env.production`):
```
VITE_API_URL=https://api.yourdomain.com/api
```

### Database Setup

#### 1. Create Initial Migration

```bash
cd src/MoviesAndGames.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../MoviesAndGames.API
```

#### 2. Apply Migration to Development Database

```bash
cd src/MoviesAndGames.API
dotnet ef database update --project ../MoviesAndGames.Infrastructure
```

#### 3. Generate SQL Scripts for Production

```bash
cd src/MoviesAndGames.Infrastructure
dotnet ef migrations script --startup-project ../MoviesAndGames.API --output migration.sql
```

#### 4. Manual Database Setup (Production)

For production, run the generated SQL script manually:
```sql
-- Connect to your production SQL Server
-- Run the migration.sql script
```

### Building the Application

#### Backend Build

**Development:**
```bash
dotnet build
```

**Release:**
```bash
dotnet build --configuration Release
```

**Publish:**
```bash
cd src/MoviesAndGames.API
dotnet publish --configuration Release --output ./publish
```

#### Frontend Build

**Development:**
```bash
cd client
npm run dev
```

**Production Build:**
```bash
cd client
npm run build
```

This creates optimized files in `client/dist/`

### Deployment Strategies

#### Option 1: IIS Deployment (Windows)

1. **Publish Backend:**
```bash
cd src/MoviesAndGames.API
dotnet publish -c Release -o ./publish
```

2. **Install .NET Hosting Bundle:**
   - Download from [Microsoft](https://dotnet.microsoft.com/download/dotnet/9.0)

3. **Create IIS Site:**
   - Open IIS Manager
   - Add New Website
   - Point to publish folder
   - Set application pool to "No Managed Code"
   - Configure bindings (HTTPS recommended)

4. **Deploy Frontend:**
   - Build React app: `npm run build`
   - Copy `dist/` contents to IIS website folder
   - Configure URL Rewrite for SPA routing

**web.config for React SPA:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="React Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
            <add input="{REQUEST_URI}" pattern="^/(api)" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

#### Option 2: Docker Deployment

**Backend Dockerfile** (`src/MoviesAndGames.API/Dockerfile`):
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/MoviesAndGames.API/MoviesAndGames.API.csproj", "src/MoviesAndGames.API/"]
COPY ["src/MoviesAndGames.Core/MoviesAndGames.Core.csproj", "src/MoviesAndGames.Core/"]
COPY ["src/MoviesAndGames.Infrastructure/MoviesAndGames.Infrastructure.csproj", "src/MoviesAndGames.Infrastructure/"]
RUN dotnet restore "src/MoviesAndGames.API/MoviesAndGames.API.csproj"
COPY . .
WORKDIR "/src/src/MoviesAndGames.API"
RUN dotnet build "MoviesAndGames.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoviesAndGames.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoviesAndGames.API.dll"]
```

**Frontend Dockerfile** (`client/Dockerfile`):
```dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**Docker Compose** (`docker-compose.yml`):
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

  api:
    build:
      context: .
      dockerfile: src/MoviesAndGames.API/Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=MoviesAndGamesDB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=true
    depends_on:
      - sqlserver

  web:
    build:
      context: ./client
      dockerfile: Dockerfile
    ports:
      - "3000:80"
    depends_on:
      - api

volumes:
  sqldata:
```

**Build and Run:**
```bash
docker-compose up -d
```

#### Option 3: Azure Deployment

**Azure App Service (Backend):**
```bash
# Login to Azure
az login

# Create resource group
az group create --name MoviesAndGamesRG --location eastus

# Create App Service plan
az appservice plan create --name MoviesAndGamesPlan --resource-group MoviesAndGamesRG --sku B1 --is-linux

# Create web app
az webapp create --resource-group MoviesAndGamesRG --plan MoviesAndGamesPlan --name moviesandgames-api --runtime "DOTNETCORE|9.0"

# Deploy
cd src/MoviesAndGames.API
az webapp up --name moviesandgames-api --resource-group MoviesAndGamesRG
```

**Azure SQL Database:**
```bash
# Create SQL Server
az sql server create --name moviesandgames-sql --resource-group MoviesAndGamesRG --location eastus --admin-user sqladmin --admin-password YourPassword123!

# Create database
az sql db create --resource-group MoviesAndGamesRG --server moviesandgames-sql --name MoviesAndGamesDB --service-objective S0
```

**Azure Static Web Apps (Frontend):**
```bash
# Install SWA CLI
npm install -g @azure/static-web-apps-cli

# Deploy
cd client
swa deploy --app-location . --api-location ../src/MoviesAndGames.API --output-location dist
```

### Post-Deployment Steps

#### 1. Verify API

```bash
curl https://your-api-url/swagger/index.html
curl https://your-api-url/api/content
```

#### 2. Database Migration

Run migrations on production:
```bash
dotnet ef database update --project src/MoviesAndGames.Infrastructure --startup-project src/MoviesAndGames.API --connection "your-prod-connection-string"
```

Or use SQL script:
```bash
sqlcmd -S your-server -d MoviesAndGamesDB -U username -P password -i migration.sql
```

#### 3. Seed Initial Data

The application automatically seeds data on first run. To manually seed:
```bash
# In API startup, SeedData.Initialize is called
```

#### 4. SSL/TLS Configuration

**For IIS:**
- Obtain SSL certificate (Let's Encrypt, Azure, or commercial)
- Install certificate in IIS
- Configure HTTPS binding

**For Nginx:**
```nginx
server {
    listen 443 ssl;
    server_name yourdomain.com;
    
    ssl_certificate /path/to/certificate.crt;
    ssl_certificate_key /path/to/private.key;
    
    location / {
        root /var/www/html;
        try_files $uri $uri/ /index.html;
    }
    
    location /api {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### Monitoring & Logging

#### Application Insights (Azure)

Add to `Program.cs`:
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

#### Serilog Setup

Install packages:
```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
```

Configure in `Program.cs`:
```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
```

### Backup & Recovery

#### Database Backup

**Automated SQL Server Backup:**
```sql
BACKUP DATABASE MoviesAndGamesDB
TO DISK = 'C:\Backups\MoviesAndGamesDB.bak'
WITH FORMAT;
```

**Azure SQL Backup:**
Automatic backups enabled by default. Configure retention:
```bash
az sql db long-term-retention-policy set --resource-group MoviesAndGamesRG --server moviesandgames-sql --database MoviesAndGamesDB --weekly-retention P4W
```

### Scaling Considerations

#### Horizontal Scaling
- Deploy multiple API instances behind load balancer
- Use distributed caching (Redis)
- Configure session state externally

#### Database Scaling
- Implement read replicas
- Use connection pooling
- Consider Azure SQL elastic pools

### Security Checklist

- [ ] HTTPS enforced
- [ ] Strong JWT secret in production
- [ ] API keys in Azure Key Vault
- [ ] CORS properly configured
- [ ] SQL injection prevention verified
- [ ] Rate limiting implemented
- [ ] Input validation on all endpoints
- [ ] Security headers configured
- [ ] Regular security updates
- [ ] Firewall rules configured

### Troubleshooting

#### Common Issues

**API doesn't start:**
- Check .NET runtime is installed
- Verify connection string
- Check application logs

**Database connection fails:**
- Verify SQL Server is running
- Check firewall rules
- Validate credentials

**Frontend can't reach API:**
- Verify CORS settings
- Check API URL in .env
- Ensure API is accessible

**502 Bad Gateway:**
- Check application pool is running (IIS)
- Verify .NET runtime version
- Check application logs

### Performance Tuning

- Enable response compression
- Implement caching headers
- Use CDN for static assets
- Optimize database queries
- Enable connection pooling
- Configure application pool settings

### Maintenance

#### Regular Tasks
- Database maintenance (index rebuild, statistics update)
- Log file cleanup
- Certificate renewal
- Security patches
- Dependency updates

#### Monitoring Metrics
- Response times
- Error rates
- Database performance
- Memory usage
- CPU utilization
- Disk space
