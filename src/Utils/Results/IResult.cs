namespace Utils.Results;

public interface IResult
{
    public bool IsSuccess { get; set; }
    public ResultCodes Code { get; set; }
    public List<Error> Errors { get; set; }
    public IResult WithSuccess(bool success);
    public IResult WithCode(ResultCodes code);
    public IResult WithErrors(params Error[] errors);
    public IResult WithFailure(string failureDetails);
    public IResult<TContent> WithContent<TContent>(TContent content);
    public Result Build();
    public Result<TContent> Build<TContent>();
    public R CastTo<R>() where R : IResult, new();
}

public interface IResult<TContentType> : IResult
{
    public TContentType? Content { get; set; }
    public new Result<TContentType> Build();
    public new R CastTo<R>() where R : IResult<TContentType>, new();
}
