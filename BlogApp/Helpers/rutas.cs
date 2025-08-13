namespace BlogApp.Helpers
{
    public static class Rutas
    {
        public const string Direccion = "http://localhost:5263/";
        public const string Post = Direccion + "api/posts/";
        public const string Usuario = Direccion + "api/usuarios/";
        public const string SubidaImagen = Direccion + "api/upload";
        public const string Login = Usuario + "login";
        public const string Registro = Usuario + "registro";
    }
}
