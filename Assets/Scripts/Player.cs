using System;
using DG.Tweening;
using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Score")]
    [SerializeField] private float startScore;
    [SerializeField] private float scoreReductionPerSecond;
    [SerializeField] private float scoreIncreasePerPuzzle;

    [Header("Coins")]
    [SerializeField] private int coinIncreasePerPuzzleSolved;
    private int coins;

    [Header("Timer")]
    [SerializeField] private float baseTimePerLevel;
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

        Score -= Mathf.Max(scoreReductionPerSecond * Time.deltaTime, 0);
        clockTick.volume = tickVolumeOverTimeLeft.Evaluate(1 - CurrentLevelTimer / LevelTimerMax);
    }

    private void OnDisable() => PuzzleRunning = false;
    public event Action GameLost;

    private void OnWin()
    {
        Score += scoreIncreasePerPuzzle;
        Coins += coinIncreasePerPuzzleSolved;
        rotator.RotateCoin();
        PuzzleRunning = false;
        if (PuzzleCycler.Instance.EndlessMode == false) FirebaseManager.Instance.OnPuzzleSolved(PuzzleCycler.Instance.SelectedPack, CurrentLevelTimer);
        winSound.Play();
    }

    private void OnLevelReload()
    {
        PuzzleRunning = true;
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

    public void StartEndlessTimer() => CurrentLevelTimer = EndlessStartTimer;
}
