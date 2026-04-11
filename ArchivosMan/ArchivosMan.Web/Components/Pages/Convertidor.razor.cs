using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ArchivosMan.BLL.Interfaces;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Convertidor
    {
        [Inject] private IUtilidadesService UtilidadesService { get; set; } = default!;
        [Inject] private ICryptoService CryptoService { get; set; } = default!;

        private double valorMonto = 0.0;
        private bool mostrarDecimales;
        private string valorEnLetras = string.Empty;
        private decimal temperaturaEntrada = 0.0m;
        private string unidadEntrada = "C"; 
        private decimal temperaturaSalida = 0.0m;
        private string entrada = string.Empty;
        private string salida = string.Empty;

        private void NumeroLetra()
        {
            valorEnLetras = UtilidadesService.ConvertirNumeroALetras(valorMonto, mostrarDecimales);
        }

        private void ConvertNumLetraOnKeyDownHandler(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                NumeroLetra();
            }
        }

        private void LimpiarControlesNumLetras()
        {
            valorMonto = 0;
            valorEnLetras = string.Empty;
            mostrarDecimales = false;
        }

        private void ConvertirTemperatura()
        {
            if (UtilidadesService is null) return;

            switch (unidadEntrada.ToUpper())
            {
                case "C":
                    temperaturaSalida = UtilidadesService.fahrenheit(temperaturaEntrada);
                    break;
                case "F":
                    temperaturaSalida = UtilidadesService.celsiusF(temperaturaEntrada);
                    break;
                default:
                    temperaturaSalida = 0.0m; break;
            }
        }

        private void LimpiarTemperatura()
        {
            temperaturaEntrada = 0.0m;
            temperaturaSalida = 0.0m;
            unidadEntrada = "C";
        }

        private string ObtenerResultadoFormateado()
        {
            if (unidadEntrada == "C")
            {
                return $"{temperaturaEntrada:0.##} °C = {temperaturaSalida:0.##} °F";
            }
            else
            {
                return $"{temperaturaEntrada:0.##} °F = {temperaturaSalida:0.##} °C";
            }
        }

        private void OnKeyDownHandlerCadenaEncriptada(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                ObtenerCadena();
            }
        }

        private void ObtenerCadena()
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                salida = $"Cadena vacía.";
                return;
            }

            try
            {
                salida = $"Cadena cifrada AES:\n{CryptoService.Encrypt(entrada)}";
                entrada = string.Empty; // Limpiar la entrada después de cifrar
            }
            catch (Exception ex)
            {
                salida = $"Ocurrió un error al cifrar la cadena: {ex.Message}";
            }
        }

        private void LimpiarCadena()
        {
            entrada = string.Empty;
            salida = string.Empty;
        }

        private void OnKeyDownHandlerCadenaDesencriptada(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                ResolverCadena();
            }
        }

        private void ResolverCadena()
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                salida = $"Cadena vacía.";
                return;
            }
            try
            {
                salida = $"Cadena descifrada AES:\n{CryptoService.Decrypt(entrada)}";
                entrada = string.Empty;
            }
            catch (Exception ex)
            {
                salida = $"Ocurrió un error al descifrar la cadena: {ex.Message}";
            }
        }
    }
}
