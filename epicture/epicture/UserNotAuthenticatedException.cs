using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicture
{
    public class UserNotAuthenticatedException : Exception
    {
        public UserNotAuthenticatedException(string message) : base(message)
        {

        }
    }
}
