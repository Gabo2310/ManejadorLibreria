using System.ComponentModel.DataAnnotations;

namespace ManejadorLibreria.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1450, 2025, ErrorMessage = "El año debe estar entre 1450 y 2025")]
        public int Year { get; set; }

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [RegularExpression(@"^(?:\d{10}|\d{13})$", ErrorMessage = "El ISBN debe tener 10 o 13 dígitos")]
        public string ISBN { get; set; } = string.Empty;
    }
}