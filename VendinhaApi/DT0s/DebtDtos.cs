using System.ComponentModel.DataAnnotations;

namespace VendinhaApi.DTOs
{
    public class CreateDebtDto
    {
        [Required(ErrorMessage = "O valor da dívida é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Value { get; set; }
    }

    public class DebtResponseDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public int ClientId { get; set; }
    }
}