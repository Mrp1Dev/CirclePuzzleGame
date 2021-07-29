using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    [SerializeField] private List<Sprite> puzzleImages;

    public static event Action LevelReload;
    private int currentIndex;

    private void Start()
    {
        nextPuzzleButton.onClick.AddListener(OnNextPuzzleClick);
        OnNextPuzzleClick();
    }
    private void OnNextPuzzleClick()
    {
        LevelReload?.Invoke();
        var settings = PuzzleManager.Instance.DefaultSettings;
        if (currentIndex >= puzzleImages.Count) return;
        settings.image = puzzleImages[currentIndex];
        PuzzleManager.Instance.GeneratePuzzle(settings);
        currentIndex++;
    }
}
