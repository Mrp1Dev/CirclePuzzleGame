using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    [SerializeField] private string formatString = "{0}";
    [SerializeField] private bool currentHighScoreMode = true;
    [SerializeField] private PuzzlePack pack;
    [SerializeField] private bool endlessMode;

    private void OnEnable()
    {
        if (!endlessMode) return;
        if (Player.Instance.Score > PlayerPrefsHighScore)
            PlayerPrefs.SetInt(PlayerPrefsKeys.EndlessHighScoreKey, Mathf.RoundToInt(Player.Instance.Score));
    }

    private void Update()
    {
        if (endlessMode)
        {
            GetComponent<TMP_Text>().text = string.Format(formatString,
                PlayerPrefsHighScore);
            return;
        }

        GetComponent<TMP_Text>().text = string.Format(formatString,
            currentHighScoreMode
                ? Mathf.RoundToInt(PuzzleCycler.Instance.SelectedPack.CurrentHighScore)
                : pack.CurrentHighScore);
    }

    private static int PlayerPrefsHighScore =>
        PlayerPrefs.GetInt(PlayerPrefsKeys.EndlessHighScoreKey, Mathf.RoundToInt(Player.Instance.Score));
}
