#!/bin/bash
set -e

echo "Setting up development environment..."

# Fix workspace ownership if needed (when running as root or with different UID)
if [ "$(id -u)" = "0" ]; then
    echo "Running as root, fixing permissions..."
    chown -R vscode:vscode /workspace/OngekiMuseumApi || true
fi

# Install dotnet-ef tool
echo "Installing Entity Framework Core tools..."
dotnet tool install --global dotnet-ef || dotnet tool update --global dotnet-ef

# Restore packages
echo "Restoring .NET packages..."
cd /workspace/OngekiMuseumApi
dotnet restore

echo "Development environment setup complete!"
