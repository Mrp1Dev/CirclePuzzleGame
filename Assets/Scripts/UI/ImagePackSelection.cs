using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePackSelection : MonoBehaviour
{
    [SerializeField] private PuzzlePack pack;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => PuzzleCycler.Instance.Init(pack));
    }
}
