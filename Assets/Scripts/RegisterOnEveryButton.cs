using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterOnEveryButton : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    private void Start()
    {
        buttons.ForEach(b => b.onClick.AddListener(GetComponent<AudioSource>().Play));
    }
}