using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject solutionImage;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if(PuzzleCycler.Instance.SelectedPack.ClueCount > 0)
        {
            PuzzleCycler.Instance.SelectedPack.ClueCount--;
            solutionImage.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
