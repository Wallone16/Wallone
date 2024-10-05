using Wallone.Shared.Services.Interfaces;

namespace Wallone.Shared.Services.Implementations
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTimeUtc() => DateTime.UtcNow;
    }
}