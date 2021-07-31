using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    private List<Sprite> puzzleImages;

    public static event Action LevelReload;
    private int currentIndex;

    public int PuzzleCount => puzzleImages.Count;
    public int CurrentlySolvedPuzzles => currentIndex + 1;
    private void Start()
    {
        nextPuzzleButton.onClick.AddListener(OnNextPuzzleClick);
    }
    public void Init(List<Sprite> puzzleImages)
    {
        this.puzzleImages = puzzleImages;
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
