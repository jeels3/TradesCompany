using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TradesCompany.Domain.Entities
{
    public class Address
    {
        public int  Id { get; set; }
        public string Street { get; set; } 
        public string? Landmark { get; set; }
        public string City { get; set; }  
        public string State { get; set; } 
        public string PostalCode { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 
    }
}
