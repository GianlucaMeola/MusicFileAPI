using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace MusicFileAPI.Extention
{
    public class ApiErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }
    }

    public static class ControllerExtension
    {
        public static ObjectResult ErrorResult(this Controller c, HttpStatusCode statusCode, string errorCode, string description)
        {
            var output = new ApiErrorResponse
            {
                ErrorCode = errorCode,
                Description = description
            };

            var result = new ObjectResult(output)
            {
                StatusCode = (int)statusCode
            };

            return result;
        }

        public static ObjectResult ErrorResult(this Controller c, HttpStatusCode statusCode, IList<ValidationFailure> errors)
        {
            var apiErrors = new List<ApiErrorResponse>();
            foreach (var e in errors)
            {
                apiErrors.Add(
                    new ApiErrorResponse
                    {
                        ErrorCode = e.ErrorCode,
                        Description = e.ErrorMessage
                    }
                );
            }

            var result = new ObjectResult(apiErrors)
            {
                StatusCode = (int)statusCode
            };

            return result;
        }
    }
}
