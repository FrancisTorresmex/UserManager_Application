namespace userManagerApplication.Models
{
    public class ScreenUserModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? URL { get; set; }
        public int IdUser { get; set; }
        public bool UserAccess { get; set; }
    }
}
