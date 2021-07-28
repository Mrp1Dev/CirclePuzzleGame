using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float toleranceAngle;
    public static event Action GameWon;
    private static List<PuzzlePiece> currentlyActivePieces = new List<PuzzlePiece>();

    private void Update()
    {
        foreach (var piece in currentlyActivePieces)
        {
            if (Vector2.Angle(piece.CorrespondingImage.up, currentlyActivePieces[0].CorrespondingImage.up) > toleranceAngle) return;
        }
        GameWon?.Invoke();
    }

    public static void Register(PuzzlePiece piece) => currentlyActivePieces.Add(piece);
    public static void DeRegister(PuzzlePiece piece) => currentlyActivePieces.Remove(piece);
}
