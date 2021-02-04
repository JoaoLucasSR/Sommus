using Sommus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sommus.Api.Repository
{
    public interface IDeathsRepository
    {
        Task<int> Add(Deaths Deaths);
        Task<int> AddAll(List<Deaths> Deaths);
        Task<Deaths> Get(string date);
        Task<Deaths> GetLast();
    }
}
