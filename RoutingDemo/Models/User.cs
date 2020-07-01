using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingDemo.Models {
    public class User {
        public int ID { get; set; }

        //varchar 50
        public string FirstName { get; set; }
        //varchar 50
        public string LastName { get; set; }
        public string Password { get; set; } // shall be hashed: można to zrobić samemu albo biblioteka Microsoft.Identity
        // są różne algorytmy hashowania => zawsze używamy algorytmów asymetrycznych + dodajemy sól (żeby utrudnić zgadnięcie rozkładu funkcji hashującej)
        // alg asymetryczny: łatwo zaszyfrować w 1 stronę, w 2 stronę inna funkcja, która jest b. droga obliczeniowo do znalezenia

        //varchar 254
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public System.Guid ActivationCode { get; set; } = Guid.NewGuid();

        [DataType(DataType.Date)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;


    }
}
