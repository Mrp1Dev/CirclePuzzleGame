using DG.Tweening;
using MUtility;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float secondsAfterEnabled;
    [SerializeField] private float easingDuration;
    [SerializeField] private Ease ease = Ease.Flash;
    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = defaultScale;
        this.DelayUnscaled(() =>
        {
            transform.DOScale(Vector2.zero, easingDuration).SetEase(ease).onComplete +=
                () => gameObject.SetActive(false);
        }, secondsAfterEnabled);
    }
}
