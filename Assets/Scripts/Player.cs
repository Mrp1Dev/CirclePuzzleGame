using System;
using UnityEngine;
using MUtility;
using System.Linq;
public class Player : Singleton<Player>
{

    public event Action GameLost;

    [Header("Score")]
    [SerializeField] private float startScore;
    [SerializeField] private float scoreReductionPerSecond;
    [SerializeField] private float scoreIncreasePerPuzzle;

    [field: Header("Coins")]
    [field: SerializeField] public int CoinIncreasePerPuzzleSolved { get; set; }
    private int coins;

    [Header("Timer")]
    [SerializeField] private float baseTimePerLevel;
    [SerializeField] private float lowScoreTime;
    [SerializeField] private float highScoreTime;
    [SerializeField] private float lowScoreCriteria;
    [SerializeField] private float highScoreCriteria;
    [Tooltip("Recommended to have a 0-1 value in both x and y axis. It is multiplied by baseTimePerLevel.")]
    [SerializeField] private AnimationCurve timeMultiplierOverCompletion;

    [Header("Coin Anim")]
    [SerializeField] private CoinRotator rotator;

    [Header("Audio")]
    [SerializeField] private AudioSource clockTick;
    [SerializeField] private AnimationCurve tickVolumeOverTimeLeft;
    [SerializeField] private AudioSource winSound;

    [field: Header("Endless Timer")]
    [field: SerializeField] public float EndlessStartTimer { get; private set; }
    [SerializeField] private float endlessTimeIncreasePerLevel;
    public bool PuzzleRunning { get; set; } = true;
    public float Score { get; private set; }

    public int Coins
    {
        get => coins;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.CoinsKey, value);
            coins = value;
        }
    }

    public float CurrentLevelTimer { get; private set; }
    public float LevelTimerMax => timeMultiplierOverCompletion.Evaluate(LevelCompletionPercentage) * baseTimePerLevel;

    private float LevelCompletionPercentage =>
        (float) PuzzleCycler.Instance.CurrentlySolvedPuzzles / PuzzleCycler.Instance.PuzzleCount;
    private void Start()
    {
        Score = startScore;
        Coins = PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0);
        if (TutorialManager.TutorialFinished)
            CurrentLevelTimer = PuzzleCycler.Instance.EndlessMode ? EndlessStartTimer : baseTimePerLevel;
        else CurrentLevelTimer = Mathf.Infinity;
        WinConditionChecker.GameWon += OnWin;
        PuzzleCycler.LevelReload += OnLevelReload;
    }

    private void Update()
    {
        if (PuzzleRunning == false)
        {
            clockTick.Stop();
            return;
        }

        if (clockTick.isPlaying == false) clockTick.Play();

        CurrentLevelTimer -= Time.deltaTime;

        if (CurrentLevelTimer <= 0)
        {
            FirebaseManager.Instance.OnPackLost(PuzzleCycler.Instance.SelectedPack, PuzzleCycler.Instance.CurrentlySolvedPuzzles);
            GameLost?.Invoke();
            PuzzleRunning = false;
        }

        Score -= scoreReductionPerSecond * Time.deltaTime;
        Score = Mathf.Max(Score, 0);
        clockTick.volume = tickVolumeOverTimeLeft.Evaluate(1 - CurrentLevelTimer / LevelTimerMax);
    }

    private void OnDisable() => PuzzleRunning = false;

    private void OnWin()
    {
        Score += scoreIncreasePerPuzzle;
        Coins += CoinIncreasePerPuzzleSolved;
        rotator.RotateCoin();
        PuzzleRunning = false;
        if (PuzzleCycler.Instance.EndlessMode == false) FirebaseManager.Instance.OnPuzzleSolved(PuzzleCycler.Instance.SelectedPack, CurrentLevelTimer);
        winSound.Play();
    }

    private void OnLevelReload()
    {
        PuzzleRunning = true;
        RecalculateBaseTimer();
        ReEvaluateTimer();
    }

    private void ReEvaluateTimer()
    {
        if (TutorialManager.TutorialFinished == false) CurrentLevelTimer = Mathf.Infinity;
        else if (PuzzleCycler.Instance.EndlessMode)
        {
            CurrentLevelTimer += endlessTimeIncreasePerLevel;
            CurrentLevelTimer = Mathf.Clamp(CurrentLevelTimer, 0, EndlessStartTimer);
        }
        else CurrentLevelTimer = LevelTimerMax;
    }

    public void ResetValues(float extraTime = 0.0f)
    {
        Score = startScore;
        PuzzleRunning = true;
        ReEvaluateTimer();
        CurrentLevelTimer += extraTime;
    }

    private void RecalculateBaseTimer()
    {
        var packsTried = PackSupplier.Instance.Packs.Where(p => p.CurrentHighScore > 0);
        var averageHighScore = 0f;
        if (packsTried != null && packsTried.Any())
            averageHighScore = (float) packsTried.Average(p => p.CurrentHighScore);

        baseTimePerLevel = averageHighScore.ReMap(lowScoreCriteria, highScoreCriteria, lowScoreTime, highScoreTime);
        print($"Current calculated base time per level: {baseTimePerLevel}");
    }

    public void StartEndlessTimer() => CurrentLevelTimer = EndlessStartTimer;
}
