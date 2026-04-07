namespace ArchivosMan.BLL
{
    public class Constantes
    {
        public static class TipoCarpetaArchivo
        {
            public static string Usuario = "IMAGENES_USUARIO";
            public static string Producto = "IMAGENES_PRODUCTO";
            public static string Logo = "IMAGENES_LOGO";
            public static string Icono = "IMAGENES_ICONO";
            public static string Media = "IMAGENES_MEDIA";
        }
        public static class CategoriaConstantes
        {
            public const string USUARIO = "USUARIO";
            public const string PRODUCTO = "PRODUCTO";
            public const string LOGO = "LOGO";
            public const string ICONO = "ICONO";
            public const string MEDIA = "MEDIA";
        }
        public static class Servicios
        {
            public const string Storage = "FireBase_Storage";
            public const string Mailing = "Servicio_Correo";
            public const string Template = "RestablecerClave.html";
        }

        public static class TipoPlantilla
        {
            public const string Actualiza = "Actualizada";
            public const string Crea = "Creada";
            public const string Restablece = "Restablecida";
        }
        public static class Usuario
        {
            public const int Correo = 0;
            public const int RolDefault = 4;
            public const int Super = 0;
            public const int Admin = 1;
            public const int Supervisor = 2;
            public const int Empleado = 3;
        }

        public static class CatImagenes
        {
            public const int UsuarioPic = 6;
            public const int ProdutoPic = 7;
            public const int LogoPic = 8;
            public const int IconoPic = 9;
            public const int MediaPic = 10;
        }

        public static readonly List<int> idsPermitidos = new() { 6, 7, 8, 9, 10 };
    }
}
