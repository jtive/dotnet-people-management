# Configuration Template

## Environment Variables Template

Create a `.env` file in the project root with the following template:

```bash
# Database Configuration
# Copy this file to .env and fill in your actual values
# DO NOT COMMIT .env TO GIT

# PostgreSQL Database Settings
DB_HOST=show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com
DB_NAME=personalinfo_dev
DB_USERNAME=postgres
DB_PASSWORD=your-password-here
DB_PORT=5432

# API Configuration (Optional - defaults are provided)
API_BASE_URL=https://localhost:7000

# Application Settings (Optional)
ASPNETCORE_ENVIRONMENT=Development
```

## GitHub Secrets Template

Add these secrets to your GitHub repository:

### AWS Credentials
- `AWS_ACCESS_KEY_ID` = your-aws-access-key
- `AWS_SECRET_ACCESS_KEY` = your-aws-secret-key

### Database Connection
- `DB_HOST` = show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com
- `DB_NAME` = personalinfo (or personalinfo_staging for staging)
- `DB_USERNAME` = postgres
- `DB_PASSWORD` = your-database-password
- `DB_PORT` = 5432

## Local Development Setup

1. Copy the environment variables template above
2. Create a `.env` file in the project root
3. Fill in your actual database credentials
4. Run the application:

```bash
# Load environment variables and run API
dotnet run --project src/PersonalInfoApi

# In another terminal, run Blazor app
dotnet run --project src/PersonalInfoBlazor
```

## Security Notes

- Never commit `.env` files to git
- Use strong passwords for database access
- Rotate credentials regularly
- Use different databases for development, staging, and production
