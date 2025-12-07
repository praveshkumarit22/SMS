# SMS - Student Management (Feature Module: Student Management)

This package scaffolds a build-ready Student Management feature using:
- .NET 10 Web API (Api / Domain / Infrastructure)
- EF Core (SQL Server)
- Angular 21 frontend with a Student feature module (admission form, file upload)
- Local filesystem file storage (with an abstraction to add Azure Blob later)
- docker-compose for local SQL Server

What this includes (Student Management only)
- Admission form (personal, academic, guardian details)
- Class & Section seed data & dropdowns
- Unique AdmissionNo validation
- Roll number assignment per AcademicYear + Class + Section (resets each year)
- Document upload (PDF/JPEG/PNG, max 10MB) stored on server filesystem
- Backend APIs for admitting students, validating AdmissionNo, uploading documents
- EF Core migration script and seed data for classes/sections
- Scripts to scaffold the solution, install dependencies and produce a build-ready ZIP

How to use
1. Requirements:
   - dotnet 10 SDK
   - node 20+ and npm
   - Angular CLI (npm i -g @angular/cli) OR the script will use npx
   - docker (for SQL Server via docker-compose) OR an existing SQL Server connection string

2. Steps (Linux/macOS):
   - chmod +x scripts/create-solution.sh
   - ./scripts/create-solution.sh
   This will:
   - create the solution and projects under ./sms-solution
   - copy template files into projects
   - run `dotnet restore`, `npm install` for the Angular app
   - set up EF migration and apply it to the dockerized SQL Server
   - build the backend and frontend and produce `sms-feature-student-management.zip`

3. Steps (Windows PowerShell):
   - Run scripts/create-solution.ps1 as administrator (PowerShell)
   - Follow the prompts

Config
- Backend connection string can be found in `src/Api/SMS.Api/appsettings.Development.json`.
- File uploads are saved to `uploads/` folder in the API project by default.
- No secrets are committed; replace placeholders with real values if needed.

Seed admin account
- A seed Admin user is created in the Users table (if you add the optional Identity integration).
- For this feature we scaffold JWT placeholders; you will need to wire real identity in later modules.

If you want me to push these files to the GitHub branch for you, grant me write access or run the script locally and push the generated branch (recommended).

If you are ready, run the create script. If you want me to output the full file contents for every file inline here (so you can paste them into the repo instead of running the script), say "Show all files inline" and I will print every source file for you.
