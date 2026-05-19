# Gilded Rose — Refactored

---

## How it works

Each item type has its own dedicated **updater class** that knows exactly how to age that item. A central **factory** picks the right updater at runtime, and the **orchestrator** (`GildedRose`) just loops and delegates — no `switch`, no string comparisons, no business logic buried in a god class.

```
GildedRose.UpdateQuality()
    └─► UpdaterFactory.GetUpdater(item)
            └─► IItemUpdater.Update(item)   ← one of five implementations
```

---

## Item rules

| Item | Behaviour |
|---|---|
| **Normal item** | Quality drops by 1/day; drops by 2 after `SellIn` passes 0 |
| **Aged Brie** | Quality *rises* by 1/day; rises by 2 after sell-by |
| **Backstage pass** | +1 quality normally; +2 with ≤10 days left; +3 with ≤5 days left; drops to **0** after the concert |
| **Conjured item** | Degrades at double the normal rate (2/day, 4 after sell-by) |
| **Sulfuras** | Legendary — Quality and SellIn never change |

Quality is always clamped between **0** and **50** (Sulfuras stays at 80, outside the normal range).

---

## Project structure

```
GildedRose/
├── Item.cs                      # Data model (Name, SellIn, Quality)
├── Quality.cs                   # Min/Max constants + Clamp helper
├── UpdaterFactory.cs            # Maps item names → correct IItemUpdater
├── Updaters/
│   ├── IItemUpdater.cs          # interface Update(Item item)
│   ├── NormalItemUpdater.cs     # Base degradation logic (configurable rate)
│   ├── AgedBrieUpdater.cs
│   ├── BackstagePassUpdater.cs
│   ├── ConjuredItemUpdater.cs   # Extends NormalItemUpdater with rate: 2
│   └── SulfurasUpdater.cs       # No-op
└── Program.cs                   # Entry point / demo
```

---

## Adding a new item type

1. Create `Updaters/YourItemUpdater.cs` implementing `IItemUpdater`.
2. Register it in `UpdaterFactory._exact` (or add a name-contains check).
3. Done. No existing class needs to change.

```csharp
// 1. New class
public class PotionUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        item.SellIn--;
        item.Quality = Quality.Clamp(item.Quality - 3);
    }
}

// 2. Register
["Potion of Wisdom"] = new PotionUpdater(),
```

---

## Design principles applied

- **Single Responsibility** — each updater owns exactly one item's rules.
- **Open/Closed** — open for extension (new updater class), closed for modification (no existing code touched).
