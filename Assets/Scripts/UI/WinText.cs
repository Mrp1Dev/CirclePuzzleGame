using UnityEngine;

public class WinText : MonoBehaviour
{
    private void Start()
    {
        GameManager.GameWon += OnWin;
        gameObject.SetActive(false);
    }

    private void OnWin() => gameObject.SetActive(true);
}
