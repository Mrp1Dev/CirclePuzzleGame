using System.Collections;
using TMPro;
using UnityEngine;

public class PackScoreWithAnimation : MonoBehaviour
{
    [SerializeField] private float timeToReachFinalValue;
    [SerializeField] private string formatString = "{0}";
    private TMP_Text text;

    private void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        text.text = "0";
        StartCoroutine(CountUp());
    }

    private IEnumerator CountUp()
    {
        var currentValue = 0f;
        var increment = Player.Instance.Score / timeToReachFinalValue;
        while (currentValue < Player.Instance.Score)
        {
            GetComponent<TMP_Text>().text = string.Format(formatString, Mathf.RoundToInt(currentValue));
            currentValue += increment * Time.deltaTime;
            yield return null;
        }

        GetComponent<TMP_Text>().text = string.Format(formatString, Mathf.RoundToInt(Player.Instance.Score));
    }
}