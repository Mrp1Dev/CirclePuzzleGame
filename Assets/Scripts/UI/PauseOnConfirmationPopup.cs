using UnityEngine;

public class PauseOnConfirmationPopup : MonoBehaviour
{
    private void OnEnable()
    {
        Player.Instance.PuzzleRunning = false;
    }

    public void UnPause()
    {
        Player.Instance.PuzzleRunning = true;
    }
}
