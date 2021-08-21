using DG.Tweening;
using UnityEngine;

public class MenuChanger : Singleton<MenuChanger>
{
    [SerializeField] private float transitionDuration;
    [SerializeField] private Ease ease;


    public void Transition(RectTransform fromAnchor, RectTransform toAnchor)
    {
        fromAnchor.parent.gameObject.SetActive(true);
        toAnchor.parent.gameObject.SetActive(true);

        fromAnchor.DOMoveX(-fromAnchor.position.x,
                transitionDuration)
            .SetEase(ease).OnComplete(() => fromAnchor.parent.gameObject.SetActive(false));
        toAnchor.position = Vector3.up * toAnchor.position.y + Vector3.right * fromAnchor.position.x * 4;
        toAnchor.DOMoveX(fromAnchor.position.x, transitionDuration).SetEase(ease);
    }
}