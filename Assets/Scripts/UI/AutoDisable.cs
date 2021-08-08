using DG.Tweening;
using MUtility;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float secondsAfterEnabled;
    [SerializeField] private float easingDuration;
    [SerializeField] private Ease ease = Ease.Flash;
    private void OnEnable()
    {
        this.DelayUnscaled(() =>
        {
            transform.DOScale(Vector2.zero, easingDuration).SetEase(ease).onComplete +=
                () => gameObject.SetActive(false);

        }, secondsAfterEnabled);
    }
}
