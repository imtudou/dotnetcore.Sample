using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Entity.ViewModel
{
    public class CheckOrCreateAppUserViewModel
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public byte Gender { get; set; }
    }   
}
