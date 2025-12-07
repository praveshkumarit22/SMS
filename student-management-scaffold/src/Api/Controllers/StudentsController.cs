using Microsoft.AspNetCore.Mvc;
using SMS.Infrastructure.Persistence;
using SMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolDbContext _db;
        private readonly IFileStorageService _fileStorage;

        public StudentsController(SchoolDbContext db, IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _db.Classes.ToListAsync();
            return Ok(classes);
        }

        [HttpGet("sections/{classId}")]
        public async Task<IActionResult> GetSections(int classId)
        {
            var sections = await _db.Sections.Where(s => s.ClassId == classId).ToListAsync();
            return Ok(sections);
        }

        [HttpGet("validate-admissionno")]
        public async Task<IActionResult> ValidateAdmissionNo([FromQuery]string admissionNo)
        {
            if (string.IsNullOrWhiteSpace(admissionNo)) return BadRequest("admissionNo is required");
            var exists = await _db.Students.AnyAsync(s => s.AdmissionNo == admissionNo);
            return Ok(new { admissionNo, isUnique = !exists });
        }

        [HttpPost("admit")]
        public async Task<IActionResult> Admit([FromBody] Student model)
        {
            if (model == null) return BadRequest();

            // Validate unique admission no
            var exists = await _db.Students.AnyAsync(s => s.AdmissionNo == model.AdmissionNo);
            if (exists) return Conflict(new { message = "Admission number already exists" });

            // Assign Academic Year if not provided
            if (model.AcademicYear == 0) model.AcademicYear = DateTime.UtcNow.Year;

            // Compute roll number per AcademicYear + Class + Section
            var count = await _db.Students
                .Where(s => s.ClassId == model.ClassId && s.SectionId == model.SectionId && s.AcademicYear == model.AcademicYear)
                .CountAsync();

            var seq = count + 1;
            var className = await _db.Classes.Where(c => c.Id == model.ClassId).Select(c => c.Name).FirstOrDefaultAsync() ?? "C";
            var sectionName = await _db.Sections.Where(s => s.Id == model.SectionId).Select(s => s.Name).FirstOrDefaultAsync() ?? "X";

            model.RollNumber = $"{model.AcademicYear}-{className}-{sectionName}-{seq.ToString("D4")}";

            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;

            _db.Students.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(Guid id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost("{id}/documents")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadDocument(Guid id, IFormFile file)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();

            if (file == null) return BadRequest("file is required");
            var allowed = new[] { "application/pdf", "image/png", "image/jpeg" };
            if (!allowed.Contains(file.ContentType))
                return BadRequest("Only PDF, PNG and JPEG are allowed");

            var path = await _fileStorage.SaveFileAsync(file, "students");
            var doc = new StudentDocument
            {
                StudentId = id,
                FileName = Path.GetFileName(file.FileName),
                FilePath = path,
                ContentType = file.ContentType
            };

            _db.StudentDocuments.Add(doc);
            await _db.SaveChangesAsync();
            return Ok(doc);
        }
    }
}
