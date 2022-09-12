using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldState 
{
    public string key;
    public int value;
}


public class WorldStates
{
    public Dictionary<string, int> states;

    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool hasState(string key)
    {
        return states.ContainsKey(key);
    }

    public void addState(string key, int value)
    {
        states.Add(key, value);
    }

    public void modifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
            {
                removeState(key);
            }
        }
        else
        {
            states.Add(key, value);
        }
        Debug.Log(string.Format("Set {0}  {1}  to world states", key, states[key]));
    }

    public void removeState(string key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    public void setState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else 
        {
            states.Add(key, value);
        }
    }

    public Dictionary<string, int> getStates()
    {
        return states;
    }
}           
