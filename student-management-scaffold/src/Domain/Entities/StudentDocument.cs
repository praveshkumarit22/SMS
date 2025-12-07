using System;

namespace SMS.Domain.Entities
{
    public class StudentDocument
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
