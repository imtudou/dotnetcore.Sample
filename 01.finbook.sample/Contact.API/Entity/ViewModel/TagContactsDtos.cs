using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.ViewModel
{
    public class TagContactsDtos
    {
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public List<string> Tags { get; set; }
    }
}
