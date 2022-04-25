using DataPUE;

namespace PUE.Controllers
{
    using System;

    public class Login
    {
        public enum ButtonType { CLOSE, CANCEL, ACCEPT }

        /// <summary>
        /// user verifies existence
        /// </summary>
        /// <param name="UserName">User</param>
        /// <param name="Password">Password</param>
        /// <returns></returns>
        public static bool ValidateUser(String UserName, String Password)
        {
            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
                return false;

            CATALOGOS cat = new CATALOGOS();
            if (cat.getLogin(UserName, Password))
                return true;
            else
                return false;
        }
    }
}
