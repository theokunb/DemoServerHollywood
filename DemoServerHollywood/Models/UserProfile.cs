using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoServerHollywood.Models
{
    public class UserProfile
    {
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }


        public static bool operator ==(UserProfile left, UserProfile right)
        {
            return left.DisplayName == right.DisplayName && left.IsAdmin == right.IsAdmin && left.PhoneNumber == right.PhoneNumber;
        }
        public static bool operator !=(UserProfile left, UserProfile right)
        {
            return left.DisplayName != right.DisplayName || left.IsAdmin != right.IsAdmin || left.PhoneNumber != right.PhoneNumber;
        }
    }
}
