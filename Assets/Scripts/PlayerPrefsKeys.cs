public static class PlayerPrefsKeys
{
    public static string GetPackHighScoreKey(PuzzlePack pack) => pack.name + "HighScore";
    public static string GetPackBuyStateKey(PuzzlePack pack) => pack.name + "BuyState";
    public static string CoinsKey => "Coins";
}
