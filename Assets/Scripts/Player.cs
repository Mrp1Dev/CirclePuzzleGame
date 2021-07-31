using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private float startScore;
    [SerializeField] private float scoreReductionPerSecond;
    [SerializeField] private float scoreIncreasePerPuzzle;
    [SerializeField] private float baseTimePerLevel;
    [Tooltip("Recommended to have a 0-1 value in both x and y axis. It is multiplied by baseTimePerLevel.")]
    [SerializeField] private AnimationCurve timeMultiplierOverCompletion;

    public event Action GameLost;
    public float Score { get; private set; }
    private float LevelCompletionPercentage => (float) PuzzleCycler.Instance.CurrentlySolvedPuzzles / PuzzleCycler.Instance.PuzzleCount;
    private float lastLevelCompletion;
    public float CurrentLevelTimer { get; private set; }
    private void Start()
    {
        Score = startScore;
        WinConditionChecker.GameWon += OnWin;
    }

    private void Update()
    {
        CurrentLevelTimer -= Time.deltaTime;
        if (!Mathf.Approximately(lastLevelCompletion, LevelCompletionPercentage))
        {
            CurrentLevelTimer = timeMultiplierOverCompletion.Evaluate(LevelCompletionPercentage) * baseTimePerLevel;
        }

        if (CurrentLevelTimer <= 0)
        {
            GameLost?.Invoke();
        }
        Score -= Mathf.Max(scoreReductionPerSecond * Time.deltaTime, 0);
        lastLevelCompletion = LevelCompletionPercentage;
    }

    private void OnWin() => Score += scoreIncreasePerPuzzle;
}
