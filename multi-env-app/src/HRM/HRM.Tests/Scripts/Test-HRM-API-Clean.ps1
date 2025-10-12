# HRM API Automated Testing Script
# Comprehensive testing across multiple environments

param(
    [string]$Environment = "Development",
    [string]$BaseUrl = "",
    [int]$MaxConcurrentRequests = 3,
    [bool]$GenerateReport = $true,
    [string]$OutputPath = ".\TestResults"
)

# Test configuration
$script:TotalTests = 0
$script:PassedTests = 0
$script:FailedTests = 0
$script:TestResults = @()
$script:StartTime = Get-Date

# Environment configurations
$EnvironmentConfig = @{
    "Development" = @{
        BaseUrl = "https://localhost:7157"
        Name = "Development Environment"
    }
    "QA" = @{
        BaseUrl = "https://localhost:7158"
        Name = "QA Environment"
    }
    "Production" = @{
        BaseUrl = "https://localhost:7159"
        Name = "Production Environment"
    }
}

# Use provided BaseUrl or get from environment config
if ([string]::IsNullOrEmpty($BaseUrl)) {
    if ($EnvironmentConfig.ContainsKey($Environment)) {
        $BaseUrl = $EnvironmentConfig[$Environment].BaseUrl
        $EnvironmentName = $EnvironmentConfig[$Environment].Name
    } else {
        Write-Host "Unknown environment: $Environment" -ForegroundColor Red
        exit 1
    }
} else {
    $EnvironmentName = "$Environment Environment"
}

function Write-TestLog {
    param([string]$Message, [string]$Level = "Info")
    
    $timestamp = Get-Date -Format "HH:mm:ss"
    $color = switch ($Level) {
        "Success" { "Green" }
        "Error" { "Red" }
        "Warning" { "Yellow" }
        default { "White" }
    }
    
    Write-Host "[$timestamp] $Message" -ForegroundColor $color
}

