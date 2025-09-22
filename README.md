# Personal Information Management System

A secure .NET 8.0 application for managing personal information data including names, birthdates, addresses, SSNs, and credit card information. The system supports both individual users and families with comprehensive data masking and formatting.

## 🏗️ Architecture

- **Backend**: .NET 8.0 Web API with Entity Framework Core
- **Frontend**: Blazor Server with real-time data masking
- **Database**: AWS RDS PostgreSQL
- **Deployment**: AWS S3 + CloudFront
- **CI/CD**: GitHub Actions

## 🔒 Security Features

- **Data Masking**: All sensitive data is masked in API responses
- **Encryption**: Sensitive data encrypted at rest
- **Input Validation**: Comprehensive validation on all inputs
- **CORS Configuration**: Properly configured cross-origin policies
- **Health Monitoring**: Built-in health checks and monitoring

## 📊 Data Models

### Person
- Individual person records with family relationships
- Support for single persons (GUID + LastName as primary key)
- Family members linked via FamilyId

### Family
- Family groups with multiple members
- Hierarchical relationship management

### Address
- Multiple addresses per person (Home, Work, Mailing)
- Primary address designation

### Credit Card
- Credit card information with last 4 digits only
- Card type and expiration tracking

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL database (AWS RDS configured)
- Visual Studio 2022 or VS Code

### Database Setup

The application will automatically create the database schema on first run. Ensure your PostgreSQL connection string is configured correctly.

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd personal-info-system
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore src/PersonalInfoSystem.sln
   ```

3. **Run the API**
   ```bash
   cd src/PersonalInfoApi
   dotnet run
   ```
   API will be available at: `https://localhost:7000`

4. **Run the Blazor App**
   ```bash
   cd src/PersonalInfoBlazor
   dotnet run
   ```
   Blazor app will be available at: `https://localhost:5000`

## 📡 API Endpoints

### Person Management
- `GET /api/person` - List all persons (masked data)
- `GET /api/person/{id}` - Get person details (masked data)
- `POST /api/person` - Create new person
- `PUT /api/person/{id}` - Update person
- `DELETE /api/person/{id}` - Delete person
- `GET /api/person/family/{familyId}` - Get persons by family

### Family Management
- `GET /api/family` - List all families
- `GET /api/family/{id}` - Get family details
- `POST /api/family` - Create new family
- `PUT /api/family/{id}` - Update family
- `DELETE /api/family/{id}` - Delete family
- `GET /api/family/{id}/members` - Get family members
- `POST /api/family/{id}/members` - Add member to family

### Address Management
- `GET /api/address/person/{personId}` - Get person addresses (masked)
- `GET /api/address/{id}` - Get address details (masked)
- `POST /api/address/person/{personId}` - Add address
- `PUT /api/address/{id}` - Update address
- `DELETE /api/address/{id}` - Delete address

### Credit Card Management
- `GET /api/creditcard/person/{personId}` - Get credit cards (masked)
- `GET /api/creditcard/{id}` - Get credit card details (masked)
- `POST /api/creditcard/person/{personId}` - Add credit card
- `PUT /api/creditcard/{id}` - Update credit card
- `DELETE /api/creditcard/{id}` - Delete credit card

### Health & Monitoring
- `GET /api/health` - Health check with statistics
- `GET /api/health/ready` - Readiness probe

## 🔐 Data Masking Examples

### SSN Masking
- **Input**: `123456789`
- **Display**: `***-**-****`
- **Formatted Input**: `123-45-6789`

### Address Masking
- **Input**: `123 Main St, Anytown, CA 12345`
- **Display**: `********`

### Credit Card Masking
- **Input**: `4111111111111111`
- **Display**: `****-****-****-1111`
- **Formatted Input**: `4111-1111-1111-1111`

### Birth Date Masking
- **Input**: `1990-01-01`
- **Display**: `********`

## 🛠️ Development

### Entity Framework Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project src/PersonalInfoShared --startup-project src/PersonalInfoApi

# Update database
dotnet ef database update --project src/PersonalInfoShared --startup-project src/PersonalInfoApi
```

### Testing

```bash
# Run all tests
dotnet test src/PersonalInfoSystem.sln

# Run with coverage
dotnet test src/PersonalInfoSystem.sln --collect:"XPlat Code Coverage"
```

### Building for Production

```bash
# Build solution
dotnet build src/PersonalInfoSystem.sln --configuration Release

# Publish API
dotnet publish src/PersonalInfoApi/PersonalInfoApi.csproj --configuration Release --output ./publish/api

# Publish Blazor
dotnet publish src/PersonalInfoBlazor/PersonalInfoBlazor.csproj --configuration Release --output ./publish/blazor
```

## 🚀 Deployment

### AWS Deployment

The application is configured for deployment to AWS using:

1. **S3** - Static website hosting for Blazor app
2. **CloudFront** - CDN for global distribution
3. **ECS** - Container hosting for API
4. **RDS** - PostgreSQL database

### GitHub Actions

The CI/CD pipeline includes:

- **Testing**: Unit tests and security scans
- **Building**: Multi-project build with artifact creation
- **Deployment**: Staging and production deployments
- **Monitoring**: Health checks and smoke tests

### Environment Variables

Required GitHub Secrets:
- `AWS_ACCESS_KEY_ID` - AWS access key for deployment
- `AWS_SECRET_ACCESS_KEY` - AWS secret access key for deployment
- `DB_HOST` - PostgreSQL database host
- `DB_NAME` - Database name
- `DB_USERNAME` - Database username
- `DB_PASSWORD` - Database password
- `DB_PORT` - Database port

**See [GitHub Secrets Configuration](docs/GITHUB_SECRETS.md) for detailed setup instructions.**

## 📝 Configuration

### Database Connection

The application uses environment variables for database configuration. Set the following environment variables:

```bash
DB_HOST=your-host
DB_NAME=your-database-name
DB_USERNAME=your-username
DB_PASSWORD=your-password
DB_PORT=5432
```

For local development, you can create a `.env` file (do not commit to git):

```bash
# .env file (DO NOT COMMIT TO GIT)
DB_HOST=show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com
DB_NAME=personalinfo_dev
DB_USERNAME=postgres
DB_PASSWORD=your-password-here
DB_PORT=5432
```

The connection string is automatically built from these environment variables with SSL Mode=Require.

### CORS Configuration

Configure allowed origins in `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("https://your-domain.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

## 🔍 Monitoring

### Health Checks

- **Health Endpoint**: `/api/health`
- **Readiness Probe**: `/api/health/ready`

### Logging

Structured logging with different levels:
- **Information**: General application flow
- **Warning**: Non-critical issues
- **Error**: Application errors
- **Debug**: Detailed debugging information (development only)

## 🛡️ Security Considerations

### Data Protection
- All PII encrypted at rest
- Secure connection strings
- Input validation and sanitization
- SQL injection prevention via EF Core

### Access Control
- CORS configuration
- API rate limiting (recommended)
- Authentication/authorization (to be implemented)

### Compliance
- GDPR compliance considerations
- Data retention policies
- Audit logging capabilities

## 📚 API Documentation

Swagger/OpenAPI documentation is available at:
- Development: `https://localhost:7000/swagger`
- Production: `https://api.personal-info.example.com/swagger`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Check the documentation
- Review the API endpoints

## 🔄 Version History

- **v1.0.0** - Initial release with core functionality
- Basic CRUD operations for persons, families, addresses, and credit cards
- Data masking and formatting
- AWS deployment configuration
- GitHub Actions CI/CD pipeline
