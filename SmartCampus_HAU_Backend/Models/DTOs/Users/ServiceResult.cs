namespace SmartCampus_HAU_Backend.Models.DTOs.Users
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult<T> Success(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResult<T> Failure(string message, List<string> errors = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

    public class ServiceResult : ServiceResult<object>
    {
        public static ServiceResult Success(string message = "")
        {
            return new ServiceResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static new ServiceResult Failure(string message, List<string> errors = null)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
