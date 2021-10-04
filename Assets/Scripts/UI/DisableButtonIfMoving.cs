using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DisableButtonIfMoving : MonoBehaviour
{
    private Vector3 lastPos;
    private Button button;
    private void Start()
    {
        lastPos = transform.position;
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.enabled = lastPos == transform.position;
        lastPos = transform.position;
    }
}
