namespace webapisolution.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }  // Ensure this matches your table schema
    }

}
