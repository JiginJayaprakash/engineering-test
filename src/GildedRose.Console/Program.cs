using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program()
        {
            Items = new List<Item>
                                      {
                                          new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                          new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                          new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                          new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                          new Item
                                              {
                                                  Name = "Backstage passes to a TAFKAL80ETC concert",
                                                  SellIn = 15,
                                                  Quality = 20
                                              },
                                          new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                      }

        };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var itemType = GetItemType(Items[i].Name);

            switch (itemType)
            {
                case ItemType.Sulfuras:
                    // Sulfuras never changes
                    break;

                case ItemType.AgedBrie:
                    UpdateAgedBrie(Items[i]);
                    break;

                case ItemType.BackstagePass:
                    UpdateBackstagePass(Items[i]);
                    break;

                case ItemType.Conjured:
                    UpdateConjuredItem(Items[i]);
                    break;

                case ItemType.Normal:
                default:
                    UpdateNormalItem(Items[i]);
                    break;
            }
        }
    }

    private enum ItemType
    {
        Normal,
        AgedBrie,
        Sulfuras,
        BackstagePass,
        Conjured
    }

    private ItemType GetItemType(string name)
    {
        return name switch
        {
            "Sulfuras, Hand of Ragnaros" => ItemType.Sulfuras,
            "Aged Brie" => ItemType.AgedBrie,
            "Backstage passes to a TAFKAL80ETC concert" => ItemType.BackstagePass,
            _ => name.Contains("Conjured") ? ItemType.Conjured : ItemType.Normal
        };
    }

    private void UpdateNormalItem(Item item)
    {
        int degradationAmount = 1;

        if (item.Quality > 0)
        {
            item.Quality = Math.Max(0, item.Quality - degradationAmount);
        }

        item.SellIn = item.SellIn - 1;

        if (item.SellIn < 0 && item.Quality > 0)
        {
            item.Quality = Math.Max(0, item.Quality - degradationAmount);
        }
    }

    private void UpdateAgedBrie(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality = item.Quality + 1;
        }

        item.SellIn = item.SellIn - 1;

        if (item.SellIn < 0 && item.Quality < 50)
        {
            item.Quality = item.Quality + 1;
        }
    }

    private void UpdateBackstagePass(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality = item.Quality + 1;

            if (item.SellIn < 11 && item.Quality < 50)
            {
                item.Quality = item.Quality + 1;
            }

            if (item.SellIn < 6 && item.Quality < 50)
            {
                item.Quality = item.Quality + 1;
            }
        }

        item.SellIn = item.SellIn - 1;

        if (item.SellIn < 0)
        {
            item.Quality = 0;
        }
    }

    private void UpdateConjuredItem(Item item)
    {
        int degradationAmount = 2;

        if (item.Quality > 0)
        {
            item.Quality = Math.Max(0, item.Quality - degradationAmount);
        }

        item.SellIn = item.SellIn - 1;

        if (item.SellIn < 0 && item.Quality > 0)
        {
            item.Quality = Math.Max(0, item.Quality - degradationAmount);
        }
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
