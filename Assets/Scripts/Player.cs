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

    [Header("Timer")]
    [SerializeField] private float baseTimePerLevel;
    [Tooltip("Recommended to have a 0-1 value in both x and y axis. It is multiplied by baseTimePerLevel.")]
    [SerializeField] private AnimationCurve timeMultiplierOverCompletion;

    [field: Header("Endless Timer")]
    [field: SerializeField] public float EndlessStartTimer { get; private set; }

    [SerializeField] private float endlessTimeIncreasePerLevel;

    [Header("Coin Anim")]
    [SerializeField] private Transform coinIcon;
    [SerializeField] private Ease ease;
    [SerializeField] private float spinDuration;

    [Header("Audio")]
    [SerializeField] private AudioSource clockTick;
    [SerializeField] private AnimationCurve tickVolumeOverTimeLeft;

    private int coins;

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
        CurrentLevelTimer = PuzzleCycler.Instance.EndlessMode ? EndlessStartTimer : baseTimePerLevel;
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
        if (coinIcon != null)
        {
            coinIcon.DOLocalRotate(Vector3.forward * 360, spinDuration).SetRelative().SetEase(ease);
            coinIcon.DOScale(new Vector3(1.3f, 1.3f, 1.0f), spinDuration / 2.0f).SetLoops(2, LoopType.Yoyo)
                .SetEase(ease);
        }

        PuzzleRunning = false;
    }

    private void OnLevelReload()
    {
        PuzzleRunning = true;
        ReEvaluateTimer();
    }

    private void ReEvaluateTimer()
    {
        if (PuzzleCycler.Instance.EndlessMode) CurrentLevelTimer += endlessTimeIncreasePerLevel;
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
