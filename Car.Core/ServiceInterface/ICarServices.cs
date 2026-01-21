using Car.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car.Core.ServiceInterface
{
    public interface ICarServices
    {
        Task<IEnumerable<Domain.Car>> GetAll();
        Task<Domain.Car> Get(Guid id);
        Task<Domain.Car> Create(Domain.Car car);
        Task<Domain.Car> Update(Domain.Car car);
        Task<Domain.Car> Delete(Guid id);
    }
}