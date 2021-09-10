using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EndlessPlayButton : MonoBehaviour
{
    [SerializeField] private List<GameObjectActive> gameObjectsToSetActive;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        gameObjectsToSetActive.ForEach(go => go.gameObject.SetActive(go.active));
        var availablePacks =
            PackSupplier.Instance.Packs.Where(p => PlayerPrefs.GetInt(PlayerPrefsKeys.GetPackBuyStateKey(p), p.FreePack ? 1 : 0) == 1)
                .ToList();
        PuzzleCycler.Instance.InitEndless(availablePacks);
        Player.Instance.StartEndlessTimer();
        FirebaseManager.Instance.OnEndlessModeClicked();
    }
}
