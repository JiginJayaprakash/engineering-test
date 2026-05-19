namespace GildedRose.Console.Updater
{
    public class AgedBrieUpdater : IItemUpdater
    {
        public void Update(Item item)
        {
            item.SellIn--;
            int appreciation = item.SellIn < 0 ? 2 : 1;
            item.Quality = Quality.Clamp(item.Quality + appreciation);
        }
    }
}
