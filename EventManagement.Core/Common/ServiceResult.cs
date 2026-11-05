using System;
using System.Collections.Generic;

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
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult Success(string message = "Thao tác thành công!")
        {
            return new ServiceResult { IsSuccess = true, Message = message };
        }

        public static ServiceResult Ok(string message = "Thao tác thành công!")
        {
            return Success(message);
        }

        public static ServiceResult Failed(string message, string? errorCode = null)
        {
            return new ServiceResult { IsSuccess = false, Message = message, ErrorCode = errorCode };
        }

        public static ServiceResult Failed(string message, List<string>? errors)
        {
            return new ServiceResult { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
        }

        public static ServiceResult Error(string message, string? errorCode = null)
        {
            return Failed(message, errorCode);
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

        public static ServiceResult<T> Ok(T data, string message = "Thao tác thành công!")
        {
            return Success(data, message);
        }

        public new static ServiceResult<T> Failed(string message, string? errorCode = null)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message, ErrorCode = errorCode };
        }

        public new static ServiceResult<T> Failed(string message, List<string>? errors)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
        }

        public new static ServiceResult<T> Error(string message, string? errorCode = null)
        {
            return Failed(message, errorCode);
        }
    }
}
