using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePackSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> puzzleImages = new List<Sprite>();
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => PuzzleCycler.Instance.Init(puzzleImages));

    }
}
