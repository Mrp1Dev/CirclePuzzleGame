using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseApp app;

    private const string EndlessModeClicked = "endless_mode_clicked";
    private const string NormalModeClicked = "normal_mode_clicked";

    private const string GlobalParamPackName = "pack_name";

    private const string PackOpened = "pack_opened";

    private const string PackLost = "pack_lost";
    private const string PackLostParamAtLevel = "at_level";

    private const string PackWon = "pack_won";
    private const string PackWonParamWithScore = "with_score";

    private const string PuzzleSolved = "puzzle_solved";
    private const string PuzzleSolvedParamWithTimeLeft = "with_time_left";

    private const string PackHighScoreMade = "pack_highscore_made";
    private const string PackHighScoreMadeParamNewHighScore = "new_high_score";

    private const string PackBought = "pack_bought";
    private const string PackBoughtParamMode = "mode";

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void OnEndlessModeClicked()
    {
        if (app == null) Debug.LogWarning($"{nameof(OnEndlessModeClicked)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(EndlessModeClicked);
    }

    public void OnNormalModeClicked()
    {
        if (app == null) Debug.LogWarning($"{nameof(OnNormalModeClicked)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(NormalModeClicked);
    }

    public void OnPackOpened(PuzzlePack pack)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackOpened)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PackOpened, new Parameter(GlobalParamPackName, pack.name));
    }

    public void OnPackLost(PuzzlePack pack, int level)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackLost)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PackLost, new Parameter(GlobalParamPackName, pack.name), new Parameter(PackLostParamAtLevel, level));
    }

    public void OnPackWon(PuzzlePack pack, int score)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackWon)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PackWon, new Parameter(GlobalParamPackName, pack.name), new Parameter(PackWonParamWithScore, score));
    }

    public void OnPuzzleSolved(PuzzlePack pack, float timeLeft)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackWon)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PuzzleSolved, new Parameter(GlobalParamPackName, pack.name), new Parameter(PuzzleSolvedParamWithTimeLeft, timeLeft));
    }

    public void OnPackHighScoreMade(PuzzlePack pack, int newHighScore)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackHighScoreMade)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PackHighScoreMade, new Parameter(GlobalParamPackName, pack.name), new Parameter(PackHighScoreMadeParamNewHighScore, newHighScore));
    }

    public void OnPackBought(PuzzlePack pack, bool scoreMode)
    {
        if (app == null) Debug.LogWarning($"{nameof(OnPackBought)} was called but app wasn't loaded.");
        else FirebaseAnalytics.LogEvent(PackBought, new Parameter(GlobalParamPackName, pack.name), new Parameter(PackBoughtParamMode, scoreMode ? "score" : "coins"));
    }
}
