using System.ComponentModel.DataAnnotations;

namespace SportNation2.Data.Models
{
    public class RegisterViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        public string Genre { get; set; }
    }
}
