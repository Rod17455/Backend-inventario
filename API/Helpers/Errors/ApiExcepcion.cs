namespace API.Helpers.Errors;

public class ApiExcepcion : ApiResponse
{
    public ApiExcepcion(int statusCode, string message = null, string details = null) :
        base(statusCode, message)
    {
        Details = details;
    }

    public string Details { get; set; }
}
