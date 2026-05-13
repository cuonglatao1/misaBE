namespace MisaBE.Common.DTOs
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ServiceResult Ok(object? data = null, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        public static ServiceResult Fail(string message, List<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}
