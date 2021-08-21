using UnityEngine;
using UnityEngine.UI;

public class InterstitialButton : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int interstitialChance;

    [SerializeField] private bool puzzleRunningAfterInterstitial;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (Random.Range(0, 100) <= interstitialChance)
            AdManager.Instance.OnInterstitialWanted(puzzleRunningAfterInterstitial);
    }
}