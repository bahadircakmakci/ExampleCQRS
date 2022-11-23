using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Repositories.Base.Threading
{
    public static class CancellationTokenProviderExtensions
    {
        public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider, CancellationToken prefferedValue = default)
        {
            return prefferedValue == default || prefferedValue == CancellationToken.None
                ? provider.Token
                : prefferedValue;
        }
    }
}
