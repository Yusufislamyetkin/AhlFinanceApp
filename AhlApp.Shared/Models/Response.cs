using System;
using System.Collections.Generic;

namespace AhlApp.Shared.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; } 

        public static Response<T> SuccessResponse(T data)
        {
            return new Response<T>
            {
                Success = true,
                Data = data,
                Errors = null 
            };
        }

        public static Response<T> ErrorResponse(string errorMessage)
        {
            return new Response<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                Errors = null 
            };
        }

        public static Response<T> ErrorResponse(string errorMessage, IEnumerable<string> errors)
        {
            return new Response<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                Errors = errors != null ? new List<string>(errors) : null
            };
        }
    }
}
