using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs
{
    public record ProductCreateDto(
        [Required(ErrorMessage = "El nombre es mandatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        string Name,

        [Range(0.01, 50000.00, ErrorMessage = "El precio debe ser mayor a 0.")]
        decimal Price,

        [Range(0, 1000, ErrorMessage = "El Stock debe estar entre 0 y 1000.")]
        int Stock
    );
}
