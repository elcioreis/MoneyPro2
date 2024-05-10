using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MoneyPro2.API.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErros(this ModelStateDictionary modelState, string error = "")
    {
        var result = new List<string>();

        if (!String.IsNullOrEmpty(error))
        {
            result.Add(error);
        }

        foreach (var item in modelState.Values)
        {
            result.AddRange(item.Errors.Select(error => error.ErrorMessage));
        }

        return result;
    }
}
