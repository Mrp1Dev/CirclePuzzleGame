using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    [SerializeField] private string formatString = "{0}";

    private void Update()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, Player.Instance != null ? Player.Instance.Coins : PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0));
    }
}
