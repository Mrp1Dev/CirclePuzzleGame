using UnityEngine;

public class EnableOnWin : MonoBehaviour
{
    private void Awake()
    {
        WinConditionChecker.GameWon += OnWin;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        WinConditionChecker.GameWon -= OnWin;
    }

    private void OnWin() => gameObject.SetActive(true);
}