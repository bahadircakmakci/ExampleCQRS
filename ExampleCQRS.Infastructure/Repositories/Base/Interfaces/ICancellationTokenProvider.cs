using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Interfaces
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
    }
}
