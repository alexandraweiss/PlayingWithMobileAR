using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIndicator: MonoBehaviour
{
    protected float newScale = 1f;
    void Update()
    {
        newScale = 0.2f * Mathf.Cos(Time.time) + 0.8f;
        transform.localScale = new Vector3(newScale, 1f, newScale); ;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }
}
