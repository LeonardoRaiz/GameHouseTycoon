using UnityEngine.UI;

namespace Models
{
    public class GameStore
    {
        public string Name { get; private set; }
        public int StoreCount { get; private set; }
        public float RevenuePerStore { get; private set; }
        public string Image { get; set; }
        public float PriceHour { get; set; }
        public float TotalGames { get; set; }

        public GameStore(string name, int initialCount, float revenuePerStore, string image, float priceHour)
        {
            Name = name;
            StoreCount = initialCount;
            RevenuePerStore = revenuePerStore;
            Image = image;
            PriceHour = priceHour;
        }

        public void SetStoreCount(int count)
        {
            StoreCount = count;
        }

        public float CalculateTotalRevenue()
        {
            return TotalGames + RevenuePerStore;
        }
    }
}