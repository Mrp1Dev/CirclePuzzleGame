using System.Collections;
using System.Collections.Generic;
using MUtility;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float secondsAfterEnabled;

    private void OnEnable()
    {
        this.DelayUnscaled(() => gameObject.SetActive(false), secondsAfterEnabled);
    }
}
