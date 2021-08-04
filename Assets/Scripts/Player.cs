using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("SCORE")] [SerializeField] private float startScore;

    [SerializeField] private float scoreReductionPerSecond;
    [SerializeField] private float scoreIncreasePerPuzzle;
    [SerializeField] private int coinIncreasePerPuzzleSolved;

    [SerializeField] private float baseTimePerLevel;

    [Tooltip("Recommended to have a 0-1 value in both x and y axis. It is multiplied by baseTimePerLevel.")]
    [SerializeField]
    private AnimationCurve timeMultiplierOverCompletion;

    private int coins;

    public bool PuzzleRunning { get; private set; } = true;
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

    private float LevelCompletionPercentage =>
        (float) PuzzleCycler.Instance.CurrentlySolvedPuzzles / PuzzleCycler.Instance.PuzzleCount;

    public float CurrentLevelTimer { get; private set; }

    private void Start()
    {
        Score = startScore;
        Coins = PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0);
        CurrentLevelTimer = baseTimePerLevel;
        WinConditionChecker.GameWon += OnWin;
        PuzzleCycler.LevelReload += OnLevelReload;
    }


    private void Update()
    {
        if (!PuzzleRunning) return;
        CurrentLevelTimer -= Time.deltaTime;

        if (CurrentLevelTimer <= 0)
        {
            GameLost?.Invoke();
            PuzzleRunning = false;
        }

        Score -= Mathf.Max(scoreReductionPerSecond * Time.deltaTime, 0);
    }

    private void OnDisable() => PuzzleRunning = false;
    public event Action GameLost;

    private void OnWin()
    {
        Score += scoreIncreasePerPuzzle;
        Coins += coinIncreasePerPuzzleSolved;
        PuzzleRunning = false;
    }

    private void OnLevelReload()
    {
        PuzzleRunning = true;
        ReEvaluateTimer();
    }

    private void ReEvaluateTimer()
    {
        CurrentLevelTimer =
            timeMultiplierOverCompletion.Evaluate(LevelCompletionPercentage) * baseTimePerLevel;
        print(CurrentLevelTimer);
    }

    public void ResetValues()
    {
        Score = startScore;
        PuzzleRunning = true;
        ReEvaluateTimer();
    }
}
