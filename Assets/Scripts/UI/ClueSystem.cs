using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject solutionImage;
    [SerializeField] private GameObject noHintsPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (PuzzleCycler.Instance.EndlessMode) gameObject.SetActive(false);
        var count = PuzzleCycler.Instance.SelectedPack.ClueCount;
        countText.text = count > 0 ? count.ToString() : "+";
    }

    private void OnClick()
    {
        if(PuzzleCycler.Instance.SelectedPack.ClueCount > 0)
        {
            PuzzleCycler.Instance.SelectedPack.ClueCount--;
            solutionImage.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            noHintsPanel.SetActive(true);
        }
    }
}
