using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedOnFlower : GAction
{
    public override bool postPerform()
    {
        GWorld.Instance.getWorld().modifyState("availableFlowers", -1);
        return true;
    }

    public override bool prePerform()
    {
        return true;
    }
}
