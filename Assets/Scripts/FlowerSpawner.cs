using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FlowerSpawner : MonoBehaviour
{
    protected List<GameObject> flowers = new List<GameObject>();
    [SerializeField]
    protected float maxAmount = 5;
    [SerializeField]
    protected float spawnTimer = 5f;
    protected float lastSpawned = 0f;
    [SerializeField]
    protected GameObject flowerPrefab;
    ARAnchorManager anchorManager;
    List<ARAnchor> anchors = new List<ARAnchor>();


    private void Start()
    {
        anchorManager = FindObjectOfType<ARAnchorManager>();
        SpawnFlower(); //TODO spawn over time
    }
    void Update()
    {
        if (Time.time >= spawnTimer + lastSpawned)
        {
            lastSpawned = Time.time;
            SpawnFlower();
        }
    }

    protected void SpawnFlower()
    {
        if (flowers.Count >= maxAmount)
        {
            return;
        }
        GWorld.Instance.getWorld().modifyState("availableFlowers", 1);
        GameObject newObj = Instantiate(flowerPrefab);
        
        newObj.transform.position = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(0.5f, 2.5f));
        newObj.transform.rotation = Quaternion.identity;
        newObj.transform.name = "flower_" + flowers.Count;
        newObj.tag = "Flower";
        flowers.Add(newObj);

        AddAnchor();
    }

    protected void AddAnchor()
    {
        //ARPlane plane = hits[0].trackable.GetComponent<ARPlane>();
        //ARAnchor anchor = anchorManager.AttachAnchor(plane, placementPose);
        //anchors.Add(anchor);
    }
}
