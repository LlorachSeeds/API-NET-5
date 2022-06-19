namespace Services.UTLs
{
    public static class Errors
    {
        public static string UnknownError { get; } = "Error Desconocido";

        public static string WrongAttributes { get; } = "Parametros Incorrectos";

        public static string UserNotAuthorized { get; } = "Usuario No Autorizado";

        public static string ElementNotFound { get; } = "El o los elementos buscados no se encuentran disponibles";

        public static string DateIncorrect { get; } = "Fecha Incorrecta";

        public static string AlreadyExist { get; } = "Imposible crear, ya existe";

        public static string EmailExist(string email)
        {
            return "El E-Mail " + email + " ya existe";
        }
    }
}