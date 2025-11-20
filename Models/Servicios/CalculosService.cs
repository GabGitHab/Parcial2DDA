using Parcial2DDA.Data;
namespace Parcial2DDA.Models.Servicios
{
    public class CalculosService
    {
        private readonly AppDbContext _context;
        public CalculosService(AppDbContext context)
        {
            _context = context;
        }

        public int CantidadTiempoEnLocal(Medicion medicion)
        {
            long tiempoEntrada = new DateTimeOffset(medicion.TiempoEntrada).ToUnixTimeSeconds();
            long tiempoSalida = new DateTimeOffset(medicion.TiempoSalida).ToUnixTimeSeconds();
            int cantidadDeTiempoEnLocal = (int)(tiempoSalida - tiempoEntrada);

            return cantidadDeTiempoEnLocal;
        }

        public double DifereciaDePesoEntradaYSalida(Medicion medicion)
        {
            double pesoEntrada = medicion.Peso;
            double pesoSalida = _context.Mediciones
                .Where(mmedicion => medicion.Huella == medicion.Huella && medicion.TiempoSalida > medicion.TiempoEntrada)
                .OrderBy(medicion => medicion.TiempoSalida)
                .Select(medicion => medicion.Peso)
                .FirstOrDefault();
            double diferenciaDePeso = pesoEntrada - pesoSalida;
            return diferenciaDePeso;
        }
    }
}
