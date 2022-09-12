using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFlower : GAction
{
    protected GameObject targetFlower;

    public override bool postPerform()
    {
        ButterflyMovement movementScript = GetComponent<ButterflyMovement>();
        if (movementScript != null)
        {
            movementScript.OnTargetReached();
        }
        if (targetFlower != null)
        {
            targetFlower.tag = "Untagged";
        }
        return true;
    }

    public override bool prePerform()
    {
        int flowerCount = 0;
        GWorld.Instance.getWorld().getStates().TryGetValue("availableFlowers", out flowerCount);
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        if (flowerCount >= 0)
        {
            ButterflyMovement movScript = GetComponent<ButterflyMovement>();
            targetFlower = flowers[0];
            movScript.SetTarget(targetFlower.transform);
            return true;
        }
        return false;
    }




}
