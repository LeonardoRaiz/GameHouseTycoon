namespace Models
{
    public class GameEconomy
    {
        public float CurrentMoney { get; set; }

        public GameEconomy(float startingMoney)
        {
            CurrentMoney = startingMoney;
        }
    }
}