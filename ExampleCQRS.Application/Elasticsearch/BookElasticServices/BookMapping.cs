using ExampleCQRS.Domain.Entities.Concrete;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Application.Elasticsearch.BookElasticServices
{
    public static class Mapping
    {
        public static CreateIndexDescriptor BookMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Book>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.Name))
                .Text(t => t.Name(n => n.Description))
                .Text(t => t.Name(n => n.Author))

            )
            );
        }
    }
}
