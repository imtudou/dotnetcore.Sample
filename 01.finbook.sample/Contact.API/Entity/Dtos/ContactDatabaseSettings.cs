using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Dtos
{
    public class ContactDataBaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }

}
