using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Person
    {
        //Properties 
        public string Name { get; set; }
        public  string Surname { get; set; }

        public int ID { get; set; }
        public string EmailAdress { get; set; }
        private string Password { get; set; }

        public Role UserRole { get; set; }  




    }

    public enum Role
    {
        Client,
        Trainer
    }

}
