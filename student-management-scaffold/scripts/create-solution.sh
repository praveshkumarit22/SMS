#!/usr/bin/env bash
set -e

ROOT="$(pwd)/sms-solution"
echo "Creating solution under $ROOT"

# Create directories
rm -rf "$ROOT"
mkdir -p "$ROOT/src/Api/SMS.Api"
mkdir -p "$ROOT/src/Domain"
mkdir -p "$ROOT/src/Infrastructure"
mkdir -p "$ROOT/src/ClientApp"
mkdir -p "$ROOT/scripts"
mkdir -p "$ROOT/templates/frontend/src/app/student/admission"
mkdir -p "$ROOT/uploads"

cd "$ROOT"

echo "Creating .NET solution and projects..."
dotnet new sln -n SMS

cd src
dotnet new webapi -n SMS.Api --framework net10.0
dotnet new classlib -n SMS.Domain --framework net10.0
dotnet new classlib -n SMS.Infrastructure --framework net10.0

dotnet sln add SMS.Api/SMS.Api.csproj
dotnet sln add SMS.Domain/SMS.Domain.csproj
dotnet sln add SMS.Infrastructure/SMS.Infrastructure.csproj

# Add project references
cd SMS.Api
dotnet add reference ../SMS.Domain/SMS.Domain.csproj
dotnet add reference ../SMS.Infrastructure/SMS.Infrastructure.csproj
cd ..

cd SMS.Infrastructure
dotnet add reference ../SMS.Domain/SMS.Domain.csproj
cd ../..

echo "Copying template files..."
# Copy templates provided in the package (this script assumes templates are in ../templates)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cp -R "$SCRIPT_DIR/../templates/." .

echo "Restoring and building backend..."
dotnet restore
dotnet build

echo "Setting up docker-compose for SQL Server..."
# launch SQL Server container for migrations
cat > docker-compose.yml <<'YAML'
version: '3.8'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Your_strong_Password1
    ports:
      - "1433:1433"
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Your_strong_Password1' -Q 'select 1'"]
      interval: 10s
      retries: 10
YAML

echo "Starting SQL Server in background..."
docker-compose up -d
echo "Waiting 15s for SQL Server to be ready..."
sleep 15

echo "Applying EF migrations (if any)..."
# Migrations are seeded as SQL script - run it
if [ -f "migrations/initial.sql" ]; then
  echo "Applying SQL migration script..."
  docker exec $(docker ps -q -f ancestor=mcr.microsoft.com/mssql/server:2019-latest) /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_strong_Password1 -i /tmp/initial.sql || true
fi

echo "Creating Angular app and installing dependencies..."
cd ../../
npx -y @angular/cli@14 new client-app --strict --routing --style=scss --skip-git
cd client-app
npm install

echo "Copying frontend templates..."
cp -R ../../templates/frontend/* src/

echo "Building frontend..."
npm run build -- --output-path=dist

cd "$ROOT"
echo "Packaging repository into zip..."
ZIPNAME="../sms-feature-student-management.zip"
cd ..
rm -f "$ZIPNAME"
zip -r "$ZIPNAME" sms-solution

echo "Done. ZIP created at $ZIPNAME"
echo "Please inspect and commit/push sms-solution to your repository, branch feature/student-management."
