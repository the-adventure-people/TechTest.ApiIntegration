namespace ApiIntegration.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int providerId, string tourRef);
        Task<Tour> Get(Func<Tour, bool> predicate);
    }
}