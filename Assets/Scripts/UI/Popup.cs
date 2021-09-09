using DG.Tweening;
using MUtility;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private Ease ease = Ease.OutBounce;
    [SerializeField] private float enableDelay;
    [SerializeField] private bool overrideAnimator = true;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        var animator = GetComponent<Animator>();
        if (overrideAnimator && animator) animator.enabled = false;
        transform.localScale = Vector2.zero.WithZ(1);
        this.Delay(Animate, enableDelay);
    }

    public void Animate()
    {
        var animator = GetComponent<Animator>();
        transform.localScale = Vector2.zero.WithZ(1);
        transform.DOScale(initialScale, animationDuration).SetEase(ease).OnComplete(() => { if (animator && overrideAnimator) animator.enabled = true; });
    }
}