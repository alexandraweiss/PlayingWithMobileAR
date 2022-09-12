using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "action";

    public float cost = 1f;

    public GameObject target;
    public string targetTag;

    public float duration = 0f;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public WorldStates beliefs;

    public GAction() 
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public virtual void Awake()
    {
        if (preConditions != null)
        {
            foreach (WorldState ws in preConditions)
            {
                preconditions.Add(ws.key, ws.value);
            }
        }
        if (afterEffects != null)
        {
            foreach (WorldState ws in afterEffects)
            {
                effects.Add(ws.key, ws.value);
            }
        }

        beliefs = GetComponent<GAgent>().beliefs;
    }

    public bool isAchievable()
    {
        return true;
    }

    public bool isAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> pair in preconditions)
        {
            if (!conditions.ContainsKey(pair.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool prePerform();

    public abstract bool postPerform();
}