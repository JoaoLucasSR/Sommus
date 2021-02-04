using Sommus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sommus.Api.Repository
{
    public interface IConfirmedRepository
    {
        Task<int> Add(Confirmed confirmed);
        Task<int> AddAll(List<Confirmed> Deaths);
        Task<Confirmed> Get(string date);
        Task<Confirmed> GetLast();
    }
}
