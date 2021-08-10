using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzlePack", menuName = "SOs/PuzzlePack")]
public class PuzzlePack : ScriptableObject
{

    private int currentHighScore;
    [field: SerializeField] public List<Sprite> Images { get; private set; }

    public int CurrentHighScore
    {
        get => currentHighScore;
        set
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.GetPackHighScoreKey(this), value);
            currentHighScore = value;
        }
    }

    [Button("Reload")]
    private void Awake()
    {
        currentHighScore = PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackHighScoreKey(this), 0);
        Debug.Log(PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackHighScoreKey(this), 0));
    }
}
