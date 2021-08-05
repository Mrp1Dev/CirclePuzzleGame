using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TotalBestScore : Singleton<TotalBestScore>
{
    [SerializeField] private string formatString = "Total Best Score: {0}";
    [SerializeField] private List<PuzzlePack> packs;
    public int TotalScore => packs.Select(p => p.CurrentHighScore).Sum();
    private void OnEnable()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, TotalScore);
    }
}
