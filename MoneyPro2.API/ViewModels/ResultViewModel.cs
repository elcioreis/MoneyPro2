namespace MoneyPro2.API.ViewModels;

public class ResultViewModel<T> where T : class
{
    public ResultViewModel(T data, List<string> errors)
    {
        Data = data;
        Errors = errors;
    }

    public ResultViewModel(T data)
    {
        Data = data;
    }

    public ResultViewModel(List<string> errors)
    {
        Errors = errors;
    }

    public ResultViewModel(string error)
    {
        Errors.Add(error);
    }

    public T Data { get; private set; } = null!;
    public List<string> Errors { get; private set; } = new();
}
