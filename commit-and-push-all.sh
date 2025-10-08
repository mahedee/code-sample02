#!/bin/bash
# Automatically commit all changes and push to current branch
# Usage: ./commit-and-push-all.sh

# Get current date and time
timestamp=$(date '+%Y-%m-%d %H:%M:%S')

# Add all changes to staging
echo "Adding all changes to staging..."
git add .

# Check if there are any changes to commit
if [ -z "$(git status --porcelain)" ]; then
    echo "No changes to commit."
    exit 0
fi

# Commit with timestamp message
commit_message="Commit everything at $timestamp"
echo "Committing with message: $commit_message"
git commit -m "$commit_message"

# Push to current branch
echo "Pushing to current branch..."
git push

echo "All changes have been committed and pushed successfully!"
