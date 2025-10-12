# Test HRM API in all environments
param([string]$Environment = "All")

$ProjectPath = "src\HRM\HRM.API"
$BaseUrl = "https://localhost:7285"

function Test-Environment {
    param([string]$Env)
    
    Write-Host "`n=== Testing $Env Environment ===" -ForegroundColor Green
    
    # Set environment variable
    $env:ASPNETCORE_ENVIRONMENT = $Env
    
    # Show configuration
    $configFile = "src\HRM\HRM.API\appsettings.$Env.json"
    if (Test-Path $configFile) {
        $config = Get-Content $configFile | ConvertFrom-Json
        Write-Host "Database: $($config.ConnectionStrings.DefaultConnection)" -ForegroundColor Yellow
    } else {
        Write-Host "Using default appsettings.json" -ForegroundColor Yellow
    }
    
    # Apply migrations
    Write-Host "Applying migrations..." -ForegroundColor Cyan
    dotnet ef database update --project $ProjectPath
    
    # Start API in background
    Write-Host "Starting API..." -ForegroundColor Cyan
    $job = Start-Job -ScriptBlock {
        param($Path, $Env)
        $env:ASPNETCORE_ENVIRONMENT = $Env
        Set-Location $Path
        $profileName = switch ($Env) {
            "Development" { "development" }
            "QA" { "qa" }
            "Production" { "production" }
            default { "development" }
        }
        dotnet run --launch-profile $profileName
    } -ArgumentList (Get-Location).Path + "\$ProjectPath", $Env
    
    # Wait for API to start
    Start-Sleep 10
    
    # Test endpoints
    try {
        Write-Host "Testing endpoints..." -ForegroundColor Cyan
        $departments = Invoke-RestMethod -Uri "$BaseUrl/api/departments" -SkipCertificateCheck
        Write-Host "✓ Departments: $($departments.Count) found" -ForegroundColor Green
        
        $employees = Invoke-RestMethod -Uri "$BaseUrl/api/employees" -SkipCertificateCheck  
        Write-Host "✓ Employees: $($employees.Count) found" -ForegroundColor Green
        
        Write-Host "✓ $Env environment test PASSED" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ $Env environment test FAILED: $($_.Exception.Message)" -ForegroundColor Red
    }
    finally {
        # Stop API
        Stop-Job $job -Force
        Remove-Job $job
    }
}

# Test specified environment(s)
if ($Environment -eq "All") {
    Test-Environment "Development"
    Test-Environment "QA" 
    Test-Environment "Production"
} else {
    Test-Environment $Environment
}

Write-Host "`nEnvironment testing completed!" -ForegroundColor Green