using NaughtyAttributes;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private bool levelCountdownMode = false;
    [DisableIf(nameof(levelCountdownMode))]
    [SerializeField] private float timeToEnlarge;
    private float initialOffsetMax = 120f;
    private float DeltaPerSecond => initialOffsetMax / timeToEnlarge;

    private void Start()
    {
        initialOffsetMax = GetComponent<RectTransform>().offsetMax.y;
        if(levelCountdownMode) PuzzleCycler.LevelReload += ResetRect;
    }

    private void Update()
    {
        if (levelCountdownMode) timeToEnlarge = Player.Instance.LevelTimerMax;
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(rect.offsetMax.x, Mathf.Min(rect.offsetMax.y -
                                                                 DeltaPerSecond * Time.deltaTime, 0.0f));
    }

    private void OnDisable() => ResetRect();

    private void OnDestroy()
    {
        if (levelCountdownMode) PuzzleCycler.LevelReload -= ResetRect;
    }

    private void ResetRect()
    {
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(rect.offsetMax.x, initialOffsetMax);
    }
}