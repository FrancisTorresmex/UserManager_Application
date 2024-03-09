namespace userManagerApplication.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public object? Data { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<string> ErrorLst { get; set; }
    }
}
