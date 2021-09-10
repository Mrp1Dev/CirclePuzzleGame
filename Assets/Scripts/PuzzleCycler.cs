using System;
using System.Collections.Generic;
using DG.Tweening;
using MUtility;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    [SerializeField] private GameObject packWinPanel;
    [SerializeField] private float winPanelDelay = 2.5f;

    [SerializeField] private GameObject tutorialText;
    [Header("Puzzle Anim")]
    [SerializeField] private float puzzleDefaultHeight;
    [SerializeField] private float tweenTime;
    [SerializeField] private Ease ease;
    [SerializeField] private GameObject puzzle;


    //ENDLESS MODE
    private List<PuzzlePack> availablePacks;
    public bool EndlessMode { get; private set; }

    public PuzzlePack SelectedPack { get; private set; }

    public int PuzzleCount => EndlessMode ? int.MaxValue : SelectedPack.Images.Count;
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
        availablePacks = null;
        EndlessMode = false;
        tutorialText.SetActive(true);
        OnNextPuzzleClick();
    }

    public void InitEndless(List<PuzzlePack> availablePacks)
    {
        this.availablePacks = availablePacks;
        EndlessMode = true;
        OnNextPuzzleClick();
    }

    private void OnNextPuzzleClick()
    {
        if (EndlessMode == false && CurrentlySolvedPuzzles >= SelectedPack.Images.Count) return;

        LevelReload?.Invoke();
        puzzle.transform.localPosition = Vector3.up * puzzleDefaultHeight;
        puzzle.transform.DOLocalMoveY(0f, tweenTime).SetEase(ease);
        var settings = PuzzleManager.Instance.DefaultSettings;
        settings.image = EndlessMode ? GetRandomPuzzle(availablePacks) : SelectedPack.Images[CurrentlySolvedPuzzles];
        PuzzleManager.Instance.GeneratePuzzle(settings);
        CurrentlySolvedPuzzles++;
    }

    private void OnWin()
    {
        if (Instance.PuzzleCount - Instance.CurrentlySolvedPuzzles > 0) return;
        if (Player.Instance.Score > SelectedPack.CurrentHighScore)
            SelectedPack.CurrentHighScore = Mathf.RoundToInt(Player.Instance.Score);
        this.DelayUnscaled(() => packWinPanel.SetActive(true), winPanelDelay);
    }

    public void ResetValues(bool clearImages = false, bool regeneratePuzzle = false)
    {
        CurrentlySolvedPuzzles = 0;
        if (clearImages) SelectedPack = null;
        if (regeneratePuzzle)
            OnNextPuzzleClick();
    }

    private Sprite GetRandomPuzzle(List<PuzzlePack> packs) => packs.RandomElement().Images.RandomElement();
}