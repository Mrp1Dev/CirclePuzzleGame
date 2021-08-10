using UnityEngine;

public class ClearSaveButton : MonoBehaviour
{

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayerPrefs.Save();
    }

    public void OnClick()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}