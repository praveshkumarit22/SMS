# School Management System — Skeleton (ASP.NET 9 + Angular 20)

Contents:
- Backend (.NET 9)
  - Models, DbContext, Services, Controller (Student Management)
  - Demonstrates unique StudentId validation, roll assignment, and document upload to Azure Blob Storage
- Frontend (Angular 20)
  - Feature module for Student Management
  - Reactive admission form with dropdowns, file upload, and service methods

Design & Implementation Notes:
- Normalize tables: Class, Section, Subject, TeacherSubjectMap in SQL example (see schema.sql)
- Validate unique student identifier in service layer + DB unique index
- Assign roll number per class+section (simple max+1 strategy demonstrated; consider gaps if deletions occur)
- Documents stored in Azure Blob Storage (pattern provided); replace with file-system implementation if needed
- Use Angular Signals or RxJS BehaviourSubjects for reactive state; example uses reactive forms
- Secure APIs with JWT + role claims (not included in this skeleton; add Authorize attributes and configure authentication)

Next steps:
- I can expand modules (Attendance, Fees, Exams, Promotion, Transport, Admin, Notifications, Dashboard)
- Or integrate this scaffold into your repository and open a PR (tell me owner/repo/branch).