using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private float timeToEnlarge;
    private float initialOffsetMax = 120f;
    private float DeltaPerSecond => initialOffsetMax / timeToEnlarge;

    private void Start()
    {
        initialOffsetMax = GetComponent<RectTransform>().offsetMax.y;
    }

    private void Update()
    {
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(rect.offsetMax.x, Mathf.Min(rect.offsetMax.y -
                                                                 DeltaPerSecond * Time.deltaTime, 0.0f));
    }

    private void OnDisable() => ResetRect();

    private void ResetRect()
    {
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(rect.offsetMax.x, initialOffsetMax);
    }
}
