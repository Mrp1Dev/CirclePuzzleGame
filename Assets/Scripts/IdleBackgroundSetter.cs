using System.Collections.Generic;
using UnityEngine;

public class IdleBackgroundSetter : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToToggle;
    private void OnEnable()
    {
        objectsToToggle.ForEach(o => o.SetActive(false));
    }

    private void OnDisable()
    {
        objectsToToggle.ForEach(o => o.SetActive(true));
    }
}
