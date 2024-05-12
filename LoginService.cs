using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fitfinder
{
    public class LoginService
    {
        private readonly Data _data;

        public LoginService(Data data)
        {
            _data = data; // Inject Data class for database operations
        }

        public bool Login(string email, string password)
        {
            bool isValid = _data.ValidateUserLogin(email, password);

            if (isValid)
            {
                UserInfo user = _data.GetUserByEmail(email);

                if (user != null)
                {
                    UserSession.CurrentUser = user; // Store user info in session
                    int userId = Convert.ToInt32(user.Email); // Example: Get user ID from UserInfo
                    UserSession.UserRole = _data.GetUserRole(userId); // Assign user role in session

                    return true; // Login successful
                }
            }

            return false; // Login failed
        }
    }
}
