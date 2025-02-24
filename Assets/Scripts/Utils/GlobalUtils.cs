public static class GlobalUtils
{
    public static string goldPrefKey = "Gold";
    public static string ShortenNumber(long num)
    {
        if (num < 1000) return num.ToString();
        if (num < 1000000) return (num / 1000.0).ToString() + "K";
        if (num < 1000000000) return (num / 1_000_000.0).ToString() + "M";
        if (num < 1000000000000) return (num / 1_000_000_000.0).ToString() + "B";
        return (num / 10000000000000).ToString() + "T";
    }
}
