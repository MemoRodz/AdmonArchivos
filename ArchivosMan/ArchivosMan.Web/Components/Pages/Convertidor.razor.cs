using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ArchivosMan.BLL.Interfaces;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class Convertidor
    {
        private readonly IUtilidadesService? _utilidadesService;
        [Inject]
        public IUtilidadesService? utilidadesService
        {
            get => _utilidadesService;

            init => _utilidadesService = value;
        }

        private double valorMonto = 0.0;
        private bool mostrarDecimales;
        private string valorEnLetras = string.Empty;
        private decimal temperaturaEntrada = 0.0m;
        private string unidadEntrada = "C"; 
        private decimal temperaturaSalida = 0.0m;

        private void NumeroLetra()
        {
            valorEnLetras = _utilidadesService.ConvertirNumeroALetras(valorMonto, mostrarDecimales);
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
            if (_utilidadesService is null) return;

            switch (unidadEntrada.ToUpper())
            {
                case "C":
                    temperaturaSalida = _utilidadesService.fahrenheit(temperaturaEntrada);
                    break;
                case "F":
                    temperaturaSalida = _utilidadesService.celsiusF(temperaturaEntrada);
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

    }
}
