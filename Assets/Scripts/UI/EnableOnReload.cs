using UnityEngine;

public class EnableOnReload : MonoBehaviour
{
    private void Start()
    {
        PuzzleCycler.LevelReload += Enable;
    }

    private void OnDestroy()
    {
        PuzzleCycler.LevelReload -= Enable;
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }
}
