namespace server.Models;

public class ControllerServiceResponse
{
    public bool IsSuccess { get; set; }
    public object? Value { get; set; }
    public string? ErrorMessage { get; set; }
}