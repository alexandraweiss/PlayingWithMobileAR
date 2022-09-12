using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlight : GAction
{
    public override bool postPerform()
    {
        Invoke("GetHungry", 5f);
        return true;
    }

    public override bool prePerform()
    {
        return true;
    }

    protected void GetHungry()
    {
        beliefs.setState("satiated", 0);
        beliefs.setState("hungry", 1);
    }
}
