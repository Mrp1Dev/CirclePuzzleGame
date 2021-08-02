using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    [SerializeField] private string formatString = "{0}";
    [SerializeField] private bool currentHighScoreMode = true;
    [SerializeField] private PuzzlePack pack;

    private void Update()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, currentHighScoreMode ? Mathf.RoundToInt(PuzzleCycler.Instance.SelectedPack.CurrentHighScore) : pack.CurrentHighScore);

    }
}
