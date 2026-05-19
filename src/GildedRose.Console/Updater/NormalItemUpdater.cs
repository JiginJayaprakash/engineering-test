namespace GildedRose.Console.Updater
{
    public class NormalItemUpdater : IItemUpdater
    {
        private readonly int _rate;
        public NormalItemUpdater(int rate = 1) => _rate = rate;

        public void Update(Item item)
        {
            item.SellIn--;
            int degradation = item.SellIn < 0 ? _rate * 2 : _rate;
            item.Quality = Quality.Clamp(item.Quality - degradation);
        }
    }
}
