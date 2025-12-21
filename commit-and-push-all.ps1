# Automatically commit all changes and push to current branch
# Usage: .\commit-and-push-all.ps1

# Get current date and time
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

# Add all changes to staging
Write-Host "Adding all changes to staging..." -ForegroundColor Green
git add .

# Check if there are any changes to commit
$status = git status --porcelain
if ([string]::IsNullOrEmpty($status)) {
    Write-Host "No changes to commit." -ForegroundColor Yellow
    exit 0
}

# Commit with timestamp message
$commitMessage = "Commit everything by Mahedee at $timestamp"
Write-Host "Committing with message: $commitMessage" -ForegroundColor Green
git commit -m $commitMessage

# Push to current branch
Write-Host "Pushing to current branch..." -ForegroundColor Green
git push

Write-Host "All changes have been committed and pushed successfully!" -ForegroundColor Green
