using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzlePack", menuName = "SOs/PuzzlePack")]
public class PuzzlePack : ScriptableObject
{
    private int currentHighScore;
    private int clueCount;
    [field: SerializeField] public List<Sprite> Images { get; private set; }
    [field: SerializeField] public bool FreePack { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [SerializeField] private int defaultClues = 5;
    public int CurrentHighScore
    {
        get => currentHighScore;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.GetPackHighScoreKey(this), value);
            currentHighScore = value;
        }
    }

    public int ClueCount
    {
        get => clueCount;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.GetPackClueCountKey(this), value);
            clueCount = value;
        }
    }

    [Button("Reload")]
    private void Awake()
    {
        currentHighScore = PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackHighScoreKey(this), 0);
        clueCount = PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackClueCountKey(this), defaultClues);
        Debug.Log($"{name} cluecount set to: {clueCount}");
    }
}