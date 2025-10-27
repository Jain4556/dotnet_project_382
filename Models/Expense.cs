using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be > 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Foreign key to Category
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
