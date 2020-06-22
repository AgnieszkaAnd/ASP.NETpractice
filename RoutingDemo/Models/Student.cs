using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingDemo.Models {
    public class Student {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Student(int id, string firstName, string lastName, int age) {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
        }
    }
}
