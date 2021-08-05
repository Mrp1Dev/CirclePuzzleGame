using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSaveButton : MonoBehaviour
{
    public void OnClick()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayerPrefs.Save();
    }
}
