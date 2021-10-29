using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Dtos
{
    public class TagContactInputViewModel
    {
        public int userid { get; set; }
        public int contactid { get; set; }
        public List<string> tags { get; set; }
    }
}
