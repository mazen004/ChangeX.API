namespace ChangeX.BLL.DTOs
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResponse<T> Ok(T data, string message = "")
            => new() { Success = true, Data = data, Message = message, StatusCode = 200 };

        public static ServiceResponse<T> Fail(string message, int statusCode = 400)
            => new() { Success = false, Message = message, StatusCode = statusCode };
    }
}
