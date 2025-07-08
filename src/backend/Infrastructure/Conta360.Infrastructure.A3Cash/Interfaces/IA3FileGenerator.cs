using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.A3Cash.Interfaces
{
    public interface IA3FileGenerator
    {
        /// <summary>
        /// Procesa los datos diarios y genera un fichero de asientos contables compatible con A3.
        /// </summary>
        /// <param name="dailyData">Diccionario donde la clave es la fecha y el valor es el importe para ese día.</param>
        /// <param name="outputFilename">Nombre del fichero de salida .DAT (ej. "ASIENTOS_DIARIOS.DAT").</param>
        /// <returns>La ruta completa al fichero generado.</returns>
        /// <exception cref="ArgumentException">Si los datos diarios son inválidos o faltan parámetros de configuración.</exception>
        /// <exception cref="System.IO.IOException">Si ocurre un error durante la escritura del fichero.</exception>
        Task<string> ProcessAndGenerateAsync(Dictionary<DateOnly, decimal> dailyData, string outputFilename = "SUENLACE_FGLD.DAT");
    }
}