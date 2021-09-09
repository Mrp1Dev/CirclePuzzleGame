using DG.Tweening;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    [SerializeField] private float spinDuration;
    [SerializeField] private Ease ease;

    public void RotateCoin()
    {
        transform.DOLocalRotate(Vector3.forward * 360, spinDuration).SetRelative().SetEase(ease);
        transform.DOScale(new Vector3(1.35f, 1.35f, 1.0f), spinDuration / 2.0f).SetLoops(2, LoopType.Yoyo)
            .SetEase(ease);
    }
}
