using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingDemo.Models {
    public class Student {
        public int ID { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-z\s]*$", ErrorMessage = "min 1 capital letter, max 50 characters")]
        public string FirstName { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-z\s]*$", ErrorMessage = "min 1 capital letter, max 50 characters")]
        public string LastName { get; set; }

        [Range(1, 130)]
        public int Age { get; set; }


        public Student() { }

        public Student(int id, string firstName, string lastName, int age) {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }
    }
}
