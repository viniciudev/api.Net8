using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public  class Client:BaseModel
    {
        public Guid keyIdentifier { get; set; }
        public required string CorporateName { get; set; }
        public string? Cnpj { get; set; }
        public string ?Telephone { get; set; }
        public string ?ZipCode { get; set; }
        public string ?Street { get; set; }
        public int Number { get; set; }
        public string? Email { get; set; }
        public Client()
        {
            keyIdentifier = Guid.NewGuid(); // Gera um novo GUID para a chave
        }

    }
   
}
