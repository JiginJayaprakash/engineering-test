using Xunit;
using GildedRose.Console;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    // ========== NORMAL ITEMS TESTS ==========

    [Fact]
    public void NormalItem_DecreasesQualityAndSellInByOne()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(19, app.Items[0].Quality);
    }

    [Fact]
    public void NormalItem_QualityDegradesTwiceAsFastAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Elixir of the Mongoose", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(8, app.Items[0].Quality); // degraded by 2
    }

    [Fact]
    public void NormalItem_QualityNeverNegative()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 0 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality); // stays at 0, doesn't go negative
    }

    [Fact]
    public void NormalItem_QualityNeverNegativeAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Elixir of the Mongoose", SellIn = -1, Quality = 1 });

        app.UpdateQuality();

        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality); // degrades by 2, but stops at 0
    }

    // ========== AGED BRIE TESTS ==========

    [Fact]
    public void AgedBrie_IncreasesQualityAsItGetsOlder()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });

        app.UpdateQuality();

        Assert.Equal(1, app.Items[0].SellIn);
        Assert.Equal(1, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_IncreasesQualityAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(12, app.Items[0].Quality); // increases by 2 after sell date
    }

    [Fact]
    public void AgedBrie_QualityCapAt50()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // capped at 50
    }

    [Fact]
    public void AgedBrie_QualityCapAt50_WhenIncreasingAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = -1, Quality = 49 });

        app.UpdateQuality();

        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // increases by 2 but capped at 50
    }

    // ========== SULFURAS TESTS ==========

    [Fact]
    public void Sulfuras_NeverDecreases()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].SellIn); // SellIn never changes
        Assert.Equal(80, app.Items[0].Quality); // Quality never changes
    }

    [Fact]
    public void Sulfuras_NeverHasToBeSOld()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = -10, Quality = 80 });

        app.UpdateQuality();

        Assert.Equal(-10, app.Items[0].SellIn); // SellIn doesn't decrease
        Assert.Equal(80, app.Items[0].Quality);
    }

    [Fact]
    public void Sulfuras_MaintainsQualityOf80()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 5, Quality = 80 });

        app.UpdateQuality();
        app.UpdateQuality();
        app.UpdateQuality();

        Assert.Equal(5, app.Items[0].SellIn);
        Assert.Equal(80, app.Items[0].Quality);
    }

    // ========== BACKSTAGE PASSES TESTS ==========

    [Fact]
    public void BackstagePass_IncreasesByOneWhenMoreThan10DaysOut()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(14, app.Items[0].SellIn);
        Assert.Equal(21, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_IncreasesByTwoWhenLessThan11DaysAndMoreThan5Days()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(22, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_IncreasesByThreeWhenLessThan6Days()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(23, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_DropsToZeroAfterConcert()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_QualityCapAt50()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // increases by 2 but capped at 50
    }

    [Fact]
    public void BackstagePass_QualityCapAt50_OneDayOut()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 49 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // would increase by 3 but capped at 50
    }

    [Fact]
    public void BackstagePass_OneDayBeforeConcert()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(23, app.Items[0].Quality); // increases by 3
    }

    // ========== CONJURED ITEMS TESTS ==========

    [Fact]
    public void ConjuredItem_DegradesTwiceAsFastAsNormal()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });

        app.UpdateQuality();

        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality); // degrades by 2 instead of 1
    }

    [Fact]
    public void ConjuredItem_DegradesFourTimesAsFastAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(6, app.Items[0].Quality); // degrades by 4 (twice as fast as normal 2x degradation)
    }

    [Fact]
    public void ConjuredItem_QualityNeverNegative()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 1 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality); // degrades by 2, but stops at 0
    }

    [Fact]
    public void ConjuredItem_QualityNeverNegativeAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 2 });

        app.UpdateQuality();

        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality); // would degrade by 4, but stops at 0
    }

    [Fact]
    public void ConjuredItem_ZeroQualityStaysZero()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 0 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    // ========== MULTIPLE ITEMS TESTS ==========

    [Fact]
    public void MultipleItems_UpdatesAllItems()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 });
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });
        app.Items.Add(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(19, app.Items[0].Quality);

        Assert.Equal(1, app.Items[1].SellIn);
        Assert.Equal(1, app.Items[1].Quality);

        Assert.Equal(0, app.Items[2].SellIn);
        Assert.Equal(80, app.Items[2].Quality);
    }

    // ========== COMBINED/EDGE CASE TESTS ==========

    [Fact]
    public void NormalItem_MultipleUpdates_QualityNeverNegative()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "+5 Dexterity Vest", SellIn = 2, Quality = 2 });

        app.UpdateQuality(); // Quality: 1, SellIn: 1
        app.UpdateQuality(); // Quality: 0, SellIn: 0
        app.UpdateQuality(); // Quality: 0 (not -2), SellIn: -1

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_MultipleUpdates_IncreasingVelocity()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 12, Quality = 20 });

        app.UpdateQuality(); // More than 10 days: +1
        Assert.Equal(11, app.Items[0].SellIn);
        Assert.Equal(21, app.Items[0].Quality);

        app.UpdateQuality(); // Still more than 10 days (11 is not < 11): +1
        Assert.Equal(10, app.Items[0].SellIn);
        Assert.Equal(22, app.Items[0].Quality);

        app.UpdateQuality(); // Now 10 days or less: +2
        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(24, app.Items[0].Quality);

        app.UpdateQuality(); // Still 10 days or less (9 <= 10): +2
        Assert.Equal(8, app.Items[0].SellIn);
        Assert.Equal(26, app.Items[0].Quality);

        app.UpdateQuality(); // Still 10 days or less (8 <= 10): +2
        Assert.Equal(7, app.Items[0].SellIn);
        Assert.Equal(28, app.Items[0].Quality);

        app.UpdateQuality(); // Still 10 days or less (7 <= 10): +2
        Assert.Equal(6, app.Items[0].SellIn);
        Assert.Equal(30, app.Items[0].Quality);

        app.UpdateQuality(); // Still 10 days or less (6 <= 10): +2
        Assert.Equal(5, app.Items[0].SellIn);
        Assert.Equal(32, app.Items[0].Quality);

        app.UpdateQuality(); // Now 5 days or less: +3
        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(35, app.Items[0].Quality);

        app.UpdateQuality(); // Still 5 days or less (4 <= 5): +3
        Assert.Equal(3, app.Items[0].SellIn);
        Assert.Equal(38, app.Items[0].Quality);

        app.UpdateQuality(); // Still 5 days or less (3 <= 5): +3
        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(41, app.Items[0].Quality);

        app.UpdateQuality(); // Still 5 days or less (2 <= 5): +3
        Assert.Equal(1, app.Items[0].SellIn);
        Assert.Equal(44, app.Items[0].Quality);

        app.UpdateQuality(); // Still 5 days or less (1 <= 5): +3
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(47, app.Items[0].Quality);

        app.UpdateQuality(); // Concert has happened (SellIn became 0): quality drops to 0
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_MultipleUpdates_IncreasingQuality()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 3, Quality = 0 });

        app.UpdateQuality(); // Quality: 1, SellIn: 2
        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(1, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 2, SellIn: 1
        Assert.Equal(1, app.Items[0].SellIn);
        Assert.Equal(2, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 3, SellIn: 0
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(3, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 5 (+2 after sell date), SellIn: -1
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(5, app.Items[0].Quality);
    }

    [Fact]
    public void ConjuredItem_MultipleUpdates()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 10 });

        app.UpdateQuality(); // Quality: 8 (-2), SellIn: 2
        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(8, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 6 (-2), SellIn: 1
        Assert.Equal(1, app.Items[0].SellIn);
        Assert.Equal(6, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 4 (-2), SellIn: 0
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality);

        app.UpdateQuality(); // Quality: 0 (-4 after sell date), SellIn: -1
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    // ========== BOUNDARY TESTS FOR BACKSTAGE PASSES ==========

    [Fact]
    public void BackstagePass_BoundaryAt11DaysStillGetsPlus1()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 11, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(10, app.Items[0].SellIn);
        Assert.Equal(21, app.Items[0].Quality); // Only +1, not +2
    }

    [Fact]
    public void BackstagePass_BoundaryAt10DaysGetsPlus2()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(22, app.Items[0].Quality); // Exactly +2 at boundary
    }

    [Fact]
    public void BackstagePass_BoundaryAt6DaysStillGetsPlus2()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 6, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(5, app.Items[0].SellIn);
        Assert.Equal(22, app.Items[0].Quality); // +2, not +3 yet
    }

    [Fact]
    public void BackstagePass_BoundaryAt5DaysGetsPlus3()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(23, app.Items[0].Quality); // Exactly +3 at boundary
    }

    // ========== BACKSTAGE PASS QUALITY CAP EDGE CASES ==========

    [Fact]
    public void BackstagePass_QualityCapAt50_WithPlus2()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // Would be +2, but capped at 50
    }

    [Fact]
    public void BackstagePass_QualityCapAt50_WithPlus3()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 48 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // Would be +3, but capped at 50
    }

    [Fact]
    public void BackstagePass_QualityAt50_StaysAt50()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 8, Quality = 50 });

        app.UpdateQuality();

        Assert.Equal(7, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality); // Already at max, stays at 50
    }

    // ========== NORMAL ITEMS - ADDITIONAL AFTER SELL DATE TESTS ==========

    [Fact]
    public void NormalItem_DegradesTwiceAfterSellDate_MultipleUpdates()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Elixir of the Mongoose", SellIn = 0, Quality = 10 });

        app.UpdateQuality(); // After sell date: -2
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(8, app.Items[0].Quality);

        app.UpdateQuality(); // Still after sell date: -2
        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(6, app.Items[0].Quality);

        app.UpdateQuality(); // Still after sell date: -2
        Assert.Equal(-3, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality);
    }

    [Fact]
    public void NormalItem_LowQualityAfterSellDate_Stops()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Elixir of the Mongoose", SellIn = -5, Quality = 1 });

        app.UpdateQuality(); // -2, but stops at 0
        Assert.Equal(-6, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    // ========== AGED BRIE - ADDITIONAL QUALITY CAP TESTS ==========

    [Fact]
    public void AgedBrie_IncreaseFromLowQualityTowardsCap()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 10, Quality = 48 });

        app.UpdateQuality(); // +1
        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(49, app.Items[0].Quality);

        app.UpdateQuality(); // +1, reaches 50
        Assert.Equal(8, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality);

        app.UpdateQuality(); // Already at 50, stays at 50
        Assert.Equal(7, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_AfterSellDate_DoubleIncreaseStopsAtCap()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = -1, Quality = 48 });

        app.UpdateQuality(); // +2, reaches 50
        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(50, app.Items[0].Quality);
    }

    // ========== SELL IN CONSISTENCY TESTS ==========

    [Fact]
    public void SellInDecreaseConsistently()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 20 });
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 5, Quality = 20 });
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 });
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 20 });
        // Sulfuras not included as it shouldn't decrease

        app.UpdateQuality();

        // All non-Sulfuras items should decrease SellIn by 1
        Assert.Equal(4, app.Items[0].SellIn); // Normal item
        Assert.Equal(4, app.Items[1].SellIn); // Aged Brie
        Assert.Equal(4, app.Items[2].SellIn); // Backstage pass
        Assert.Equal(4, app.Items[3].SellIn); // Conjured
    }

    // ========== TRANSITION FROM BEFORE TO AFTER SELL DATE ==========

    [Fact]
    public void NormalItem_TransitionFromBeforeToAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Elixir of the Mongoose", SellIn = 1, Quality = 10 });

        app.UpdateQuality(); // SellIn is 1, not yet sold, -1
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(9, app.Items[0].Quality);

        app.UpdateQuality(); // SellIn is 0, now sold, -2
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(7, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_TransitionFromBeforeToAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Aged Brie", SellIn = 1, Quality = 45 });

        app.UpdateQuality(); // SellIn is 1, not yet sold, +1
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(46, app.Items[0].Quality);

        app.UpdateQuality(); // SellIn is 0, now sold, +2
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(48, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_TransitionFromBeforeToAfterConcert()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 20 });

        app.UpdateQuality(); // SellIn is 1, before concert, +3
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(23, app.Items[0].Quality);

        app.UpdateQuality(); // SellIn is 0, concert has happened, quality drops to 0
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    // ========== CONJURED ITEMS - ADDITIONAL EDGE CASES ==========

    [Fact]
    public void ConjuredItem_QualityNeverExceedsZeroAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 3 });

        app.UpdateQuality(); // Would degrade by 4, but stops at 0
        Assert.Equal(-2, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void ConjuredItem_TransitionFromBeforeToAfterSellDate()
    {
        var app = new Program();
        app.Items.Add(new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 10 });

        app.UpdateQuality(); // SellIn is 1, not yet sold, -2
        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(8, app.Items[0].Quality);

        app.UpdateQuality(); // SellIn is 0, now sold, -4
        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality);
    }
}