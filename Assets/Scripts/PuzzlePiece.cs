using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform CorrespondingImage { get; set; }

    private void OnEnable()
    {
        GameManager.Register(this);
    }

    private void OnDisable()
    {
        GameManager.DeRegister(this);
        CorrespondingImage = null;
    }
}
