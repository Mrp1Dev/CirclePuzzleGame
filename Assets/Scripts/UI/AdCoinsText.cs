using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdCoinsText : MonoBehaviour
{
    [SerializeField] private string formatString = "Get <color=yellow>{0}</color> Coins";

    private void Update()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, AdManager.Instance.CoinIncreaseOnWatch);
    }
}
