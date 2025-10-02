using Reloj_Marcador.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloj_Marcador.Services
{
    public class MarcasService : IMarcasService
    {
        private readonly Repository.MarcasRepository _marcasRepository;

        public MarcasService(Repository.MarcasRepository marcasRepository)
        {
            _marcasRepository = marcasRepository;
        }

        public async Task<IEnumerable<string>> GetAllAreaByID(string id)
        {
            return await _marcasRepository.GetAllAreaByID(id);
        }
    }
}
