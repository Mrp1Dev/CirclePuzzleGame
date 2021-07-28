using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] private int sliceCount;
    [SerializeField] private float diameter;
    [SerializeField] private GameObject maskPrefab;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Transform maskHolderParent;
    [SerializeField] private Transform imageHolderParent;
    [SerializeField] private Sprite image;
    [SerializeField] private int sortingOrderOffset;

    private void Start()
    {
        GeneratePuzzle();
    }

    private void GeneratePuzzle()
    {
        float sizeDelta = diameter / sliceCount;
        for (int i = sortingOrderOffset; i < sliceCount + sortingOrderOffset; i++)
        {
            var sprite = SpawnSprite(i);
            SpawnMask(sizeDelta, i, sprite);
        }
    }
    
    private void SpawnMask(float sizeDelta, int i, Transform correspondingImage)
    {
        var mask = PoolingManager.Instance.GetFromPool(maskPrefab, maskHolderParent, false).GetComponent<SpriteMask>();
        mask.frontSortingOrder = i;
        var scale = diameter - sizeDelta * i;
        mask.transform.localScale = new Vector2(scale, scale);
        mask.GetComponent<PuzzlePiece>().CorrespondingImage = correspondingImage;
    }

    private Transform SpawnSprite(int i)
    {
        var instantiated = PoolingManager.Instance.GetFromPool(imagePrefab, imageHolderParent, false).GetComponent<SpriteRenderer>();
        instantiated.sprite = image;
        instantiated.sortingOrder = i;
        instantiated.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        instantiated.transform.localScale = new Vector2(diameter, diameter);
        return instantiated.transform;
    }
}
