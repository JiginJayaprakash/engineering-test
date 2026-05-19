namespace GildedRose.Console
{
    public static class Quality
    {
        public const int Min = 0;
        public const int Max = 50;
        public static int Clamp(int v) => Math.Clamp(v, Min, Max);
    }
}
