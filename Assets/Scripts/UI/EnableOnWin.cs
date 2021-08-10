using UnityEngine;

public class EnableOnWin : MonoBehaviour
{
    [SerializeField] private bool enableOnFinalWin;

    private void Awake()
    {
        WinConditionChecker.GameWon += OnWin;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        WinConditionChecker.GameWon -= OnWin;
    }

    private void OnWin()
    {
        if (PuzzleCycler.Instance.PuzzleCount - PuzzleCycler.Instance.CurrentlySolvedPuzzles <= 0 &&
            enableOnFinalWin == false) return;
        gameObject.SetActive(true);
    }
}