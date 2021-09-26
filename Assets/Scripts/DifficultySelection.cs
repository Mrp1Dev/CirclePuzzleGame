using UnityEngine;
using UnityEngine.UI;

public class DifficultySelection : MonoBehaviour
{
    [SerializeField] private int easySliceCount = 3;
    [SerializeField] private int easyCoinIncrease = 1;
    [SerializeField] private int hardSliceCount = 4;
    [SerializeField] private int hardCoinIncrease = 2;
    [SerializeField] private Image easyButton;
    [SerializeField] private Image hardButton;
    [SerializeField] private GameObject easyBorder;
    [SerializeField] private GameObject hardBorder;
    [SerializeField] private Player player;

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.DifficultySettingKey, 0) == 0)
            OnEasyModeSelected();
        else OnHardModeSelected();
        gameObject.SetActive(false);
    }

    public void OnEasyModeSelected()
    {
        easyBorder.SetActive(true);
        hardBorder.SetActive(false);
        var settings = PuzzleManager.Instance.DefaultSettings;
        settings.sliceCount = easySliceCount;
        PuzzleManager.Instance.DefaultSettings = settings;
        player.CoinIncreasePerPuzzleSolved = easyCoinIncrease;
        PlayerPrefs.SetInt(PlayerPrefsKeys.DifficultySettingKey, 0);
    }

    public void OnHardModeSelected()
    {
        easyBorder.SetActive(false);
        hardBorder.SetActive(true);
        var settings = PuzzleManager.Instance.DefaultSettings;
        settings.sliceCount = hardSliceCount;
        PuzzleManager.Instance.DefaultSettings = settings;
        player.CoinIncreasePerPuzzleSolved = hardCoinIncrease;
        PlayerPrefs.SetInt(PlayerPrefsKeys.DifficultySettingKey, 1);
    }
}
