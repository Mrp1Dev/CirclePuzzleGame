using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GameObjectActive
{
    public GameObject gameObject;
    public bool active;
}

public class ImagePackSelection : MonoBehaviour
{
    [SerializeField] private PuzzlePack pack;
    [SerializeField] private bool freePack = true;
    [SerializeField] private Color notAffordableColor;
    [SerializeField] private Color affordableColor;
    [SerializeField] private Color boughtColor;

    [SerializeField] private bool scoreUnlock;

    [DisableIf(nameof(scoreUnlock))] [SerializeField]
    private int cost;

    [EnableIf(nameof(scoreUnlock))] [SerializeField]
    private int scoreRequirement;

    [SerializeField] private GameObject highScoreText;
    [SerializeField] private GameObject costOrScoreText;
    [SerializeField] private string costOrScoreFormatString = "Cost: {0}";
    [SerializeField] private List<GameObjectActive> gameObjectActiveModeToSetOnPlay;
    [SerializeField] private GameObject notEnoughCoinsPanel;
    private bool buyState;

    public bool BuyState
    {
        get => buyState;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.GetPackBuyStateKey(pack), value ? 1 : 0);
            buyState = value;
        }
    }

    private static int PlayerPrefsCoins => PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0);

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        BuyState = PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackBuyStateKey(pack), freePack ? 1 : 0) == 1;
        costOrScoreText.GetComponent<TMP_Text>().text = scoreUnlock
            ? string.Format(costOrScoreFormatString, scoreRequirement)
            : string.Format(costOrScoreFormatString, cost);
    }

    private void Update()
    {
        if (BuyState)
            GetComponent<Image>().color = boughtColor;
        else
        {
            var condition = scoreUnlock
                ? TotalBestScore.Instance.TotalScore > scoreRequirement
                : (Player.Instance == null ? PlayerPrefsCoins : Player.Instance.Coins) >=
                  cost;
            GetComponent<Image>().color = condition ? affordableColor : notAffordableColor;
        }


        highScoreText.SetActive(BuyState);
        costOrScoreText.SetActive(!BuyState);
    }


    private void OnClick()
    {
        if (BuyState)
        {
            foreach (var gameObjectActiveMode in gameObjectActiveModeToSetOnPlay)
                gameObjectActiveMode.gameObject.SetActive(gameObjectActiveMode.active);

            PuzzleCycler.Instance.Init(pack);
        }
        else
        {
            if (scoreUnlock)
            {
                if (TotalBestScore.Instance.TotalScore > scoreRequirement) BuyState = true;
            }
            else
            {
                if (Player.Instance != null)
                {
                    if (Player.Instance.Coins >= cost)
                    {
                        Player.Instance.Coins -= cost;
                        BuyState = true;
                    }
                    else
                        notEnoughCoinsPanel.SetActive(true);
                }
                else if (PlayerPrefs.GetInt(PlayerPrefsKeys.CoinsKey, 0) >= cost)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.CoinsKey, PlayerPrefsCoins - cost);
                    BuyState = true;
                }
                else
                    notEnoughCoinsPanel.SetActive(true);
            }
        }
    }
}
