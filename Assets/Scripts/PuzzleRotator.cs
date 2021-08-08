using MUtility;
using UnityEngine;

public class PuzzleRotator : MonoBehaviour
{
    [SerializeField] private LayerMask puzzleLayer;
    private PuzzlePiece currentlyHeldPiece;
    private Vector2 previousMouseDir = Vector2.zero;

    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition).XY();

    private void Update()
    {
        if (!Player.Instance.PuzzleRunning) return;
        if (Input.GetMouseButtonDown(0))
        {
            var hits = Physics2D.OverlapCircleAll(MousePos, 0.001f, puzzleLayer);
            if (hits.Length > 0)
            {
                if (currentlyHeldPiece != null) ToggleSelectionSprite(false);
                var smallestSize = float.MaxValue;
                foreach (var hit in hits)
                {
                    if (!(hit.transform.localScale.sqrMagnitude < smallestSize)) continue;
                    smallestSize = hit.transform.localScale.sqrMagnitude;
                    currentlyHeldPiece = hit.transform.GetComponent<PuzzlePiece>();
                }
            }

            ToggleSelectionSprite(true);
            previousMouseDir = DirToMouse();
        }

        if (Input.GetMouseButton(0) && currentlyHeldPiece != null)
        {
            currentlyHeldPiece.Image.localRotation =
                Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(previousMouseDir, DirToMouse())) *
                currentlyHeldPiece.Image.localRotation;
            previousMouseDir = DirToMouse();
        }
    }

    private void ToggleSelectionSprite(bool newActive)
    {
        if(currentlyHeldPiece == null) return;
        currentlyHeldPiece.Border.gameObject.SetActive(newActive);
    }

    private Vector2 DirToMouse() => (MousePos - transform.position.XY()).normalized;
}
