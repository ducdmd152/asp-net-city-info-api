using CityInfo.API.Entities;
namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCityAsync(
            int city,
            int pointOfInterestId);
        Task<bool> CityExistsAsync(int cityId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();
    }
}
