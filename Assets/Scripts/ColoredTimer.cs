using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MUtility;
using UnityEngine;

public class ColoredTimer : MonoBehaviour
{
    [SerializeField] private Color plentyTimeLeftColor;
    [SerializeField] private Color mediumTimeLeftColor;
    [SerializeField] private Color lowTimeLeftColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private GameObject gameScene;
    private float t = 0.0f;
    private float DeltaPerSecond => 1.0f / Player.Instance.LevelTimerMax;
    private bool normalMode;
    private bool allowLerp= false;
    private void Update()
    {
        var cam = GetComponent<Camera>();
        if ((Player.Instance == null || !Player.Instance.PuzzleRunning || gameScene.activeSelf == false))
        {
            allowLerp = false;
            t = 0.0f;
            if (normalMode) return;
            cam.DOColor(normalColor, 0.8f).SetEase(Ease.InOutCubic);
            normalMode = true;
            return;
        }

        if (cam.backgroundColor.ApproxEquals(normalColor) && Player.Instance.PuzzleRunning)
        {
            cam.DOColor(plentyTimeLeftColor, 0.8f).SetEase(Ease.InOutCubic).OnComplete(() => allowLerp = true);
            normalMode = false;
        }
        if(!allowLerp) return;
        cam.backgroundColor = t < 0.5f
            ? Color.Lerp(plentyTimeLeftColor, mediumTimeLeftColor, t.ReMap(0f, 0.5f, 0f, 1f))
            : Color.Lerp(mediumTimeLeftColor, lowTimeLeftColor, t.ReMap(0.5f, 1f, 0f, 1f));
        t += DeltaPerSecond * Time.deltaTime;
    }
}
