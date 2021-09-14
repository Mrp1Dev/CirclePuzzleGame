using UnityEngine;
using System.Collections.Generic;
using MUtility;

public class IdleRotator : MonoBehaviour
{
    [SerializeField] private List<Transform> sprites;
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        var s = PackSupplier.Instance.Packs.RandomElement().Images.RandomElement();
        sprites.ForEach(sp => sp.GetComponent<SpriteRenderer>().sprite = s);
    }

    private void Update()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].Rotate(Vector3.forward * rotateSpeed * Time.deltaTime * (i % 2 == 0 ? 1f : -1f));
        }
    }
}   
