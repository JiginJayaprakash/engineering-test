namespace GildedRose.Console.Updater
{
    public class BackstagePassUpdater : IItemUpdater
    {
        public void Update(Item item)
        {
            item.SellIn--;

            item.Quality = item.SellIn < 0 ? 0
                : item.SellIn < 5 ? Quality.Clamp(item.Quality + 3)
                : item.SellIn < 10 ? Quality.Clamp(item.Quality + 2)
                : Quality.Clamp(item.Quality + 1);
        }
    }
}
