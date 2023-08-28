using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CityDataStore
    {
        public List<CityDTO> Cities { get; set; }
        public static CityDataStore Instance { get; } = new CityDataStore();

        public CityDataStore()
        {
            Cities = new List<CityDTO>()
            {
                new CityDTO()
                {
                    Id = 1,
                    Name = "Ha Noi",
                    Description = "The captial of Vietnam."
                },
                new CityDTO()
                {
                    Id = 2,
                    Name = "New York City",
                    Description = "The one with that big park."
                },
                new CityDTO()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The city of love."
                },
                new CityDTO()
                {
                    Id = 4,
                    Name = "Tokyo",
                    Description = "The bustling metropolis."
                },
            };
        }
    }
}
