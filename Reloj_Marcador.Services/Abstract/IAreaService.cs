using Reloj_Marcador.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services.Abstract
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(string id);
        Task<int> InsertAsync(Area area);
        Task<int> UpdateAsync(Area area);
        Task<int> DeleteAsync(string id);
    }
}