namespace TestUserApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public int Age { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
