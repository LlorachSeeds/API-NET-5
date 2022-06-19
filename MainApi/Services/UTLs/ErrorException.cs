using System;
using System.Collections.Generic;

namespace Services.UTLs
{
    public class ErrorException : Exception
    {
        public int Code { get; set; }

        public List<Error> Errors { get; set; }

        public string Tittle { get; set; }

        public ErrorException(int code, string tittle, List<Error> errors)
        {
            Code = code;
            Tittle = tittle;
            Errors = errors;
        }

        public ErrorException(int code, string methodAffected, string errorMessage)
        {
            Errors = new List<Error>();

            Code = code;
            Tittle = "Error en" + methodAffected;
            Error error = new Error(methodAffected, errorMessage);
            Errors.Add(error);
        }
    }
}