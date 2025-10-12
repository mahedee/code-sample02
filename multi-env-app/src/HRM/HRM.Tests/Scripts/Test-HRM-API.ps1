# HRM API Automated Testing Script
# This script performs comprehensive API testing for the HRM application
# across different environments with detailed reporting.

param(
    [string]$Environment = "Development",
    [string]$BaseUrl = "",
    [switch]$Verbose = $false,
    [switch]$StopOnFailure = $false,
    [switch]$GenerateReport = $true,
    [switch]$ReadOnly = $false
)

# Script configuration
$script:TestResults = @()
$script:PassedTests = 0
$script:FailedTests = 0
$script:TotalTests = 0
$script:StartTime = Get-Date

# Color configuration
$Colors = @{
    Success = "Green"
    Error = "Red"
    Warning = "Yellow"
    Info = "Cyan"
    Header = "Magenta"
}

# Determine base URL if not provided
if (-not $BaseUrl) {
    $BaseUrl = switch ($Environment.ToLower()) {
        "development" { "https://localhost:7285" }
        "qa" { "https://qa-hrm.company.com" }
        "production" { "https://hrm.company.com" }
        default { "https://localhost:7285" }
    }
}

function Write-TestLog {
    param(
        [string]$Message, 
        [string]$Level = "Info",
        [switch]$NoNewline = $false
    )
    
    $color = $Colors[$Level]
    $timestamp = Get-Date -Format "HH:mm:ss"
    $logMessage = "[$timestamp] $Message"
    
    if ($NoNewline) {
        Write-Host $logMessage -ForegroundColor $color -NoNewline
    } else {
        Write-Host $logMessage -ForegroundColor $color
    }
    
    if ($Verbose -and $GenerateReport) {
        Add-Content -Path "api-test-log.txt" -Value "[$timestamp] [$Level] $Message"
    }
}

