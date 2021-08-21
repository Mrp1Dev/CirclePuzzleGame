using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    [SerializeField] private string formatString;
    private TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var timer = Mathf.RoundToInt(Player.Instance.CurrentLevelTimer);
        tmpText.text = string.Format(formatString, timer);
    }
}