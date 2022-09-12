using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld 
{
    private static readonly GWorld instance = new GWorld();

    private static WorldStates world;

    private static List<GameObject> butterflies;
    private static List<GameObject> flowers;

    static GWorld()
    {
        world = new WorldStates();
        butterflies = new List<GameObject>();
        flowers = new List<GameObject>();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates getWorld()
    {
        return world;
    }

    public void addButterfly(GameObject butterfly)
    {
        butterflies.Add(butterfly);
    }

    public void removeButterfly(GameObject butterfly)
    {
        butterflies.Remove(butterfly);
    }

    public void addFlower(GameObject flower)
    {
        flowers.Add(flower);
    }

    public void removeFlower(GameObject flower)
    {
        flowers.Remove(flower);
    }
}
