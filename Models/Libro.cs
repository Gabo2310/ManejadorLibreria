namespace ManejadorLibreria.Models
{
    public class Libro
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
