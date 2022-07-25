namespace Utils.Results;

public class Result : IResult
{
    public bool IsSuccess { get; set; } = true;
    public ResultCodes Code { get; set; } = ResultCodes.Success;
    public List<Error> Errors { get; set; } = new List<Error>();

    public R CastTo<R>() where R : IResult, new()
    {
        return new R
        {
            IsSuccess = IsSuccess,
            Code = Code,
            Errors = Errors
        };
    }

    public IResult WithCode(ResultCodes code)
    {
        Code = code;
        return this;
    }

    public IResult<TContentType> WithContent<TContentType>(TContentType content)
    {
        return new Result<TContentType>() { Code = Code, Errors = Errors, Content = content };
    }

    public IResult WithErrors(params Error[] errors)
    {
        Errors.AddRange(errors);
        if (Errors.Any())
            IsSuccess = false;
        return this;
    }

    public IResult WithSuccess(bool success)
    {
        IsSuccess = success;
        return this;
    }

    public IResult WithFailure(string failureDetails)
    {
        Code = ResultCodes.Failure;
        IsSuccess = false;
        return WithErrors(new Error { Detail = failureDetails });
    }

    public static IResult OfSuccess()
    {
        return new Result { IsSuccess = true, Code = ResultCodes.Success };
    }

    public static IResult<TContent> OfSuccess<TContent>(TContent? content = default)
    {
        return new Result<TContent> { IsSuccess = true, Code = ResultCodes.Success, Content = content };
    }

    public static IResult OfFailure(string failureDetails)
    {
        return new Result().WithFailure(failureDetails);
    }

    public static IResult OfNotFoundResult(string name)
    {
        return new Result
        {
            IsSuccess = false,
            Code = ResultCodes.NotFound
        }.WithErrors(Error.OfNotFound(name));
    }

    public static IResult OfUnauthorizedResult(string resource)
    {
        return new Result
        {
            IsSuccess = false,
            Code = ResultCodes.InsufficientPermission
        }.WithErrors(Error.OfUnauthorized(resource));
    }

    public static IResult OfInternalFailure(string message)
    {
        return new Result
        {
            IsSuccess = false,
            Code = ResultCodes.InternalFailure
        }.WithErrors(new Error { Detail = message });
    }

    public Result Build()
    {
        return this;
    }

    public Result<TContentType> Build<TContentType>()
    {
        return new Result<TContentType>() { Code = Code, Errors = Errors, IsSuccess = IsSuccess, Content = default };
    }
}

public class Result<TContent> : Result, IResult<TContent>
{
    public TContent? Content { get; set; } = default;

    Result<TContent> IResult<TContent>.Build()
    {
        return this;
    }

    R IResult<TContent>.CastTo<R>()
    {

        return new R
        {
            IsSuccess = IsSuccess,
            Code = Code,
            Content = Content,
            Errors = Errors
        };
    }
}