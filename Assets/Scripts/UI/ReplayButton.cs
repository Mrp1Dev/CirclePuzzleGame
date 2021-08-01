using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    [SerializeField] private bool regeneratePuzzleOnReplay = false;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PuzzleCycler.Instance.ResetValues(false, regeneratePuzzleOnReplay);
        Player.Instance.ResetValues();
    }
}
