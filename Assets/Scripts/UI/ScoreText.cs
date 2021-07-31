using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        tmpText.text = $"Score: {Mathf.RoundToInt(Player.Instance.Score)}";
    }
}
