# AWS App Runner Setup Guide

## Prerequisites

1. **AWS Account** with appropriate permissions
2. **ECR Repository** for storing Docker images
3. **IAM Role** for App Runner instance

## Step 1: Create ECR Repository

```bash
# Create ECR repository
aws ecr create-repository --repository-name personal-info-system --region us-east-2

# Get login token
aws ecr get-login-password --region us-east-2 | docker login --username AWS --password-stdin YOUR_ACCOUNT_ID.dkr.ecr.us-east-2.amazonaws.com
```

## Step 2: Create IAM Role for App Runner

Create an IAM role with the following trust policy:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "tasks.apprunner.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
```

Attach the following managed policies:
- `AWSAppRunnerServicePolicyForECRAccess`
- Any additional policies for RDS/Secrets Manager if needed

## Step 3: Update Configuration

1. **Update `apprunner-config.json`:**
   - Replace `YOUR_ACCOUNT_ID` with your actual AWS account ID
   - Update the `ImageIdentifier` with your ECR repository URI
   - Adjust CPU/Memory as needed

2. **Set Environment Variables:**
   Add your database connection strings and other secrets to the configuration.

## Step 4: Deploy

### Option A: Using AWS CLI

```bash
# Create the App Runner service
aws apprunner create-service --cli-input-json file://apprunner-config.json --region us-east-2
```

### Option B: Using GitHub Actions

1. Set up GitHub Secrets:
   - `AWS_ACCESS_KEY_ID`
   - `AWS_SECRET_ACCESS_KEY`

2. Replace the existing workflow with `apprunner-deploy.yml`

3. Push to main branch to trigger deployment

## Step 5: Access Your Application

After deployment, App Runner will provide you with:
- **Service URL** (e.g., `https://abc123.us-east-2.awsapprunner.com`)
- **Auto-scaling** based on traffic
- **HTTPS** enabled by default
- **Health checks** and monitoring

## Configuration Options

### Environment Variables
Add to `apprunner-config.json`:
```json
"RuntimeEnvironmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Production",
  "ASPNETCORE_URLS": "http://+:8080",
  "ConnectionStrings__DefaultConnection": "your-connection-string"
}
```

### Scaling
Adjust in `apprunner-config.json`:
```json
"InstanceConfiguration": {
  "Cpu": "1 vCPU",        // 0.25, 0.5, 1, 2, 4
  "Memory": "2 GB"         // 0.5, 1, 2, 3, 4, 8, 12, 16
}
```

## Benefits of App Runner for POC

✅ **Serverless** - No server management  
✅ **Auto-scaling** - Scales to zero when not used  
✅ **HTTPS** - SSL/TLS enabled by default  
✅ **Simple** - Just push code and deploy  
✅ **Cost-effective** - Pay only for what you use  
✅ **Integrated** - Works with ECR, IAM, VPC  

## Cost Estimate (POC)

- **0.25 vCPU, 0.5 GB RAM**: ~$7/month for always-on
- **Scales to zero**: Pay only for active requests
- **ECR storage**: ~$1/month for Docker images
- **Total**: ~$8-15/month depending on usage
