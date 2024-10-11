using Wallone.Shared.Services.Interfaces;

namespace Wallone.Shared.Services.Implementations
{
    internal sealed class GuidGenerator : IGuidGenerator
    {
        public Guid Create() => Guid.NewGuid();
    }
}