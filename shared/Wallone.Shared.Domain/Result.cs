namespace Wallone.Shared.Domain
{
    public class Result
    {
        public virtual bool IsSuccess => !ErrorMessages.Any();
        public IEnumerable<string> ErrorMessages { get; init; } = Enumerable.Empty<string>();
    }

    public sealed class Result<T> : Result
    {
        public override bool IsSuccess => Data != null;
        public T? Data { get; init; }
    }
}