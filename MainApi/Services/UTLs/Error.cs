namespace Services.UTLs
{
    public class Error
    {
        public string AtrributeName { get; }

        public string Message { get; set; }

        public Error(string atrributeName, string message)
        {
            AtrributeName = atrributeName;
            Message = message;
        }
    }
}