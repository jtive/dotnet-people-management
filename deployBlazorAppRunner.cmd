@echo off
echo Creating Blazor App Runner service...

aws apprunner create-service ^
  --service-name "personal-info-blazor" ^
  --source-configuration "{\"ImageRepository\": {\"ImageIdentifier\": \"486151888818.dkr.ecr.us-east-2.amazonaws.com/personal-info-blazor:latest\", \"ImageConfiguration\": {\"Port\": \"8080\", \"RuntimeEnvironmentVariables\": {\"ASPNETCORE_ENVIRONMENT\": \"Production\", \"ASPNETCORE_URLS\": \"http://+:8080\", \"ApiSettings__BaseUrl\": \"https://6pz6yc2hhw.us-east-2.awsapprunner.com\"}}, \"ImageRepositoryType\": \"ECR\"}, \"AutoDeploymentsEnabled\": true}" ^
  --instance-configuration "{\"Cpu\": \"0.25 vCPU\", \"Memory\": \"0.5 GB\", \"InstanceRoleArn\": \"arn:aws:iam::486151888818:role/AppRunnerECRAccessRole2\"}" ^
  --region us-east-2

echo Blazor App Runner service creation completed!
pause
