using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DisableWithEase : MonoBehaviour
{
    [SerializeField] private float easingDuration;
    [SerializeField] private Ease ease;
    [SerializeField] private Transform target;
    public void Disable()
    {
        target.DOScale(Vector2.zero, easingDuration).SetEase(ease).onComplete += () => target.gameObject.SetActive(false);
    }
}
