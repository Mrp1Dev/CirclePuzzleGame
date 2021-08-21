using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] private float neededWidth;

    private void Start()
    {
        GetComponent<Camera>().orthographicSize = neededWidth * ((float) Screen.height / Screen.width) * 0.5f;
    }
}