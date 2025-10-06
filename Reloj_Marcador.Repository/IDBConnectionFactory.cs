using System.Data;

namespace Reloj_Marcador.Repository
{
    
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
