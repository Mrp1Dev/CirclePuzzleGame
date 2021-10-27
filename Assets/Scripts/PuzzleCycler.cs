using System;
using System.Collections.Generic;
using DG.Tweening;
using MUtility;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PuzzleCycler : Singleton<PuzzleCycler>
{
    [SerializeField] private Button nextPuzzleButton;
    [SerializeField] private GameObject packWinPanel;
    [SerializeField] private float winPanelDelay = 2.5f;

    [SerializeField] private GameObject tutorialText;
    [Header("Puzzle Anim")]
    [SerializeField] private float puzzleDefaultHeight;
    [SerializeField] private float puzzleFinalHeight;
    [SerializeField] private float tweenTime;
    [SerializeField] private Ease ease;
    [SerializeField] private GameObject puzzle;


    //ENDLESS MODE
    private List<PuzzlePack> availablePacks;
    public bool EndlessMode { get; private set; }
    private HashSet<Sprite> alreadyShownPuzzles = new HashSet<Sprite>();
    [Header("Puzzle Choosing")]
    [SerializeField] private int iterationsToGetBestPuzzle = 5;
    [SerializeField, Range(0, 100)] private int resetHashSetThreshold;

    public PuzzlePack SelectedPack { get; private set; }

    public int PuzzleCount => EndlessMode ? int.MaxValue : SelectedPack.Images.Count;
    public int CurrentlySolvedPuzzles { get; private set; }

    private void Start()
    {
        nextPuzzleButton.onClick.AddListener(OnNextPuzzleClick);
        WinConditionChecker.GameWon += OnWin;
        Player.Instance.GameLost += OnGameLost;
    }

    private void OnGameLost()
    {
        if (EndlessMode) return;
        SelectedPack.LossCount++;
    }

    private void OnDestroy()
    {
        WinConditionChecker.GameWon -= OnWin;
        Player.Instance.GameLost -= OnGameLost;
    }

    public static event Action LevelReload;

    public void Init(PuzzlePack puzzleImages)
    {
        puzzleImages.ShuffleImages();
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
        puzzle.transform.DOLocalMoveY(puzzleFinalHeight, tweenTime).SetEase(ease);
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
        SelectedPack.LossCount--;
        this.DelayUnscaled(() => packWinPanel.SetActive(true), winPanelDelay);
    }

    public void ResetValues(bool clearImages = false, bool regeneratePuzzle = false)
    {
        CurrentlySolvedPuzzles = 0;
        if (clearImages) SelectedPack = null;
        if (regeneratePuzzle)
            OnNextPuzzleClick();
    }

    private Sprite GetRandomPuzzle(List<PuzzlePack> packs)
    {
        if ((float) alreadyShownPuzzles.Count / packs.Sum(p => p.Images.Count) > resetHashSetThreshold / 100f)
            alreadyShownPuzzles.Clear();
        Sprite res;
        var i = 0;
        do
        {
            res = packs.RandomElement().Images.RandomElement();
            i++;
        } while (alreadyShownPuzzles.Contains(res) && i < iterationsToGetBestPuzzle);
        alreadyShownPuzzles.Add(res);
        return res;
    }
}
