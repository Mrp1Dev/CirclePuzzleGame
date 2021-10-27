using UnityEngine;

public class PauseOnPopup : MonoBehaviour
{
    bool wasGameRunningOnEnable;
    private void OnEnable()
    {
        wasGameRunningOnEnable = Player.Instance.PuzzleRunning;
        Player.Instance.PuzzleRunning = false;
    }

    public void UnPause()
    {
        Player.Instance.PuzzleRunning = wasGameRunningOnEnable;
    }
}