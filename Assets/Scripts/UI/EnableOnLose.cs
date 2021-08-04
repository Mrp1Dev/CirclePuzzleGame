using UnityEngine;

public class EnableOnLose : MonoBehaviour
{
    private void Awake()
    {
        Player.Instance.GameLost += OnLose;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.Instance.GameLost -= OnLose;
    }

    private void OnLose() => gameObject.SetActive(true);
}