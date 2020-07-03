using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingDemo.Models {
    public class User {
        public int ID { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-z\s]*$", ErrorMessage = "min 1 capital letter, max 50 characters")]
        public string FirstName { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-z\s]*$", ErrorMessage = "min 1 capital letter, max 50 characters")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression( @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,20}$",
                            ErrorMessage = " you shall input 8-20 characters, at least one letter, one number and one special character")]
        // Minimum eight characters, at least one letter, one number and one special character:
        public string Password { get; set; } // shall be hashed: można to zrobić samemu albo biblioteka Microsoft.Identity
        // są różne algorytmy hashowania => zawsze używamy algorytmów asymetrycznych + dodajemy sól (żeby utrudnić zgadnięcie rozkładu funkcji hashującej)
        // alg asymetryczny: łatwo zaszyfrować w 1 stronę, w 2 stronę inna funkcja, która jest b. droga obliczeniowo do znalezenia


        public string Email { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public System.Guid ActivationCode { get; set; } = Guid.NewGuid();

        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}
