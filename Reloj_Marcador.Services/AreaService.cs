using Reloj_Marcador.Entities;
using Reloj_Marcador.Repository;
using Reloj_Marcador.Services.Abstract;

namespace Reloj_Marcador.Services
{
    public class AreaService : IAreaService
    {
        private readonly AreaRepository _areaRepository;

        public AreaService(AreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _areaRepository.GetAllAsync();
        }

        public async Task<Area?> GetByIdAsync(string id)
        {
            return await _areaRepository.GetByIdAsync(id);
        }

        public async Task<int> InsertAsync(Area area)
        {
            return await _areaRepository.InsertAsync(area);
        }

        public async Task<int> UpdateAsync(Area area)
        {
            return await _areaRepository.UpdateAsync(area);
        }

        public async Task<int> DeleteAsync(string id)
        {
            return await _areaRepository.DeleteAsync(id);
        }
    }
}
