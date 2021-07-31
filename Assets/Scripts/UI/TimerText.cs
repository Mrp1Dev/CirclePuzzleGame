using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    private TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        tmpText.text = $"Time Left: {Mathf.RoundToInt(Player.Instance.CurrentLevelTimer)}";
    }
}
