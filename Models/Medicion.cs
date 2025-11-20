using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Parcial2DDA.Models
{
    public class Medicion
    {
        public int Id { get; set; }
        public string Huella { get; set; }
        public double Peso { get; set; }
        public string Tipo { get; set; }
        public DateTime TiempoEntrada { get; set; }
        public DateTime TiempoSalida { get; set; }


    }
}
