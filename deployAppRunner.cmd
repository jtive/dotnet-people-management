@echo off
echo Creating App Runner service...

aws apprunner create-service ^
  --service-name "personal-info-system" ^
  --source-configuration "{\"ImageRepository\": {\"ImageIdentifier\": \"486151888818.dkr.ecr.us-east-2.amazonaws.com/personal-info-system:latest\", \"ImageConfiguration\": {\"Port\": \"8080\", \"RuntimeEnvironmentVariables\": {\"ASPNETCORE_ENVIRONMENT\": \"Production\", \"ASPNETCORE_URLS\": \"http://+:8080\"}}, \"ImageRepositoryType\": \"ECR\"}, \"AutoDeploymentsEnabled\": true}" ^
  --instance-configuration "{\"Cpu\": \"0.25 vCPU\", \"Memory\": \"0.5 GB\", \"InstanceRoleArn\": \"arn:aws:iam::486151888818:role/AppRunnerECRAccessRole\"}" ^
  --region us-east-2

echo App Runner service creation completed!
pause
