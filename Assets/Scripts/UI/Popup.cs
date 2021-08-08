using DG.Tweening;
using MUtility;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private Ease ease = Ease.OutBounce;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        Animate();
    }

    public void Animate()
    {
        transform.localScale = Vector2.zero.WithZ(1);
        transform.DOScale(initialScale, animationDuration).SetEase(ease);
    }
}
