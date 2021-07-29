using UnityEngine;

public class DisableOnReload : MonoBehaviour
{
    private void Awake()
    {
        PuzzleCycler.LevelReload += OnReload;
    }

    private void OnDestroy()
    {
        PuzzleCycler.LevelReload -= OnReload;
    }

    private void OnReload() => gameObject.SetActive(false);
}
