namespace ArchivosMan.BLL.Interfaces
{
    public interface IUtilidadesService
    {
        string ConvertirNumeroALetras(double Numero, bool CentimosEnLetra = false);

        string NumeroRecursivo(long Numero);

        decimal celsiusF(decimal fahrenheit);

        decimal fahrenheit(decimal celsius);
    }
}
