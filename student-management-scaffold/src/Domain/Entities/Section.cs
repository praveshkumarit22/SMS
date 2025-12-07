namespace SMS.Domain.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; } = null!;
    }
}
