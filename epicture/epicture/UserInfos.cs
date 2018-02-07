using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicture
{
    public class UserInfos
    {
        public string FullName;
        public string Username;
        public string UserId;

        public UserInfos(string fullName, string username, string userId)
        {
            FullName = fullName;
            Username = username;
            UserId = userId;
        }
    }
}
