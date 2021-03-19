using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ProductShop.DTO
{
    public class GlobalUsers
    {
        public int UsersCount { get; set; }

        public UserPersonalInfoDTO[] Users { get; set; }
    }
}
