using System;
using UnityEngine;

public class WinConditionChecker : Singleton<WinConditionChecker>
{
    [SerializeField] private float toleranceAngle;
    public static event Action GameWon;
    bool gameWonCalled = false;

    private void Start()
    {
        PuzzleCycler.LevelReload += OnLevelReload;
    }

    private void OnDestroy()
    {
        PuzzleCycler.LevelReload -= OnLevelReload;
    }

    private void OnLevelReload()
    {
        gameWonCalled = false;
    }

    private void Update()
    {
        foreach (var piece in PuzzleManager.Instance.CurrentlyActivePieces)
        {
            if (Vector2.Angle(piece.Image.up, PuzzleManager.Instance.CurrentlyActivePieces[0].Image.up) >
                toleranceAngle) return;
        }

        if (!gameWonCalled)
            GameWon?.Invoke();
        gameWonCalled = true;
    }
}
