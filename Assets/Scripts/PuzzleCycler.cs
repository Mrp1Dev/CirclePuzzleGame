using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    [SerializeField] private GameObject packWinPanel;
    private int currentIndex;
    public PuzzlePack SelectedPack { get; private set; }

    public int PuzzleCount => SelectedPack.Images.Count;
    public int CurrentlySolvedPuzzles => currentIndex + 1;

    private void Start()
    {
        nextPuzzleButton.onClick.AddListener(OnNextPuzzleClick);
    }

    public static event Action LevelReload;

    public void Init(PuzzlePack puzzleImages)
    {
        this.SelectedPack = puzzleImages;
        OnNextPuzzleClick();
    }

    private void OnNextPuzzleClick()
    {
        LevelReload?.Invoke();
        var settings = PuzzleManager.Instance.DefaultSettings;
        if (currentIndex >= SelectedPack.Images.Count)
        {
            packWinPanel.SetActive(true);
            if (Player.Instance.Score > SelectedPack.CurrentHighScore)
                SelectedPack.CurrentHighScore = Mathf.RoundToInt(Player.Instance.Score);
            return;
        }

        settings.image = SelectedPack.Images[currentIndex];
        PuzzleManager.Instance.GeneratePuzzle(settings);
        currentIndex++;
    }

    public void ResetValues(bool clearImages = false, bool regeneratePuzzle = false)
    {
        currentIndex = 0;
        if (clearImages) SelectedPack = null;
        if (regeneratePuzzle)
            OnNextPuzzleClick();
    }
}
