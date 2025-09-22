# GitHub Actions + AWS App Runner Deployment Setup

## ✅ **What's Already Done:**

1. **App Runner workflow** is now in `.github/workflows/deploy.yml`
2. **Dockerfile** created for containerization
3. **App Runner configuration** ready in `apprunner-config.json`
4. **Old complex workflow** backed up to `deploy-backup.yml`

## 🔧 **Setup Steps:**

### **1. Set GitHub Secrets**
Go to your GitHub repository → Settings → Secrets and variables → Actions

Add these secrets:
- `AWS_ACCESS_KEY_ID`: Your AWS access key
- `AWS_SECRET_ACCESS_KEY`: Your AWS secret key

### **2. Create IAM User/Role (if needed)**
Your AWS user/role needs these permissions:
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:*",
        "apprunner:*",
        "iam:PassRole"
      ],
      "Resource": "*"
    }
  ]
}
```

### **3. Update App Runner Configuration**
Edit `apprunner-config.json`:
1. Replace `YOUR_ACCOUNT_ID` with your actual AWS account ID
2. Update the `ImageIdentifier` to use your ECR repository

### **4. Deploy!**

#### **Option A: First-time deployment (Manual)**
```bash
# Create the App Runner service first
aws apprunner create-service --cli-input-json file://apprunner-config.json --region us-east-2
```

#### **Option B: Let GitHub Actions handle everything**
Just push to the `main` branch - the workflow will:
1. ✅ Create ECR repository if it doesn't exist
2. ✅ Build and push Docker image
3. ✅ Update App Runner service
4. ✅ Wait for deployment to complete
5. ✅ Test the deployment
6. ✅ Show you the service URL

## 🚀 **How It Works:**

1. **Push to main branch** → Triggers GitHub Actions
2. **GitHub Actions** → Builds Docker image from your code
3. **Pushes to ECR** → Stores your container image
4. **Updates App Runner** → Deploys new version
5. **App Runner** → Runs your application with auto-scaling
6. **You get a URL** → Like `https://abc123.us-east-2.awsapprunner.com`

## 📋 **What Happens on Each Push:**

```yaml
Trigger: Push to main branch
↓
Build: Create Docker image from your code
↓
Push: Store image in AWS ECR
↓
Deploy: Update App Runner service
↓
Test: Verify deployment works
↓
Done: Show you the service URL
```

## 🔍 **Monitoring:**

- **GitHub Actions**: Shows build/deploy status
- **AWS App Runner Console**: Shows service health and metrics
- **App Runner URL**: Your live application

## 💰 **Costs:**

- **App Runner**: ~$7/month for always-on (0.25 vCPU, 0.5GB RAM)
- **ECR**: ~$1/month for image storage
- **GitHub Actions**: Free for public repos, 2000 minutes/month for private
- **Total**: ~$8-15/month for POC

## 🛠 **Troubleshooting:**

### **If deployment fails:**
1. Check GitHub Actions logs
2. Verify AWS credentials are correct
3. Ensure your AWS user has required permissions
4. Check App Runner service status in AWS console

### **If service doesn't start:**
1. Check Dockerfile is correct
2. Verify port 8080 is exposed
3. Check application logs in App Runner console

## ✅ **Next Steps:**

1. **Set up GitHub secrets** (AWS credentials)
2. **Push to main branch** to trigger deployment
3. **Check the deployment summary** in GitHub Actions
4. **Access your application** via the provided URL

Your Personal Info System will be live and accessible via HTTPS!