function Invoke-ApiTest {
    param(
        [string]$TestName,
        [string]$Method = "GET",
        [string]$Endpoint,
        [object]$Body = $null,
        [hashtable]$ExpectedResult = @{},
        [int]$ExpectedStatusCode = 200,
        [string]$TestCategory = "General"
    )
    
    $script:TotalTests++
    $testStart = Get-Date
    
    try {
        Write-TestLog "Testing: $TestName" "Info"
        
        $headers = @{
            "Accept" = "application/json"
            "Content-Type" = "application/json"
        }
        
        $requestParams = @{
            Uri = "$BaseUrl$Endpoint"
            Method = $Method
            Headers = $headers
            SkipCertificateCheck = $true
            TimeoutSec = 30
            ErrorAction = "Stop"
        }
        
        if ($Body -and ($Method -eq "POST" -or $Method -eq "PUT")) {
            $requestParams.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-WebRequest @requestParams
        $content = $null
        
        if ($response.Content) {
            try {
                $content = $response.Content | ConvertFrom-Json
            }
            catch {
                # Handle non-JSON responses
                $content = $response.Content
            }
        }
        
        # Verify status code
        if ($response.StatusCode -ne $ExpectedStatusCode) {
            throw "Expected status code $ExpectedStatusCode, got $($response.StatusCode)"
        }
        
        # Verify expected results for JSON responses
        if ($content -and $content -is [PSCustomObject] -and $ExpectedResult.Count -gt 0) {
            foreach ($key in $ExpectedResult.Keys) {
                $expectedValue = $ExpectedResult[$key]
                $actualValue = $content.$key
                
                if ($actualValue -ne $expectedValue) {
                    throw "Expected $key to be '$expectedValue', got '$actualValue'"
                }
            }
        }
        
        $testEnd = Get-Date
        $duration = ($testEnd - $testStart).TotalMilliseconds
        
        $script:PassedTests++
        Write-TestLog "✓ PASSED: $TestName ($([math]::Round($duration, 2))ms)" "Success"
        
        $script:TestResults += @{
            Name = $TestName
            Status = "PASSED"
            Duration = $duration
            Endpoint = $Endpoint
            Method = $Method
            StatusCode = $response.StatusCode
            Category = $TestCategory
            Timestamp = $testStart
        }
        
        return $content
        
    }
    catch {
        $testEnd = Get-Date
        $duration = ($testEnd - $testStart).TotalMilliseconds
        
        $script:FailedTests++
        $errorMessage = $_.Exception.Message
        Write-TestLog "✗ FAILED: $TestName - $errorMessage" "Error"
        
        $script:TestResults += @{
            Name = $TestName
            Status = "FAILED"
            Duration = $duration
            Endpoint = $Endpoint
            Method = $Method
            Error = $errorMessage
            Category = $TestCategory
            Timestamp = $testStart
        }
        
        if ($StopOnFailure) {
            throw "Test failed: $TestName - $errorMessage"
        }
        
        return $null
    }
}

function Test-ApiHealthCheck {
    Write-TestLog "=== API Health Check ===" "Header"
    
    try {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        $response = Invoke-WebRequest -Uri $BaseUrl -SkipCertificateCheck -TimeoutSec 10 -ErrorAction Stop
        $stopwatch.Stop()
        
        Write-TestLog "✓ API is responding at $BaseUrl ($($stopwatch.ElapsedMilliseconds)ms)" "Success"
        return $true
    }
    catch {
        Write-TestLog "✗ API not responding at $BaseUrl - $($_.Exception.Message)" "Error"
        return $false
    }
}

function Test-DepartmentsApi {
    Write-TestLog "=== Testing Departments API ===" "Header"
    
    # Test GET all departments
    $departments = Invoke-ApiTest -TestName "Get All Departments" -Endpoint "/api/departments" -TestCategory "Departments"
    
    if ($departments) {
        # Test GET specific department
        $firstDept = $departments[0]
        if ($firstDept.departmentId) {
            Invoke-ApiTest -TestName "Get Department by ID" -Endpoint "/api/departments/$($firstDept.departmentId)" -TestCategory "Departments"
        }
        
        if (-not $ReadOnly) {
            # Test POST new department
            $newDept = @{
                name = "Test Department $(Get-Random)"
                description = "Automated test department"
                isActive = $true
            }
            
            $createdDept = Invoke-ApiTest -TestName "Create New Department" -Method "POST" -Endpoint "/api/departments" -Body $newDept -ExpectedStatusCode 201 -TestCategory "Departments"
            
            if ($createdDept) {
                # Test PUT update department
                $updateDept = @{
                    name = "Updated Test Department"
                    description = "Updated by automated test"
                    isActive = $true
                }
                
                Invoke-ApiTest -TestName "Update Department" -Method "PUT" -Endpoint "/api/departments/$($createdDept.departmentId)" -Body $updateDept -ExpectedStatusCode 204 -TestCategory "Departments"
                
                # Test DELETE department
                Invoke-ApiTest -TestName "Delete Department" -Method "DELETE" -Endpoint "/api/departments/$($createdDept.departmentId)" -ExpectedStatusCode 204 -TestCategory "Departments"
            }
        }
    }
    
    # Test error cases
    Invoke-ApiTest -TestName "Get Non-existent Department" -Endpoint "/api/departments/99999" -ExpectedStatusCode 404 -TestCategory "Departments"
}

function Test-EmployeesApi {
    Write-TestLog "=== Testing Employees API ===" "Header"
    
    # Test GET all employees
    $employees = Invoke-ApiTest -TestName "Get All Employees" -Endpoint "/api/employees" -TestCategory "Employees"
    
    if ($employees) {
        # Test GET specific employee
        $firstEmp = $employees[0]
        if ($firstEmp.employeeId) {
            Invoke-ApiTest -TestName "Get Employee by ID" -Endpoint "/api/employees/$($firstEmp.employeeId)" -TestCategory "Employees"
        }
        
        if (-not $ReadOnly) {
            # Get departments for employee test
            $departments = Invoke-ApiTest -TestName "Get Departments for Employee Test" -Endpoint "/api/departments" -TestCategory "Setup"
            
            if ($departments -and $departments.Count -gt 0) {
                $activeDept = $departments | Where-Object { $_.isActive -eq $true } | Select-Object -First 1
                
                if ($activeDept) {
                    # Test POST new employee
                    $newEmp = @{
                        firstName = "Test"
                        lastName = "Employee$(Get-Random)"
                        email = "test.employee$(Get-Random)@company.com"
                        phoneNumber = "555-$(Get-Random -Minimum 1000 -Maximum 9999)"
                        hireDate = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        salary = 75000
                        departmentId = $activeDept.departmentId
                    }
                    
                    $createdEmp = Invoke-ApiTest -TestName "Create New Employee" -Method "POST" -Endpoint "/api/employees" -Body $newEmp -ExpectedStatusCode 201 -TestCategory "Employees"
                    
                    if ($createdEmp) {
                        # Test PUT update employee
                        $updateEmp = @{
                            firstName = "Updated"
                            lastName = "Employee"
                            email = $createdEmp.email
                            phoneNumber = $createdEmp.phoneNumber
                            hireDate = $createdEmp.hireDate
                            salary = 80000
                            departmentId = $createdEmp.departmentId
                        }
                        
                        Invoke-ApiTest -TestName "Update Employee" -Method "PUT" -Endpoint "/api/employees/$($createdEmp.employeeId)" -Body $updateEmp -ExpectedStatusCode 204 -TestCategory "Employees"
                        
                        # Test DELETE employee
                        Invoke-ApiTest -TestName "Delete Employee" -Method "DELETE" -Endpoint "/api/employees/$($createdEmp.employeeId)" -ExpectedStatusCode 204 -TestCategory "Employees"
                    }
                }
            }
        }
    }
    
    # Test error cases
    Invoke-ApiTest -TestName "Get Non-existent Employee" -Endpoint "/api/employees/99999" -ExpectedStatusCode 404 -TestCategory "Employees"
}

function Test-ApiPerformance {
    Write-TestLog "=== Performance Testing ===" "Header"
    
    # Test response times
    $performanceResults = @()
    
    for ($i = 1; $i -le 5; $i++) {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        try {
            $response = Invoke-WebRequest -Uri "$BaseUrl/api/departments" -SkipCertificateCheck -TimeoutSec 10
            $stopwatch.Stop()
            $performanceResults += $stopwatch.ElapsedMilliseconds
            Write-TestLog "Performance test $i`: $($stopwatch.ElapsedMilliseconds)ms" "Info"
        }
        catch {
            $stopwatch.Stop()
            Write-TestLog "Performance test $i failed: $($_.Exception.Message)" "Error"
        }
    }
    
    if ($performanceResults.Count -gt 0) {
        $avgTime = ($performanceResults | Measure-Object -Average).Average
        $maxTime = ($performanceResults | Measure-Object -Maximum).Maximum
        $minTime = ($performanceResults | Measure-Object -Minimum).Minimum
        
        Write-TestLog "Performance Summary - Avg: $([math]::Round($avgTime, 2))ms, Min: $minTime ms, Max: $maxTime ms" "Info"
        
        # Add performance results
        $script:TestResults += @{
            Name = "Performance Test"
            Status = if ($avgTime -lt 1000) { "PASSED" } else { "FAILED" }
            Duration = $avgTime
            Category = "Performance"
            Details = "Avg: $([math]::Round($avgTime, 2))ms, Min: $minTime ms, Max: $maxTime ms"
        }
    }
}

function Test-ConcurrentRequests {
    Write-TestLog "=== Concurrent Request Testing ===" "Header"
    
    try {
        $jobs = @()
        $concurrentCount = 10
        
        # Start concurrent requests
        for ($i = 1; $i -le $concurrentCount; $i++) {
            $job = Start-Job -ScriptBlock {
                param($url)
                try {
                    $response = Invoke-WebRequest -Uri $url -SkipCertificateCheck -TimeoutSec 10
                    return @{
                        Success = $true
                        StatusCode = $response.StatusCode
                        ResponseTime = (Measure-Command { Invoke-WebRequest -Uri $url -SkipCertificateCheck -TimeoutSec 10 }).TotalMilliseconds
                    }
                }
                catch {
                    return @{
                        Success = $false
                        Error = $_.Exception.Message
                    }
                }
            } -ArgumentList "$BaseUrl/api/departments"
            
            $jobs += $job
        }
        
        # Wait for all jobs to complete
        $results = $jobs | Wait-Job | Receive-Job
        $jobs | Remove-Job
        
        $successCount = ($results | Where-Object { $_.Success }).Count
        $successRate = ($successCount / $concurrentCount) * 100
        
        Write-TestLog "Concurrent requests: $successCount/$concurrentCount successful ($([math]::Round($successRate, 2))%)" "Info"
        
        $script:TestResults += @{
            Name = "Concurrent Requests Test"
            Status = if ($successRate -ge 90) { "PASSED" } else { "FAILED" }
            Category = "Load"
            Details = "$successCount/$concurrentCount successful ($([math]::Round($successRate, 2))%)"
        }
    }
    catch {
        Write-TestLog "Concurrent test failed: $($_.Exception.Message)" "Error"
    }
}

function Generate-TestReport {
    $endTime = Get-Date
    $totalDuration = ($endTime - $script:StartTime).TotalSeconds
    
    Write-TestLog "`n=== TEST SUMMARY REPORT ===" "Header"
    Write-TestLog "Environment: $Environment" "Info"
    Write-TestLog "Base URL: $BaseUrl" "Info"
    Write-TestLog "Test Duration: $([math]::Round($totalDuration, 2)) seconds" "Info"
    Write-TestLog "Total Tests: $script:TotalTests" "Info"
    Write-TestLog "Passed: $script:PassedTests" "Success"
    Write-TestLog "Failed: $script:FailedTests" $(if ($script:FailedTests -gt 0) { "Error" } else { "Success" })
    
    $successRate = if ($script:TotalTests -gt 0) { [math]::Round(($script:PassedTests / $script:TotalTests) * 100, 2) } else { 0 }
    Write-TestLog "Success Rate: $successRate%" $(if ($successRate -eq 100) { "Success" } elseif ($successRate -ge 80) { "Warning" } else { "Error" })
    
    if ($GenerateReport) {
        # Generate detailed results by category
        Write-TestLog "`nDetailed Results by Category:" "Info"
        $categories = $script:TestResults | Group-Object Category
        
        foreach ($category in $categories) {
            Write-TestLog "`n--- $($category.Name) Tests ---" "Header"
            foreach ($result in $category.Group) {
                $status = if ($result.Status -eq "PASSED") { "✓" } else { "✗" }
                $duration = if ($result.Duration) { "($([math]::Round($result.Duration, 2)) ms)" } else { "" }
                
                if ($result.Status -eq "PASSED") {
                    Write-TestLog "$status $($result.Name) $duration" "Success"
                } else {
                    Write-TestLog "$status $($result.Name) - $($result.Error)" "Error"
                }
            }
        }
        
        # Generate JSON report
        $report = @{
            Environment = $Environment
            BaseUrl = $BaseUrl
            Timestamp = $script:StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            Duration = $totalDuration
            Summary = @{
                TotalTests = $script:TotalTests
                PassedTests = $script:PassedTests
                FailedTests = $script:FailedTests
                SuccessRate = $successRate
            }
            Results = $script:TestResults
            SystemInfo = @{
                PowerShellVersion = $PSVersionTable.PSVersion.ToString()
                OS = $PSVersionTable.OS
                MachineName = $env:COMPUTERNAME
            }
        }
        
        $reportFile = "hrm-api-test-report-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
        $report | ConvertTo-Json -Depth 10 | Out-File -FilePath $reportFile -Encoding UTF8
        Write-TestLog "JSON report saved to: $reportFile" "Info"
        
        # Generate HTML report
        $htmlReport = Generate-HtmlReport -Report $report
        $htmlReportFile = "hrm-api-test-report-$(Get-Date -Format 'yyyyMMdd-HHmmss').html"
        $htmlReport | Out-File -FilePath $htmlReportFile -Encoding UTF8
        Write-TestLog "HTML report saved to: $htmlReportFile" "Info"
    }
}

function Generate-HtmlReport {
    param($Report)
    
    $html = @"
<!DOCTYPE html>
<html>
<head>
    <title>HRM API Test Report</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .header { background-color: #f4f4f4; padding: 20px; border-radius: 5px; }
        .summary { display: flex; gap: 20px; margin: 20px 0; }
        .summary-card { padding: 15px; border-radius: 5px; text-align: center; min-width: 120px; }
        .success { background-color: #d4edda; color: #155724; }
        .error { background-color: #f8d7da; color: #721c24; }
        .warning { background-color: #fff3cd; color: #856404; }
        .info { background-color: #d1ecf1; color: #0c5460; }
        table { width: 100%; border-collapse: collapse; margin: 20px 0; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        th { background-color: #f2f2f2; }
        .passed { color: #28a745; }
        .failed { color: #dc3545; }
        .category-header { background-color: #e9ecef; font-weight: bold; }
    </style>
</head>
<body>
    <div class="header">
        <h1>HRM API Test Report</h1>
        <p><strong>Environment:</strong> $($Report.Environment)</p>
        <p><strong>Base URL:</strong> $($Report.BaseUrl)</p>
        <p><strong>Timestamp:</strong> $($Report.Timestamp)</p>
        <p><strong>Duration:</strong> $([math]::Round($Report.Duration, 2)) seconds</p>
    </div>
    
    <div class="summary">
        <div class="summary-card info">
            <h3>$($Report.Summary.TotalTests)</h3>
            <p>Total Tests</p>
        </div>
        <div class="summary-card success">
            <h3>$($Report.Summary.PassedTests)</h3>
            <p>Passed</p>
        </div>
        <div class="summary-card error">
            <h3>$($Report.Summary.FailedTests)</h3>
            <p>Failed</p>
        </div>
        <div class="summary-card $(if ($Report.Summary.SuccessRate -eq 100) { 'success' } elseif ($Report.Summary.SuccessRate -ge 80) { 'warning' } else { 'error' })">
            <h3>$($Report.Summary.SuccessRate)%</h3>
            <p>Success Rate</p>
        </div>
    </div>
    
    <h2>Test Results</h2>
    <table>
        <thead>
            <tr>
                <th>Test Name</th>
                <th>Status</th>
                <th>Duration (ms)</th>
                <th>Category</th>
                <th>Details</th>
            </tr>
        </thead>
        <tbody>
"@
    
    $currentCategory = ""
    foreach ($result in ($Report.Results | Sort-Object Category, Name)) {
        if ($result.Category -ne $currentCategory) {
            $currentCategory = $result.Category
            $html += "<tr class='category-header'><td colspan='5'>$currentCategory</td></tr>"
        }
        
        $statusClass = if ($result.Status -eq "PASSED") { "passed" } else { "failed" }
        $duration = if ($result.Duration) { [math]::Round($result.Duration, 2) } else { "N/A" }
        $details = if ($result.Error) { $result.Error } elseif ($result.Details) { $result.Details } else { "" }
        
        $html += "<tr><td>$($result.Name)</td><td class='$statusClass'>$($result.Status)</td><td>$duration</td><td>$($result.Category)</td><td>$details</td></tr>"
    }
    
    $html += @"
        </tbody>
    </table>
    
    <div class="header">
        <h3>System Information</h3>
        <p><strong>PowerShell Version:</strong> $($Report.SystemInfo.PowerShellVersion)</p>
        <p><strong>Operating System:</strong> $($Report.SystemInfo.OS)</p>
        <p><strong>Machine Name:</strong> $($Report.SystemInfo.MachineName)</p>
    </div>
</body>
</html>
"@
    
    return $html
}

# Main execution
try {
    Write-TestLog "Starting HRM API Automated Testing" "Header"
    Write-TestLog "Environment: $Environment | Base URL: $BaseUrl" "Info"
    Write-TestLog "Read-Only Mode: $ReadOnly | Generate Report: $GenerateReport" "Info"
    
    # Initialize log file
    if ($Verbose -and $GenerateReport) {
        "HRM API Test Log - $(Get-Date)" | Out-File -FilePath "api-test-log.txt" -Encoding UTF8
    }
    
    # Health check
    if (-not (Test-ApiHealthCheck)) {
        throw "API health check failed. Cannot proceed with tests."
    }
    
    # Run API tests
    Test-DepartmentsApi
    Test-EmployeesApi
    
    # Performance and load tests
    Test-ApiPerformance
    Test-ConcurrentRequests
    
    # Generate report
    Generate-TestReport
    
    # Exit with appropriate code
    if ($script:FailedTests -eq 0) {
        Write-TestLog "`nAll tests passed successfully! 🎉" "Success"
        exit 0
    } else {
        Write-TestLog "`nSome tests failed. Check the report for details. ⚠️" "Warning"
        exit 1
    }
}
catch {
    Write-TestLog "Test execution failed: $($_.Exception.Message)" "Error"
    
    if ($GenerateReport) {
        # Generate failure report
        $failureReport = @{
            Environment = $Environment
            BaseUrl = $BaseUrl
            Timestamp = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            Error = $_.Exception.Message
            Results = $script:TestResults
        }
        
        $reportFile = "hrm-api-test-failure-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
        $failureReport | ConvertTo-Json -Depth 10 | Out-File -FilePath $reportFile -Encoding UTF8
        Write-TestLog "Failure report saved to: $reportFile" "Info"
    }
    
    exit 1
}