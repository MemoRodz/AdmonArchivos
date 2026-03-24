using ArchivosMan.BLL.Interfaces;

namespace ArchivosMan.BLL.Implementacion
{
    public class UtilidadesService : IUtilidadesService
    {
        /// <summary>
        /// Convertir Cantidad Numérica Pesos en Cantidad en Letra Moneda.
        /// </summary>
        /// <param name="Numero">Cantidad Numérica agregando "M.N" (Moneda Nacional)</param>
        /// <param name="CentimosEnLetra">True convierte Centavos, False escribe números.</param>
        /// <returns>Cantidad en Letra.</returns>
        public string ConvertirNumeroALetras(double Numero, bool CentimosEnLetra = false)
        {
            //************************************************************
            // Parámetros
            //************************************************************
            string Moneda = "Peso";             // Nombre de Moneda (Singular)
            string Monedas = "Pesos";           // Nombre de Moneda (Plural)
            string Centimo = "Centavo";         // Nombre de Céntimos (Singular)
            string Centimos = "Centavos";       // Nombre de Céntimos (Plural)
            string Preposicion = "Con";         // Preposición entre Moneda y Céntimos
            //************************************************************
            // VariablesCODIGO en VB.Net compartido en http://exceltotal.com/
            double NumCentimos;
            string Letra;
            const double Maximo = 1999999999.99;

            try
            {
                // Validar que el Numero está dentro de los límites
                if (Numero >= 0 && Numero <= Maximo)
                {
                    Letra = NumeroRecursivo((long)Math.Floor(Numero));              // Convertir el Numero en letras

                    // Si Numero = 1 agregar leyenda Moneda (Singular)
                    if (Numero == 1)
                        Letra += " " + Moneda;
                    // De lo contrario agregar leyenda Monedas (Plural)
                    else
                        Letra += " " + Monedas;

                    NumCentimos = Math.Round((Numero - Math.Floor(Numero)) * 100);              // Obtener los centimos del Numero

                    // Si NumCentimos es mayor a cero inicar la conversión
                    if (NumCentimos >= 0)
                    {
                        // Si el parámetro CentimosEnLetra es VERDADERO obtener letras para los céntimos
                        if (CentimosEnLetra)
                        {
                            // Convertir centavos a letra
                            Letra += " " + Preposicion + " " + NumeroRecursivo((long)NumCentimos);      // Convertir los céntimos en letra
                            // Si NumCentimos = 1 agregar leyenda Centimos (Singular)
                            if (NumCentimos == 1)
                                Letra += " " + Centimo + " M. N. ";
                            // De lo contrario agregar leyenda Centimos (Plural)
                            else
                                Letra += " " + Centimos + " M. N. ";
                            // De lo contrario mostrar los céntimos como número
                        }
                        else
                        {
                            if (NumCentimos < 10)
                                Letra += " 0" + NumCentimos + "/100 M. N.";
                            else
                                Letra += " " + NumCentimos + "/100 M. N.";
                        }
                    }

                    // Regresar el resultado final de la conversión
                    return Letra;
                }
                else
                {
                    // Si el Numero no está dentro de los límites, entivar un mensaje de error
                    return "ERROR: El número excede los límites.";
                }
            }
            catch (Exception ex)
            {
                // Si el Número tiene un valor no válido
                return "Error en conversión.\n" + ex.Message;
            }
        }
        /// <summary>
        /// Conversión de Número en su cadena correspondiente.
        /// </summary>
        /// <param name="Numero">Número a convertir.</param>
        /// <returns>Cadena de número convertidoa texto.</returns>
        public string NumeroRecursivo(long Numero)
        {
            //************************************************************
            // VariablesCODIGO en VB.Net compartido en http://exceltotal.com/
            string Resultado = string.Empty;

            //**************************************************
            // Nombre de los números
            //**************************************************
            string[] Unidades = { "", "Un", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve", "Diez", "Once", "Doce", "Trece", "Catorece", "Quince", "Dieciséis", "Diecisiete", "Dieciocho", "Diecinueve", "Veinte", "Veintiuno", "Veintidos", "Veintitres", "Veinticuatro", "Veinticinco", "Veintiseis", "Veintisiete", "Veintiocho", "Veintinueve" };
            string[] Decenas = { "", "Diez", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa", "Cien" };
            string[] Centenas = { "", "Ciento", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos" };
            //**************************************************

            if (Numero == 0)
                Resultado = "Cero";
            else if (Numero >= 1 && Numero <= 29)
                Resultado = Unidades[Numero];
            else if (Numero >= 30 && Numero <= 100)
                Resultado = Decenas[Numero / 10] + (Numero % 10 != 0 ? " y " + NumeroRecursivo(Numero % 10) : "");
            else if (Numero >= 101 && Numero <= 999)
                Resultado = Centenas[Numero / 100] + (Numero % 100 != 0 ? " " + NumeroRecursivo(Numero % 100) : "");
            else if (Numero >= 1000 && Numero <= 1999)
                Resultado = "Mil" + (Numero % 1000 != 0 ? " " + NumeroRecursivo(Numero % 1000) : "");
            else if (Numero >= 2000 && Numero <= 999999)
                Resultado = NumeroRecursivo(Numero / 1000) + " Mil" + (Numero % 1000 != 0 ? " " + NumeroRecursivo(Numero % 1000) : "");
            else if (Numero >= 1000000 && Numero <= 1999999)
                Resultado = "Un Millón" + (Numero % 1000000 != 0 ? " " + NumeroRecursivo(Numero % 1000000) : "");
            else if (Numero >= 2000000 && Numero <= 1999999999)
                Resultado = NumeroRecursivo(Numero / 1000000) + " Millones" + (Numero % 1000000 != 0 ? " " + NumeroRecursivo(Numero % 1000000) : "");

            return Resultado;
        }
        /// <summary>
        /// Conversión de Fahrenheit a Celsius.
        /// </summary>
        /// <param name="fahrenheit"></param>
        /// <returns>Grados Celsius.</returns>
        public decimal celsiusF(decimal fahrenheit)
        {
            return (fahrenheit - 32m) * (5m / 9m);
        }
        /// <summary>
        /// Conversión de Celsius a Fahrenheit.
        /// </summary>
        /// <param name="celsius"></param>
        /// <returns>Grados Fahrenheit.</returns>
        public decimal fahrenheit(decimal celsius)
        {
            return celsius * 9 / 5 + 32;
        }
    }
}
