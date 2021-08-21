using UnityEngine;
using UnityEngine.UI;

public class TransitionOnClick : MonoBehaviour
{
    [SerializeField] private RectTransform from;
    [SerializeField] private RectTransform to;
    [SerializeField] private bool autoRegister = true;

    private void Start()
    {
        if (autoRegister)
            GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (TryGetComponent<ImagePackSelection>(out var packSelection) && packSelection.BuyState == false) return;
        MenuChanger.Instance.Transition(from, to);
    }
}