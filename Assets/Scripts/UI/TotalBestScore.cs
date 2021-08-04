using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TotalBestScore : MonoBehaviour
{
    [SerializeField] private string formatString = "Total Best Score: {0}";
    [SerializeField] private List<PuzzlePack> packs;
    private void OnEnable()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, packs.Select(p => p.CurrentHighScore).Sum());
    }
}
