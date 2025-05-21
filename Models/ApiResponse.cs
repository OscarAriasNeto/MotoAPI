﻿namespace MotoAPI.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        public ApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}