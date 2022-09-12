using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : GAgent
{
    
    protected override void Start()
    {
        base.Start();
        
        SubGoal sub1 = new SubGoal("satiated", 1, false);
        SubGoal sub2 = new SubGoal("hasMoved", 1, false);
        SubGoal sub3 = new SubGoal("foundFlower", 1, false);

        goals.Add(sub1, 3);
        goals.Add(sub2, 1);
        goals.Add(sub3, 5);
    }
}
