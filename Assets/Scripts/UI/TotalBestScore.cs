using System.Linq;
using TMPro;
using UnityEngine;

public class TotalBestScore : Singleton<TotalBestScore>
{
    [SerializeField] private string formatString = "Total Best Score: {0}";
    public int TotalScore => PackSupplier.Instance.Packs.Select(p => p.CurrentHighScore).Sum();

    private void OnEnable()
    {
        GetComponent<TMP_Text>().text = string.Format(formatString, TotalScore);
    }
}