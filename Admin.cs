﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Admin : User
    {
        public int AdminId { get; set; }
        public int PersonId { get { return UserId; } }

        public Admin(int adminId, string name, string surname, string email, string password, byte[] profilePic, int genderId, string favoriteColor, string dreamDestination, string favoriteAnimal)
            : base(name, surname, email, password, profilePic, genderId, favoriteColor, dreamDestination, favoriteAnimal)
        {
            AdminId = adminId;
        }
    }

}
