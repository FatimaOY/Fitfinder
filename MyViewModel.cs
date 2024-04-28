using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class MyViewModel
    {
        private Data _data; // Assuming Data class is already defined

        public MyViewModel()
        {
            _data = new Data(); // Initialize the Data instance
        }

        public void AddNewClient(Client client)
        {
            int userId = _data.InsertUser(client); // Insert a new user and get the ID

            if (userId != -1)
            {
                _data.InsertClient(userId, client); // Insert into Clients table
            }
        }

        public void AddNewTrainer(Trainer trainer)
        {
            int userId = _data.InsertUser(trainer); // Insert a new user and get the ID

            if (userId != -1)
            {
                _data.InsertTrainer(userId, trainer); // Insert into Trainers table
            }
        }
    }
}
