using System.Collections.Generic;

namespace Services.UTLs
{
    public class CustomResult
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; }

        public string TraceId { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public CustomResult(ErrorException exception)
        {
            Status = exception.Code;
            Title = exception.Tittle;
            Errors = new Dictionary<string, string>();

            foreach (Error err in exception.Errors)
            {
                Errors.Add(err.AtrributeName, err.Message);
            }
        }
    }
}