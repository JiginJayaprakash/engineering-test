using GildedRose.Console.Updater;

namespace GildedRose.Console
{
    public class UpdaterFactory
    {
        private static readonly Dictionary<string, IItemUpdater> _exact = new()
        {
            ["Sulfuras, Hand of Ragnaros"] = new SulfurasUpdater(),
            ["Aged Brie"] = new AgedBrieUpdater(),
            ["Backstage passes to a TAFKAL80ETC concert"] = new BackstagePassUpdater(),
        };

        private static readonly IItemUpdater _normal = new NormalItemUpdater();
        private static readonly IItemUpdater _conjured = new ConjuredItemUpdater();

        public IItemUpdater GetUpdater(Item item) =>
            _exact.TryGetValue(item.Name, out var u) ? u
            : item.Name.Contains("Conjured") ? _conjured
            : _normal;
    }
}
