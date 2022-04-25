using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAppBackEnd.Models.DTO
{
    public class PasswordDTO
    {
        public string? Hash { get; set; }
        public string? Salt { get; set; }
    }
}