function Invoke-ApiTest {
    param(
        [string]$TestName,
        [string]$Endpoint,
        [string]$Method = "GET",
        [object]$Body = $null,
        [int]$ExpectedStatusCode = 200,
        [string]$TestCategory = "API"
    )
    
    $script:TotalTests++
    $testStart = Get-Date
    
    try {
        $uri = "$BaseUrl$Endpoint"
        $headers = @{ "Content-Type" = "application/json" }
        
        $requestParams = @{
            Uri = $uri
            Method = $Method
            Headers = $headers
            ErrorAction = "Stop"
        }
        
        if ($Body -and $Method -in @("POST", "PUT", "PATCH")) {
            $requestParams.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-RestMethod @requestParams
        $testEnd = Get-Date
        $duration = ($testEnd - $testStart).TotalMilliseconds
        
        if ($response) {
            $script:PassedTests++
            Write-TestLog "PASS: $TestName ($([math]::Round($duration, 2))ms)" "Success"
        } else {
            $script:FailedTests++
            Write-TestLog "FAIL: $TestName - No response received" "Error"
        }
        
        $script:TestResults += @{
            Name = $TestName
            Status = "PASSED"
            Duration = $duration
            Endpoint = $Endpoint
            Method = $Method
            StatusCode = 200
            Category = $TestCategory
            Timestamp = $testStart
        }
        
        return $response
    }
    catch {
        $testEnd = Get-Date
        $duration = ($testEnd - $testStart).TotalMilliseconds
        
        $script:FailedTests++
        $errorMessage = $_.Exception.Message
        Write-TestLog "FAIL: $TestName - $errorMessage ($([math]::Round($duration, 2))ms)" "Error"
        
        $script:TestResults += @{
            Name = $TestName
            Status = "FAILED"
            Duration = $duration
            Endpoint = $Endpoint
            Method = $Method
            StatusCode = if ($_.Exception.Response) { $_.Exception.Response.StatusCode } else { "N/A" }
            Error = $errorMessage
            Category = $TestCategory
            Timestamp = $testStart
        }
        
        return $null
    }
}

function Test-DepartmentsApi {
    Write-TestLog "Testing Departments API..." "Info"
    
    # Test 1: Get all departments
    $departments = Invoke-ApiTest -TestName "Get All Departments" -Endpoint "/api/departments" -TestCategory "Departments"
    
    if ($departments) {
        # Test 2: Get specific department
        if ($departments.Count -gt 0) {
            $departmentId = $departments[0].departmentId
            Invoke-ApiTest -TestName "Get Department by ID" -Endpoint "/api/departments/$departmentId" -TestCategory "Departments"
        }
        
        # Test 3: Create new department
        $newDepartment = @{
            Name = "Test Department $(Get-Date -Format 'HHmmss')"
            Description = "Automated test department"
            IsActive = $true
        }
        
        $createdDept = Invoke-ApiTest -TestName "Create Department" -Endpoint "/api/departments" -Method "POST" -Body $newDepartment -TestCategory "Departments"
        
        if ($createdDept -and $createdDept.departmentId) {
            # Test 4: Update department
            $updateDept = @{
                DepartmentId = $createdDept.departmentId
                Name = "Updated Test Department"
                Description = "Updated description"
                IsActive = $false
            }
            
            Invoke-ApiTest -TestName "Update Department" -Endpoint "/api/departments/$($createdDept.departmentId)" -Method "PUT" -Body $updateDept -TestCategory "Departments"
            
            # Test 5: Delete department
            Invoke-ApiTest -TestName "Delete Department" -Endpoint "/api/departments/$($createdDept.departmentId)" -Method "DELETE" -TestCategory "Departments"
        }
    }
    
    # Test 6: Get non-existent department (should return 404)
    try {
        Invoke-RestMethod -Uri "$BaseUrl/api/departments/99999" -Method GET -ErrorAction Stop
        Write-TestLog "FAIL: Get Non-existent Department - Should have returned 404" "Error"
        $script:FailedTests++
    }
    catch {
        Write-TestLog "PASS: Get Non-existent Department - Correctly returned 404" "Success"
        $script:PassedTests++
    }
    $script:TotalTests++
}

function Test-EmployeesApi {
    Write-TestLog "Testing Employees API..." "Info"
    
    # Get departments first for employee creation
    $departments = Invoke-ApiTest -TestName "Get Departments for Employee Tests" -Endpoint "/api/departments" -TestCategory "Employees"
    
    # Test 1: Get all employees
    $employees = Invoke-ApiTest -TestName "Get All Employees" -Endpoint "/api/employees" -TestCategory "Employees"
    
    if ($employees) {
        # Test 2: Get specific employee
        if ($employees.Count -gt 0) {
            $employeeId = $employees[0].employeeId
            Invoke-ApiTest -TestName "Get Employee by ID" -Endpoint "/api/employees/$employeeId" -TestCategory "Employees"
            
            # Test 3: Get employees by department
            $departmentId = $employees[0].departmentId
            Invoke-ApiTest -TestName "Get Employees by Department" -Endpoint "/api/employees/department/$departmentId" -TestCategory "Employees"
        }
    }
    
    # Test 4: Create new employee
    if ($departments -and $departments.Count -gt 0) {
        $newEmployee = @{
            FirstName = "Test"
            LastName = "Employee$(Get-Date -Format 'HHmmss')"
            Email = "test.employee$(Get-Date -Format 'HHmmss')@test.com"
            PhoneNumber = "123-456-7890"
            HireDate = (Get-Date).ToString("yyyy-MM-dd")
            Salary = 50000
            DepartmentId = $departments[0].departmentId
        }
        
        $createdEmp = Invoke-ApiTest -TestName "Create Employee" -Endpoint "/api/employees" -Method "POST" -Body $newEmployee -TestCategory "Employees"
        
        if ($createdEmp -and $createdEmp.employeeId) {
            # Test 5: Update employee
            $updateEmp = @{
                EmployeeId = $createdEmp.employeeId
                FirstName = "Updated"
                LastName = "Employee"
                Email = $createdEmp.email
                PhoneNumber = "123-456-7891"
                HireDate = $createdEmp.hireDate
                Salary = 55000
                DepartmentId = $createdEmp.departmentId
            }
            
            Invoke-ApiTest -TestName "Update Employee" -Endpoint "/api/employees/$($createdEmp.employeeId)" -Method "PUT" -Body $updateEmp -TestCategory "Employees"
            
            # Test 6: Delete employee
            Invoke-ApiTest -TestName "Delete Employee" -Endpoint "/api/employees/$($createdEmp.employeeId)" -Method "DELETE" -TestCategory "Employees"
        }
    }
}

function Test-ConcurrentRequests {
    Write-TestLog "Testing Concurrent Requests..." "Info"
    
    $jobs = @()
    $testStart = Get-Date
    
    for ($i = 1; $i -le $MaxConcurrentRequests; $i++) {
        $job = Start-Job -ScriptBlock {
            param($BaseUrl, $TestIndex)
            try {
                $response = Invoke-RestMethod -Uri "$BaseUrl/api/departments" -Method GET -TimeoutSec 30
                return @{ Success = $true; TestIndex = $TestIndex; Count = $response.Count }
            }
            catch {
                return @{ Success = $false; TestIndex = $TestIndex; Error = $_.Exception.Message }
            }
        } -ArgumentList $BaseUrl, $i
        
        $jobs += $job
    }
    
    $results = $jobs | Wait-Job | Receive-Job
    $jobs | Remove-Job
    
    $testEnd = Get-Date
    $duration = ($testEnd - $testStart).TotalMilliseconds
    
    $successCount = ($results | Where-Object { $_.Success }).Count
    $script:TotalTests++
    
    if ($successCount -eq $MaxConcurrentRequests) {
        $script:PassedTests++
        Write-TestLog "PASS: Concurrent Requests Test - $successCount/$MaxConcurrentRequests successful ($([math]::Round($duration, 2))ms)" "Success"
    } else {
        $script:FailedTests++
        Write-TestLog "FAIL: Concurrent Requests Test - Only $successCount/$MaxConcurrentRequests successful" "Error"
    }
}

function Generate-TestReport {
    if (-not $GenerateReport) { return }
    
    Write-TestLog "Generating test report..." "Info"
    
    if (-not (Test-Path $OutputPath)) {
        New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
    }
    
    $endTime = Get-Date
    $totalDuration = ($endTime - $script:StartTime).TotalSeconds
    
    # Generate JSON report
    $report = @{
        TestRun = @{
            Environment = $EnvironmentName
            BaseUrl = $BaseUrl
            StartTime = $script:StartTime
            EndTime = $endTime
            Duration = $totalDuration
            TotalTests = $script:TotalTests
            PassedTests = $script:PassedTests
            FailedTests = $script:FailedTests
            SuccessRate = if ($script:TotalTests -gt 0) { [math]::Round(($script:PassedTests / $script:TotalTests) * 100, 2) } else { 0 }
        }
        Tests = $script:TestResults
    }
    
    $jsonPath = Join-Path $OutputPath "test-results-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
    $report | ConvertTo-Json -Depth 10 | Out-File -FilePath $jsonPath -Encoding UTF8
    
    Write-TestLog "Test report saved to: $jsonPath" "Success"
}

function Show-TestSummary {
    $endTime = Get-Date
    $totalDuration = ($endTime - $script:StartTime).TotalSeconds
    $successRate = if ($script:TotalTests -gt 0) { [math]::Round(($script:PassedTests / $script:TotalTests) * 100, 2) } else { 0 }
    
    Write-Host "`n" + "="*70 -ForegroundColor Cyan
    Write-Host "HRM API TEST SUMMARY" -ForegroundColor Cyan
    Write-Host "="*70 -ForegroundColor Cyan
    Write-Host "Environment: $EnvironmentName" -ForegroundColor White
    Write-Host "Base URL: $BaseUrl" -ForegroundColor White
    Write-Host "Duration: $([math]::Round($totalDuration, 2)) seconds" -ForegroundColor White
    Write-Host ""
    Write-Host "RESULTS:" -ForegroundColor Yellow
    Write-Host "  Total Tests: $($script:TotalTests)" -ForegroundColor White
    Write-Host "  Passed: $($script:PassedTests)" -ForegroundColor Green
    Write-Host "  Failed: $($script:FailedTests)" -ForegroundColor Red
    Write-Host "  Success Rate: $successRate%" -ForegroundColor $(if ($successRate -ge 90) { "Green" } elseif ($successRate -ge 70) { "Yellow" } else { "Red" })
    Write-Host "="*70 -ForegroundColor Cyan
}

# Main execution
Write-Host "`nHRM API Automated Testing" -ForegroundColor Cyan
Write-Host "Environment: $EnvironmentName" -ForegroundColor Yellow
Write-Host "Base URL: $BaseUrl" -ForegroundColor Yellow
Write-Host "Max Concurrent Requests: $MaxConcurrentRequests" -ForegroundColor Yellow
Write-Host ""

# Test API availability
try {
    Write-TestLog "Testing API availability..." "Info"
    $healthCheck = Invoke-RestMethod -Uri "$BaseUrl/api/departments" -Method GET -TimeoutSec 10
    Write-TestLog "API is available and responding" "Success"
}
catch {
    Write-TestLog "API is not available: $($_.Exception.Message)" "Error"
    Write-TestLog "Please ensure the HRM API is running on $BaseUrl" "Warning"
    exit 1
}

# Run all tests
Test-DepartmentsApi
Test-EmployeesApi
Test-ConcurrentRequests

# Generate reports and show summary
Generate-TestReport
Show-TestSummary

# Exit with appropriate code
exit $(if ($script:FailedTests -eq 0) { 0 } else { 1 })