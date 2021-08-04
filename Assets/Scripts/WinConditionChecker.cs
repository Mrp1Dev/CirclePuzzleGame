using System;
using UnityEngine;

public class WinConditionChecker : Singleton<WinConditionChecker>
{
    [SerializeField] private float toleranceAngle;

    private void Update()
    {
        if (!Player.Instance.PuzzleRunning) return;
        foreach (var piece in PuzzleManager.Instance.CurrentlyActivePieces)
            if (Vector2.Angle(piece.Image.up, PuzzleManager.Instance.CurrentlyActivePieces[0].Image.up) >
                toleranceAngle)
                return;

        GameWon?.Invoke();
    }

    public static event Action GameWon;
}