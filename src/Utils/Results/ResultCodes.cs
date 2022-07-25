using Ardalis.SmartEnum;

namespace Utils.Results;

public class ResultCodes : SmartEnum<ResultCodes, int>
{
    public static readonly ResultCodes Success = new(nameof(Success), 1);
    public static readonly ResultCodes InsufficientPermission = new(nameof(InsufficientPermission), 2);
    public static readonly ResultCodes Failure = new(nameof(Failure), 3);
    public static readonly ResultCodes NotFound = new(nameof(NotFound), 4);
    public static readonly ResultCodes InternalFailure = new(nameof(InternalFailure), 5);
    public ResultCodes(string name, int value) : base(name, value) { }
}
