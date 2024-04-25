using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    /*public enum Gender
    {   
        Female,
        Male,
        X
    }*/
    public class Gender
    {
        public int GenderId { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        // Constructor
        public Gender(int genderId, string name)
        {
            GenderId = genderId;
            Name = name;
        }
    }
}
