using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    private IEnumerator Start()
    {
        var operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }
}
