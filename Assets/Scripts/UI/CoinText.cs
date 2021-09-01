using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    [SerializeField] private string formatString = "{0}";
    [SerializeField] private bool showOnlyCurrentPackEarnings;

    private void Update()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString,
            Player.Instance == null
                ? PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0)
                : Player.Instance.Coins);
    }
}