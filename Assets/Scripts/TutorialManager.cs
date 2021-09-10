using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private DisableWithEase firstTutorialText;
    [SerializeField] private GameObject finger;
    public static bool TutorialFinished { get; private set; }

    private void Awake()
    {
        TutorialFinished = PlayerPrefs.GetInt(PlayerPrefsKeys.TutorialCompleted, 0) == 1;
        finger.SetActive(TutorialFinished == false);
    }

    private void Start()
    {
        WinConditionChecker.GameWon += OnWin;
    }

    private void OnWin()
    {
        if (firstTutorialText.gameObject.activeSelf)
        {
            firstTutorialText.Disable();
            if (TutorialFinished == false)
            {
                FirebaseManager.Instance.OnTutorialComplete();
                TutorialFinished = true;
                PlayerPrefs.SetInt(PlayerPrefsKeys.TutorialCompleted, 1);
            }
        }
    }
}
