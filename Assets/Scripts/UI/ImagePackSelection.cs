using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
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
    [SerializeField] private int cost;
    [SerializeField] private GameObject highscoreText;
    [SerializeField] private GameObject costText;
    [SerializeField] private string costFormatString = "Cost: {0}";
    [SerializeField] private List<GameObjectActive> gameObjectActiveModeToSetOnPlay;
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

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        BuyState = PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackBuyStateKey(pack), freePack ? 1 : 0) == 1;
        costText.GetComponent<TMP_Text>().text = string.Format(costFormatString, cost);
    }

    private void Update()
    {
        GetComponent<Image>().color = BuyState
            ? boughtColor
            : (Player.Instance == null ? 0 : Player.Instance.Coins) >= cost
                ? affordableColor
                : notAffordableColor;
        highscoreText.SetActive(BuyState);
        costText.SetActive(!BuyState);
    }

    private void OnClick()
    {
        if (BuyState)
        {
            foreach (var gameObjectActiveMode in gameObjectActiveModeToSetOnPlay)
            {
                gameObjectActiveMode.gameObject.SetActive(gameObjectActiveMode.active);
            }
            PuzzleCycler.Instance.Init(pack);
        }
        else
        {
            if (Player.Instance != null && Player.Instance.Coins >= cost)
            {
                Player.Instance.Coins -= cost;
                BuyState = true;
            }
            else
            {
                //todo: cant afford dialog
            }
        }
    }
}
