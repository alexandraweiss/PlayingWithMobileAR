using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButterflySpawner : MonoBehaviour
{
    public GameObject butterflyPrefab;
    void Start()
    {
#if UNITY_EDITOR
        if (butterflyPrefab != null)
        {
            GameObject butterfly = Object.Instantiate(butterflyPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("[ButterflySpawner]: prefab not assigned. Please check scene setup.");
        }
#endif
    }
}
