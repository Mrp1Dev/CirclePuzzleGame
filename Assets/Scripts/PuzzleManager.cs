using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleManager : Singleton<PuzzleManager>
{
    private Vector3 lastUp = Vector3.up;

    [field: SerializeField] public PuzzleGenerationSettings DefaultSettings { get; set; }

    public List<PuzzlePiece> CurrentlyActivePieces { get; } = new List<PuzzlePiece>();

    private void Start()
    {
    }

    public void GeneratePuzzle(PuzzleGenerationSettings settings, bool clearCurrent = true)
    {
        if (clearCurrent)
        {
            for (var i = CurrentlyActivePieces.Count - 1; i >= 0; i--)
            {
                PoolingManager.Instance.ReturnToPool(CurrentlyActivePieces[i].Image.gameObject);
                PoolingManager.Instance.ReturnToPool(CurrentlyActivePieces[i].gameObject);
                CurrentlyActivePieces.RemoveAt(i);
            }
        }

        for (var i = settings.sortingOrderOffset; i < settings.sliceCount + settings.sortingOrderOffset; i++)
        {
            var sprite = SpawnSprite(i, settings);
            SpawnMask(i, sprite, settings);
        }
    }

    private void SpawnMask(int i, Transform correspondingImage, PuzzleGenerationSettings settings)
    {
        var sizeDelta = settings.diameter / settings.sliceCount;
        var mask = PoolingManager.Instance.GetFromPool(settings.maskPrefab, settings.maskHolderParent)
            .GetComponent<SpriteMask>();
        mask.frontSortingOrder = i;
        var scale = settings.diameter - sizeDelta * i;
        mask.transform.localScale = new Vector2(scale, scale);
        mask.GetComponent<PuzzlePiece>().Image = correspondingImage;
        CurrentlyActivePieces.Add(mask.GetComponent<PuzzlePiece>());
    }

    private Transform SpawnSprite(int i, PuzzleGenerationSettings settings)
    {
        var instantiated = PoolingManager.Instance
            .GetFromPool(settings.imagePrefab, settings.imageHolderParent)
            .GetComponent<SpriteRenderer>();
        instantiated.sprite = settings.image;
        instantiated.sortingOrder = i;
        instantiated.transform.localScale = new Vector2(settings.diameter, settings.diameter);
        if (i == settings.sliceCount - 1)
        {
            instantiated.transform.up = Vector3.up;
        }
        else
        {
            do
            {
                instantiated.transform.rotation =
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
            } while (Vector3.Dot(instantiated.transform.up, lastUp) > 0.85f);
        }

        lastUp = instantiated.transform.up;
        return instantiated.transform;
    }

    [Serializable]
    public struct PuzzleGenerationSettings
    {
        public int sliceCount;
        public float diameter;
        public GameObject maskPrefab;
        public GameObject imagePrefab;
        public Transform maskHolderParent;
        public Transform imageHolderParent;
        public Sprite image;
        public int sortingOrderOffset;
    }
}