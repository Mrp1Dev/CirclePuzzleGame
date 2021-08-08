using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform Image { get; set; }
    [field: SerializeField]
    public Transform Border { get; private set; }
}
