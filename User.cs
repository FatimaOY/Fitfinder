using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class User
    {
        //Properties 
        public string Name { get; set; }
        public  string Surname { get; set; }

        public int ID { get; set; }
        public string EmailAdress { get; set; }
        private string Password { get; set; }

        public Role UserRole { get; set; }

        public Data data = new Data();

        // Constructor for initialization
        public User(int userId, string name, string surname, string password, string emailAdress)
        {
            ID = userId;
            Name = name;
            Surname = surname;
            Password = password;
            EmailAdress = emailAdress;
        }

        public override string ToString()
        {
            return $"User: {Name} {Surname} ({EmailAdress})";
        }


    }

    public enum Role
    {
        Client,
        Trainer
    }

}
