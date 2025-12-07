using System;

namespace SMS.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string AdmissionNo { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? RollNumber { get; set; }
        public int AcademicYear { get; set; }
        public string? GuardianName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
