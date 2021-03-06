using System.Collections.Generic;
using MUtility;
using UnityEngine;
using System.Linq;
public class PuzzleRotator : MonoBehaviour
{
    [SerializeField] private LayerMask puzzleLayer;
    [Header("Audio")]
    [SerializeField] private AudioSource clickSoundEffect;
    [SerializeField] private List<AudioClip> clickAudioClips;
    [SerializeField] private DisableWithEase finger;
    private PuzzlePiece currentlyHeldPiece;
    private Vector2 previousMouseDir = Vector2.zero;

    private Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition).XY();

    private void Start()
    {
        WinConditionChecker.GameWon += () => ToggleSelectionSprite(false);
    }

    private void OnDestroy()
    {
        WinConditionChecker.GameWon -= () => ToggleSelectionSprite(false);
    }

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
                    if (hit.transform.localScale.sqrMagnitude >= smallestSize) continue;
                    smallestSize = hit.transform.localScale.sqrMagnitude;
                    currentlyHeldPiece = hit.transform.GetComponent<PuzzlePiece>();
                }
                finger.Disable();
            }

            ToggleSelectionSprite(true);
            previousMouseDir = DirToMouse();
        }

        if (Input.GetMouseButton(0) && currentlyHeldPiece != null)
        {
            currentlyHeldPiece.Image.localRotation =
                Quaternion.Euler(
                    Vector3.forward * Vector2.SignedAngle(previousMouseDir, DirToMouse())) *
                currentlyHeldPiece.Image.localRotation;
            previousMouseDir = DirToMouse();
            if (clickSoundEffect.isPlaying) return;
            clickSoundEffect.clip = clickAudioClips.RandomElement();
            clickSoundEffect.Play();
        }
        else
            clickSoundEffect.Stop();
    }

    private void ToggleSelectionSprite(bool newActive)
    {
        if (currentlyHeldPiece == null) return;
        currentlyHeldPiece.Border.gameObject.SetActive(newActive);
    }

    private Vector2 DirToMouse() => transform.position.XY().DirTo(MousePos);
}