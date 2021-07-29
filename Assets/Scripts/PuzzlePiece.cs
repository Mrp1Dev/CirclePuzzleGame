using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform Image { get; set; }
    private void OnDisable()
    {
        Image = null;
    }
}
