using System;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    [SerializeField] private GameObject packWinPanel;
    public PuzzlePack SelectedPack { get; private set; }

    public int PuzzleCount => SelectedPack.Images.Count;
    public int CurrentlySolvedPuzzles { get; private set; }

    private void Start()
    {
        nextPuzzleButton.onClick.AddListener(OnNextPuzzleClick);
        WinConditionChecker.GameWon += OnWin;
    }

    private void OnDestroy()
    {
        WinConditionChecker.GameWon -= OnWin;
    }

    public static event Action LevelReload;

    public void Init(PuzzlePack puzzleImages)
    {
        SelectedPack = puzzleImages;
        OnNextPuzzleClick();
    }

    private void OnNextPuzzleClick()
    {
        if (CurrentlySolvedPuzzles >= SelectedPack.Images.Count) return;

        LevelReload?.Invoke();
        var settings = PuzzleManager.Instance.DefaultSettings;
        settings.image = SelectedPack.Images[CurrentlySolvedPuzzles];
        PuzzleManager.Instance.GeneratePuzzle(settings);
        CurrentlySolvedPuzzles++;
    }

    private void OnWin()
    {
        if (Instance.PuzzleCount - Instance.CurrentlySolvedPuzzles > 0) return;
        packWinPanel.SetActive(true);
        if (Player.Instance.Score > SelectedPack.CurrentHighScore)
            SelectedPack.CurrentHighScore = Mathf.RoundToInt(Player.Instance.Score);
    }

    public void ResetValues(bool clearImages = false, bool regeneratePuzzle = false)
    {
        CurrentlySolvedPuzzles = 0;
        if (clearImages) SelectedPack = null;
        if (regeneratePuzzle)
            OnNextPuzzleClick();
    }
}
