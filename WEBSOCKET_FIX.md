# WebSocket Connection Fix for AWS App Runner

## Problem
The Blazor Server application was failing to connect via WebSockets with the error:
```
WebSocket connection to 'wss://zkic2gys56.us-east-2.awsapprunner.com/_blazor?id=...' failed
```

## Root Cause
AWS App Runner **does not support WebSockets** by default. Blazor Server relies on SignalR, which tries to use WebSockets as the primary transport method.

## Solution
Configure SignalR to use **Long Polling** instead of WebSockets for AWS App Runner compatibility.

### Changes Made

1. **Updated `src/PersonalInfoBlazor/Program.cs`**:
   - Added SignalR configuration to force Long Polling transport
   - Updated Blazor Hub mapping to use Long Polling

2. **Created `apprunner-blazor-config.json`**:
   - Proper configuration for the Blazor service
   - Points to the correct ECR image: `personal-info-blazor:latest`
   - Sets correct environment variables

3. **Created `deployBlazorAppRunner.cmd`**:
   - Deployment script for the Blazor service specifically

## Deployment Steps

### Option 1: Manual Deployment
```bash
# Deploy the Blazor service
aws apprunner create-service --cli-input-json file://apprunner-blazor-config.json --region us-east-2
```

### Option 2: Using the Script
```cmd
deployBlazorAppRunner.cmd
```

### Option 3: GitHub Actions
The existing workflow in `.github/workflows/deploy.yml` already builds both images. After pushing these changes, the Blazor service will be updated automatically.

## Architecture
```
┌─────────────────────┐    ┌─────────────────────┐
│   Blazor Frontend   │    │   API Backend       │
│ (personal-info-     │───▶│ (personal-info-     │
│  blazor service)    │    │  api service)       │
│                     │    │                     │
│ SignalR (Long       │    │ REST API            │
│  Polling)           │    │                     │
└─────────────────────┘    └─────────────────────┘
```

## Key Configuration Changes

### SignalR Transport Configuration
```csharp
builder.Services.AddSignalR(options =>
{
    // Disable WebSockets for App Runner compatibility
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});
```

### Blazor Hub Configuration
```csharp
app.MapBlazorHub(options =>
{
    // Configure Blazor Hub for App Runner
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});
```

## Testing
After deployment, the Blazor Server application should:
1. ✅ Load without WebSocket errors
2. ✅ Maintain real-time updates via Long Polling
3. ✅ Connect to the API service properly
4. ✅ Function identically to the WebSocket version

## Performance Notes
- **Long Polling** is slightly less efficient than WebSockets
- **Latency** may be marginally higher
- **Functionality** remains identical
- **Compatibility** with AWS App Runner is guaranteed

## Rollback Plan
If issues occur, you can revert the Program.cs changes:
```csharp
// Remove the SignalR configuration
// Remove the BlazorHub options
// Use default WebSocket transport (will fail on App Runner)
```

## Related Files
- `src/PersonalInfoBlazor/Program.cs` - Main configuration changes
- `apprunner-blazor-config.json` - App Runner service configuration
- `deployBlazorAppRunner.cmd` - Deployment script
- `.github/workflows/deploy.yml` - CI/CD pipeline
