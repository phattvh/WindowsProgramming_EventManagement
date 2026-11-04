using System;

namespace EventManagement.Core.Common
{
    /// <summary>
    /// Lớp kết quả trả về chuẩn mực của tầng BLL (ServiceResult)
    /// </summary>
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }

        public static ServiceResult Success(string message = "Thao tác thành công!")
        {
            return new ServiceResult { IsSuccess = true, Message = message };
        }

        public static ServiceResult Failed(string message, string? errorCode = null)
        {
            return new ServiceResult { IsSuccess = false, Message = message, ErrorCode = errorCode };
        }
    }

    /// <summary>
    /// Lớp kết quả trả về kèm dữ liệu (Generic ServiceResult)
    /// </summary>
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data, string message = "Thao tác thành công!")
        {
            return new ServiceResult<T> { IsSuccess = true, Message = message, Data = data };
        }

        public new static ServiceResult<T> Failed(string message, string? errorCode = null)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message, ErrorCode = errorCode };
        }
    }
}
