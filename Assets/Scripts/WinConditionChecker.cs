using System;
using UnityEngine;
using System.Linq;
public class WinConditionChecker : Singleton<WinConditionChecker>
{
    [SerializeField] private float toleranceAngle;
    [SerializeField] private float adjustmentSpeed;
    [SerializeField] private GameObject gameLostPanel;
    [SerializeField] private GameObject confirmationPopup;

    private void Update()
    {
        if (!Player.Instance.PuzzleRunning)
        {
            if (gameLostPanel.activeSelf || confirmationPopup.activeSelf) return;
            foreach (var piece in PuzzleManager.Instance.CurrentlyActivePieces)
                piece.Image.up = Vector3.RotateTowards(piece.Image.up,
                    PuzzleManager.Instance.CurrentlyActivePieces.Last().Image.up,
                    Mathf.Deg2Rad * adjustmentSpeed * Time.deltaTime, 0.0f);
            return;
        }

        foreach (var piece in PuzzleManager.Instance.CurrentlyActivePieces)
            if (Vector2.Angle(piece.Image.up,
                    PuzzleManager.Instance.CurrentlyActivePieces[0].Image.up) >
                toleranceAngle)
                return;
        GameWon?.Invoke();
    }

    public static event Action GameWon;
}