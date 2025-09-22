# Personal Information Management System - Development Plan

## Project Overview
A .NET 8.0 application for securely managing personal information data including names, birthdates, addresses, SSNs, and credit card information. The system supports both individual users and families with proper data masking and formatting.

## Architecture

### Technology Stack
- **Backend**: .NET 8.0 Web API
- **Frontend**: Blazor Server
- **Database**: AWS RDS PostgreSQL
- **Deployment**: AWS S3 + CloudFront
- **CI/CD**: GitHub Actions
- **ORM**: Entity Framework Core with Npgsql

### Project Structure
```
personal-info-system/
├── src/
│   ├── PersonalInfoApi/              # Web API project
│   ├── PersonalInfoBlazor/           # Blazor Server project
│   └── PersonalInfoShared/           # Shared models and utilities
├── infrastructure/
│   ├── terraform/                    # Infrastructure as Code
│   └── docker/                       # Docker configurations
├── docs/                            # Documentation
├── .github/workflows/               # GitHub Actions
└── scripts/                         # Deployment scripts
```

## Database Design

### Core Tables

#### 1. Families Table
```sql
CREATE TABLE Families (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FamilyName VARCHAR(100) NOT NULL,
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UpdatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 2. Persons Table
```sql
CREATE TABLE Persons (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FamilyId UUID REFERENCES Families(Id),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    BirthDate DATE,
    SSN VARCHAR(11), -- Encrypted
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UpdatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    
    -- For single person: Primary Key = GUID + LastName
    -- For family members: Linked via FamilyId
    CONSTRAINT chk_single_or_family CHECK (
        (FamilyId IS NULL AND Id IS NOT NULL) OR 
        (FamilyId IS NOT NULL)
    )
);
```

#### 3. Addresses Table
```sql
CREATE TABLE Addresses (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    PersonId UUID REFERENCES Persons(Id) ON DELETE CASCADE,
    AddressType VARCHAR(20) NOT NULL, -- 'Home', 'Work', 'Mailing'
    StreetAddress VARCHAR(200) NOT NULL,
    City VARCHAR(100) NOT NULL,
    State VARCHAR(2) NOT NULL,
    ZipCode VARCHAR(10) NOT NULL,
    Country VARCHAR(2) DEFAULT 'US',
    IsPrimary BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UpdatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 4. CreditCards Table
```sql
CREATE TABLE CreditCards (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    PersonId UUID REFERENCES Persons(Id) ON DELETE CASCADE,
    CardType VARCHAR(20) NOT NULL, -- 'Visa', 'MasterCard', 'Amex', etc.
    LastFourDigits VARCHAR(4) NOT NULL,
    ExpirationMonth INTEGER NOT NULL,
    ExpirationYear INTEGER NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UpdatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

## Data Masking & Security

### Masking Strategy
- **Names**: Show only FirstName and LastName
- **SSN**: Display as XXX-XX-XXXX
- **Addresses**: Show as ********
- **Credit Cards**: Show as ****-****-****-1234
- **Birth Dates**: Show as ********

### Security Implementation
1. **Encryption at Rest**: All sensitive data encrypted in PostgreSQL
2. **Encryption in Transit**: HTTPS/TLS for all communications
3. **Data Masking**: Runtime masking for display purposes
4. **Access Control**: Role-based access control
5. **Audit Logging**: Track all data access and modifications

## API Endpoints

### Person Management
- `GET /api/persons` - List all persons (masked data)
- `GET /api/persons/{id}` - Get person details (masked data)
- `POST /api/persons` - Create new person
- `PUT /api/persons/{id}` - Update person
- `DELETE /api/persons/{id}` - Delete person

### Family Management
- `GET /api/families` - List all families
- `GET /api/families/{id}/members` - Get family members
- `POST /api/families` - Create new family
- `POST /api/families/{id}/members` - Add member to family

### Address Management
- `GET /api/persons/{id}/addresses` - Get person addresses (masked)
- `POST /api/persons/{id}/addresses` - Add address
- `PUT /api/addresses/{id}` - Update address

### Credit Card Management
- `GET /api/persons/{id}/creditcards` - Get credit cards (masked)
- `POST /api/persons/{id}/creditcards` - Add credit card
- `PUT /api/creditcards/{id}` - Update credit card

## Blazor Frontend Features

### Pages
1. **Dashboard** - Overview of all persons/families
2. **Person Details** - View/edit individual person data
3. **Family Management** - Manage family groups
4. **Add Person** - Form to add new person
5. **Add Family** - Form to create new family

### Components
1. **MaskedInput** - Input component with real-time masking
2. **PersonCard** - Display person information with masking
3. **FamilyTree** - Visual family relationship display
4. **DataTable** - Table with masked data display

### Real-time Masking Examples
- **SSN Input**: User types "123456789" → Displays "123-45-6789"
- **Credit Card**: User types "4111111111111111" → Displays "4111-1111-1111-1111"
- **Address Display**: Shows "123 Main St" → Displays "********"

## Deployment Architecture

### AWS Infrastructure
```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   CloudFront    │────│   S3 Bucket      │    │   API Gateway   │
│   Distribution  │    │   (Static Files) │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
                                                         │
                       ┌──────────────────┐             │
                       │   Application    │─────────────┤
                       │   Load Balancer  │             │
                       └──────────────────┘             │
                                 │                      │
                       ┌──────────────────┐             │
                       │   ECS/Fargate    │─────────────┘
                       │   (.NET API)     │
                       └──────────────────┘
                                 │
                       ┌──────────────────┐
                       │   RDS PostgreSQL │
                       │   (Encrypted)    │
                       └──────────────────┘
```

### GitHub Actions Workflow
1. **Test Phase**: Run unit tests, integration tests, security scans
2. **Build Phase**: Build Docker images for API and Blazor app
3. **Deploy Phase**: Deploy to AWS using Terraform
4. **Post-Deploy**: Run smoke tests and health checks

## Security Considerations

### Data Protection
- All PII encrypted at rest using AWS KMS
- Field-level encryption for sensitive data
- Secure key management with AWS Secrets Manager
- Regular security audits and penetration testing

### Compliance
- GDPR compliance for EU data
- CCPA compliance for California residents
- SOC 2 Type II certification preparation
- Regular compliance audits

### Access Control
- Multi-factor authentication
- Role-based access control (RBAC)
- API rate limiting and throttling
- Comprehensive audit logging

## Development Phases

### Phase 1: Foundation (Week 1-2)
- [ ] Set up project structure
- [ ] Configure Entity Framework with PostgreSQL
- [ ] Create basic data models
- [ ] Set up development environment

### Phase 2: Core API (Week 3-4)
- [ ] Implement CRUD operations for persons
- [ ] Add family management endpoints
- [ ] Implement data masking services
- [ ] Add input validation and error handling

### Phase 3: Blazor Frontend (Week 5-6)
- [ ] Create basic Blazor pages
- [ ] Implement masked input components
- [ ] Add person and family management UI
- [ ] Integrate with API endpoints

### Phase 4: Security & Testing (Week 7-8)
- [ ] Implement encryption/decryption
- [ ] Add comprehensive unit tests
- [ ] Security testing and vulnerability assessment
- [ ] Performance optimization

### Phase 5: Deployment (Week 9-10)
- [ ] Set up AWS infrastructure
- [ ] Configure GitHub Actions CI/CD
- [ ] Deploy to staging environment
- [ ] Production deployment and monitoring

## Monitoring & Maintenance

### Logging
- Application logs with structured logging
- API access logs
- Database query logs
- Security event logs

### Monitoring
- Application performance monitoring (APM)
- Database performance monitoring
- Infrastructure monitoring
- Security monitoring and alerting

### Backup & Recovery
- Automated database backups
- Point-in-time recovery capability
- Disaster recovery procedures
- Regular backup testing

## Risk Mitigation

### Technical Risks
- **Database Performance**: Implement proper indexing and query optimization
- **Security Vulnerabilities**: Regular security scans and updates
- **Data Loss**: Comprehensive backup and recovery procedures
- **Scalability**: Load testing and auto-scaling configuration

### Business Risks
- **Compliance Violations**: Regular compliance audits
- **Data Breaches**: Comprehensive security measures and incident response
- **System Downtime**: High availability configuration and monitoring
- **User Experience**: Extensive testing and user feedback integration

## Success Metrics

### Technical Metrics
- API response time < 200ms
- 99.9% uptime
- Zero security vulnerabilities
- 100% test coverage for critical paths

### Business Metrics
- User satisfaction score > 4.5/5
- Data processing accuracy 99.99%
- Compliance audit success rate 100%
- System adoption rate tracking

## Next Steps

1. **Environment Setup**: Configure development environment with all required tools
2. **Database Setup**: Create AWS RDS PostgreSQL instance
3. **Project Initialization**: Create solution structure and initial projects
4. **Model Development**: Implement Entity Framework models
5. **API Development**: Build core API endpoints
6. **Frontend Development**: Create Blazor application
7. **Testing**: Implement comprehensive test suite
8. **Deployment**: Set up CI/CD pipeline and AWS infrastructure
9. **Security Review**: Conduct security assessment and penetration testing
10. **Go-Live**: Deploy to production with monitoring and support procedures
