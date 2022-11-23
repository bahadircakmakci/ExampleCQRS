using ExampleCQRS.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Domain.Entities.Concrete
{
    public class Book : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public string Author { get; set; }
    }
}
