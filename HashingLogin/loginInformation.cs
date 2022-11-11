using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashingLogin
{
    public class loginInformation
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Userpassword { get; set; }
        public string Salt { get; set; }

    }
}
