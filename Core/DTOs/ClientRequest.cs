using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ClientRequest
    {
     
        public required string CorporateName { get; set; }
        public string? Cnpj { get; set; }
        public string? Telephone { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Email { get; set; }
      
    }
}
