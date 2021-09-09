using DG.Tweening;
using UnityEngine;

public class DisableWithEase : MonoBehaviour
{
    [SerializeField] private float easingDuration;
    [SerializeField] private Ease ease;
    [SerializeField] private Transform target;
    [SerializeField] private bool overrideAnimator = true;
    public void Disable()
    {
        var animator = GetComponent<Animator>();
        if (overrideAnimator && animator) animator.enabled = false;
        target.DOScale(Vector2.zero, easingDuration).SetEase(ease).onComplete +=
            () =>
            {
                if (overrideAnimator && animator) animator.enabled = true;
                target.gameObject.SetActive(false);
            };
    }
}