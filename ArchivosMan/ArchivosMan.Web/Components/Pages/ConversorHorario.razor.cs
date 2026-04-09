using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class ConversorHorario
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        protected string ZonaOrigen { get; set; } = "America/Mexico_City";
        protected string ZonaDestino { get; set; } = "Europe/London";
        protected string FiltroOrigen { get; set; } = string.Empty;
        protected string FiltroDestino { get; set; } = string.Empty;
        protected DateTime FechaHoraEntrada { get; set; } = DateTime.Now;
        protected DateTime? Resultado { get; set; }
        protected string MensajeError { get; set; } = string.Empty;
        protected string DiferenciaHoras { get; set; } = string.Empty;

        private static readonly List<ZonaHoraria> _zonas = CargarZonas();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    var zonaBrowser = await JS.InvokeAsync<string>(
                        "timezoneHelper.getLocalTimeZone");

                    var encontrada = _zonas.FirstOrDefault(z =>
                        z.IanaId.Equals(zonaBrowser,
                        StringComparison.OrdinalIgnoreCase));

                    if (encontrada is not null)
                        ZonaOrigen = encontrada.Id;

                    StateHasChanged();
                }
                catch
                {
                    // Si falla la detección, se queda con el valor por defecto
                }
            }
        }

        protected IEnumerable<ZonaHoraria> ZonasFiltradas(string filtro) =>
            string.IsNullOrWhiteSpace(filtro)
                ? _zonas
                : _zonas.Where(z =>
                    z.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase));

        protected string NombreZona(string id) =>
            _zonas.FirstOrDefault(z => z.Id == id)?.Nombre ?? id;

        protected void UsarAhora()
        {
            FechaHoraEntrada = DateTime.Now;
            Resultado = null;
        }

        protected void Convertir()
        {
            MensajeError = string.Empty;
            Resultado = null;
            DiferenciaHoras = string.Empty;

            if (string.IsNullOrWhiteSpace(ZonaOrigen) ||
                string.IsNullOrWhiteSpace(ZonaDestino))
            {
                MensajeError = "Selecciona ambas zonas horarias.";
                return;
            }

            if (ZonaOrigen == ZonaDestino)
            {
                MensajeError = "Las zonas horarias origen y destino son iguales.";
                return;
            }

            try
            {
                var tzOrigen = TimeZoneInfo.FindSystemTimeZoneById(ZonaOrigen);
                var tzDestino = TimeZoneInfo.FindSystemTimeZoneById(ZonaDestino);

                var fechaOrigen = DateTime.SpecifyKind(FechaHoraEntrada, DateTimeKind.Unspecified);
                var utc = TimeZoneInfo.ConvertTimeToUtc(fechaOrigen, tzOrigen);
                Resultado = TimeZoneInfo.ConvertTimeFromUtc(utc, tzDestino);

                var offsetOrigen = tzOrigen.GetUtcOffset(fechaOrigen);
                var offsetDestino = tzDestino.GetUtcOffset(Resultado.Value);
                var diff = offsetDestino - offsetOrigen;

                string signo = diff.TotalHours >= 0 ? "+" : "-";
                DiferenciaHoras = diff.TotalMinutes % 60 == 0
                    ? $"{signo}{Math.Abs(diff.Hours)}h respecto al origen"
                    : $"{signo}{Math.Abs(diff.Hours)}h {Math.Abs(diff.Minutes)}min respecto al origen";
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al convertir: {ex.Message}";
            }
        }

        protected void InvertirZonas()
        {
            (ZonaOrigen, ZonaDestino) = (ZonaDestino, ZonaOrigen);

            (FiltroOrigen, FiltroDestino) = (FiltroDestino, FiltroOrigen);

            if (Resultado is not null)
            {
                FechaHoraEntrada = Resultado.Value;
                Resultado = null;
                DiferenciaHoras = string.Empty;
            }
        }

        private static List<ZonaHoraria> CargarZonas()
        {
            // Mapeo de IDs de sistema a nombres amigables + IANA ID para detección del navegador
            return new List<ZonaHoraria>
            {
                new("UTC", "UTC - Tiempo Universal Coordinado", "UTC"),
                // América
                new("America/Mexico_City", "Ciudad de México (UTC-6)", "America/Mexico_City"),
                new("America/Cancun", "Cancún (UTC-5)", "America/Cancun"),
                new("America/Tijuana", "Tijuana (UTC-8)", "America/Tijuana"),
                new("America/New_York", "Nueva York (UTC-5)", "America/New_York"),
                new("America/Chicago", "Chicago (UTC-6)", "America/Chicago"),
                new("America/Denver", "Denver (UTC-7)", "America/Denver"),
                new("America/Los_Angeles", "Los Ángeles (UTC-8)", "America/Los_Angeles"),
                new("America/Bogota", "Bogotá (UTC-5)", "America/Bogota"),
                new("America/Lima", "Lima (UTC-5)", "America/Lima"),
                new("America/Santiago", "Santiago de Chile (UTC-4)", "America/Santiago"),
                new("America/Argentina/Buenos_Aires", "Buenos Aires (UTC-3)", "America/Argentina/Buenos_Aires"),
                new("America/Sao_Paulo", "São Paulo (UTC-3)", "America/Sao_Paulo"),
                new("America/Caracas", "Caracas (UTC-4)", "America/Caracas"),
                new("America/Havana", "La Habana (UTC-5)", "America/Havana"),
                new("America/Toronto", "Toronto (UTC-5)", "America/Toronto"),
                new("America/Vancouver", "Vancouver (UTC-8)", "America/Vancouver"),
                // Europa
                new("Europe/London", "Londres (UTC+0)", "Europe/London"),
                new("Europe/Paris", "París (UTC+1)", "Europe/Paris"),
                new("Europe/Madrid", "Madrid (UTC+1)", "Europe/Madrid"),
                new("Europe/Berlin", "Berlín (UTC+1)", "Europe/Berlin"),
                new("Europe/Rome", "Roma (UTC+1)", "Europe/Rome"),
                new("Europe/Athens", "Atenas (UTC+2)", "Europe/Athens"),
                new("Europe/Moscow", "Moscú (UTC+3)", "Europe/Moscow"),
                new("Europe/Istanbul", "Estambul (UTC+3)", "Europe/Istanbul"),
                // Asia
                new("Asia/Dubai", "Dubái (UTC+4)", "Asia/Dubai"),
                new("Asia/Karachi", "Karachi (UTC+5)", "Asia/Karachi"),
                new("Asia/Kolkata", "Nueva Delhi (UTC+5:30)", "Asia/Kolkata"),
                new("Asia/Dhaka", "Daca (UTC+6)", "Asia/Dhaka"),
                new("Asia/Bangkok", "Bangkok (UTC+7)", "Asia/Bangkok"),
                new("Asia/Shanghai", "Shanghái (UTC+8)", "Asia/Shanghai"),
                new("Asia/Tokyo", "Tokio (UTC+9)", "Asia/Tokyo"),
                new("Asia/Seoul", "Seúl (UTC+9)", "Asia/Seoul"),
                new("Asia/Singapore", "Singapur (UTC+8)", "Asia/Singapore"),
                // Oceanía
                new("Australia/Sydney", "Sídney (UTC+10)", "Australia/Sydney"),
                new("Pacific/Auckland", "Auckland (UTC+12)", "Pacific/Auckland"),
                // África
                new("Africa/Cairo", "El Cairo (UTC+2)", "Africa/Cairo"),
                new("Africa/Lagos", "Lagos (UTC+1)", "Africa/Lagos"),
                new("Africa/Johannesburg", "Johannesburgo (UTC+2)", "Africa/Johannesburg"),
            };
        }

        public record ZonaHoraria(string Id, string Nombre, string IanaId);
    }
}
