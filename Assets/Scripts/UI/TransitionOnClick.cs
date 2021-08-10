using UnityEngine;
using UnityEngine.UI;

public class TransitionOnClick : MonoBehaviour
{
    [SerializeField] private RectTransform from;
    [SerializeField] private RectTransform to;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MenuChanger.Instance.Transition(from, to);
    }
}
