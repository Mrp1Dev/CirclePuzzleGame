using UnityEngine;
using MUtility;
using System.Linq;
public class PuzzleRotator : MonoBehaviour
{
    [SerializeField] private LayerMask puzzleLayer;
    private PuzzlePiece currentlyHeldPiece = null;
    private Vector2 previousMouseDir = Vector2.zero;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyHeldPiece == null)
            {
                var hits = Physics2D.OverlapCircleAll(MousePos, 0.001f, puzzleLayer);
                if (hits.Length > 0)
                {
                    var smallestSize = float.MaxValue;
                    foreach (var hit in hits)
                    {
                        if (hit.transform.localScale.sqrMagnitude < smallestSize)
                        {
                            smallestSize = hit.transform.localScale.sqrMagnitude;
                            currentlyHeldPiece = hit.transform.GetComponent<PuzzlePiece>();
                        }
                    }
                }
            }
            previousMouseDir = DirToMouse();
        }
        if (currentlyHeldPiece != null)
        {
            currentlyHeldPiece.CorrespondingImage.localRotation = Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(previousMouseDir, DirToMouse())) * currentlyHeldPiece.CorrespondingImage.localRotation;
            previousMouseDir = DirToMouse();
        }

        if (Input.GetMouseButtonUp(0)) currentlyHeldPiece = null;
    }

    private Vector2 DirToMouse()
    {
        return (MousePos - transform.position.XY()).normalized;
    }

    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition).XY();
}
