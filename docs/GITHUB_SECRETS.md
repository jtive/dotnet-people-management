# GitHub Secrets Configuration

This document outlines the required GitHub secrets for deploying the Personal Information Management System.

## Required Secrets

### AWS Credentials
- `AWS_ACCESS_KEY_ID` - AWS access key for deployment
- `AWS_SECRET_ACCESS_KEY` - AWS secret access key for deployment

### Database Connection
- `DB_HOST` - PostgreSQL database host (e.g., `show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com`)
- `DB_NAME` - Database name (e.g., `personalinfo` for production, `personalinfo_dev` for development)
- `DB_USERNAME` - Database username (e.g., `postgres`)
- `DB_PASSWORD` - Database password
- `DB_PORT` - Database port (e.g., `5432`)

## Setting Up Secrets in GitHub

1. Go to your repository on GitHub
2. Click on **Settings** tab
3. In the left sidebar, click **Secrets and variables** → **Actions**
4. Click **New repository secret**
5. Add each secret with the exact name and value listed above

## Environment-Specific Configuration

### Staging Environment
- `DB_NAME`: `personalinfo_staging`

### Production Environment
- `DB_NAME`: `personalinfo`

## Security Best Practices

1. **Never commit secrets to code** - All sensitive information should be stored in GitHub secrets
2. **Use environment-specific databases** - Separate databases for staging and production
3. **Rotate credentials regularly** - Update database passwords periodically
4. **Limit AWS permissions** - Use IAM roles with minimal required permissions
5. **Enable database encryption** - Ensure PostgreSQL is configured with SSL/TLS

## Local Development

For local development, you can set environment variables in your IDE or use a `.env` file (not committed to git):

```bash
# .env file (DO NOT COMMIT TO GIT)
DB_HOST=show-tell.czgcu8i084mj.us-east-2.rds.amazonaws.com
DB_NAME=personalinfo_dev
DB_USERNAME=postgres
DB_PASSWORD=your-password-here
DB_PORT=5432
```

## Connection String Format

The application automatically builds connection strings from environment variables:

```
Host={DB_HOST};Port={DB_PORT};Database={DB_NAME};Username={DB_USERNAME};Password={DB_PASSWORD};SSL Mode=Require;
```

## Troubleshooting

### Common Issues

1. **Connection timeout**: Check if the database host is accessible from your deployment environment
2. **Authentication failed**: Verify username and password are correct
3. **Database not found**: Ensure the database exists and the name is correct
4. **SSL connection required**: Make sure `SSL Mode=Require` is included in the connection string

### Testing Connection

You can test the database connection locally by setting the environment variables and running:

```bash
dotnet run --project src/PersonalInfoApi
```

The application will attempt to connect to the database on startup and create the schema if it doesn't exist.

## Database Setup

### Prerequisites
**IMPORTANT**: You must create the database manually before running the application. Entity Framework can create tables, but not the database itself.

### Step 1: Create the Database
Connect to your PostgreSQL server and create the database:

```sql
-- Connect to PostgreSQL as postgres user
-- Then run:
CREATE DATABASE personalinfo;
CREATE DATABASE personalinfo_dev;
CREATE DATABASE personalinfo_staging;
```

### Step 2: Application Creates Tables
The application will automatically create the database schema (tables, indexes, etc.) using Entity Framework. Ensure the database user has the following permissions:

- `CREATE` - To create tables
- `ALTER` - To modify table structure
- `INSERT` - To add data
- `UPDATE` - To modify data
- `DELETE` - To remove data
- `SELECT` - To read data
- `INDEX` - To create indexes

## Monitoring

Monitor the application logs for database connection issues:

- **Health endpoint**: `/api/health` - Shows database connection status
- **Readiness probe**: `/api/health/ready` - Indicates if the application is ready to serve requests

## Backup and Recovery

Ensure you have:

1. **Automated backups** configured on your RDS instance
2. **Point-in-time recovery** enabled
3. **Cross-region backup** for disaster recovery
4. **Regular backup testing** to verify recovery procedures
