using MUtility;
using UnityEngine;

public class EnableOnLose : MonoBehaviour
{
    [SerializeField] private bool forceChildren;
    [SerializeField] private bool endlessMode;

    private void Awake()
    {
        Player.Instance.GameLost += OnLose;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.Instance.GameLost -= OnLose;
    }

    private void OnLose()
    {
        if (endlessMode != PuzzleCycler.Instance.EndlessMode) return;
        if (forceChildren) gameObject.SetActiveIncludingChildren(true);
        else gameObject.SetActive(true);
    }
}
