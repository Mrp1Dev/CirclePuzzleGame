using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform CorrespondingImage { get; set; }

    private void OnDisable()
    {
        CorrespondingImage = null;
    }
}
