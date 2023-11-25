using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Id cant be blank")]
        [Range(0, int.MaxValue, ErrorMessage = "Id must be btw 0 and max value of int")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Person Name cant be blank")]
        public string? ProductName { get; set; }

        public override string ToString()
        {
            return $"Product Id : {Id}, Product Name: {ProductName}";
        }
    }
}
