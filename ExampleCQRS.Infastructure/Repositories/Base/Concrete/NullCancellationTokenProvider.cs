using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Concrete
{
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public CancellationToken Token { get; } = CancellationToken.None;

        private NullCancellationTokenProvider()
        {

        }
    }
}
