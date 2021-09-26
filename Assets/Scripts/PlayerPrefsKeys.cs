public static class PlayerPrefsKeys
{
    public static string CoinsKey => "Coins";
    public static string EndlessHighScoreKey => "HighScoreEndless";
    public static string TutorialCompleted => "TutorialCompleted";
    public static string DifficultySettingKey => "DifficultySetting";
    public static string GetPackHighScoreKey(PuzzlePack pack) => pack.name + "HighScore";
    public static string GetPackBuyStateKey(PuzzlePack pack) => pack.name + "BuyState";
}