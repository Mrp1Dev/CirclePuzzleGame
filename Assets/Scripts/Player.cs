using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private float startScore;
    [SerializeField] private float scoreReductionPerSecond;
    [SerializeField] private float scoreIncreasePerPuzzle;
    [SerializeField] private float baseTimePerLevel;

    [Tooltip("Recommended to have a 0-1 value in both x and y axis. It is multiplied by baseTimePerLevel.")]
    [SerializeField]
    private AnimationCurve timeMultiplierOverCompletion;

    public bool PuzzleRunning { get; private set; } = true;
    public event Action GameLost;
    public float Score { get; private set; }

    private float LevelCompletionPercentage =>
        (float) PuzzleCycler.Instance.CurrentlySolvedPuzzles / PuzzleCycler.Instance.PuzzleCount;

    private float lastLevelCompletion;
    public float CurrentLevelTimer { get; private set; }

    private void Start()
    {
        Score = startScore;
        WinConditionChecker.GameWon += OnWin;
        PuzzleCycler.LevelReload += OnLevelReload;
    }


    private void Update()
    {
        if(!PuzzleRunning) return;
        CurrentLevelTimer -= Time.deltaTime;
        if (!Mathf.Approximately(lastLevelCompletion, LevelCompletionPercentage))
        {
            CurrentLevelTimer = timeMultiplierOverCompletion.Evaluate(LevelCompletionPercentage) * baseTimePerLevel;
        }

        if (CurrentLevelTimer <= 0)
        {
            GameLost?.Invoke();
            PuzzleRunning = false;
        }

        Score -= Mathf.Max(scoreReductionPerSecond * Time.deltaTime, 0);
        lastLevelCompletion = LevelCompletionPercentage;
    }

    private void OnWin()
    {
        Score += scoreIncreasePerPuzzle;
        PuzzleRunning = false;
    }

    private void OnLevelReload() => PuzzleRunning = true;
}
