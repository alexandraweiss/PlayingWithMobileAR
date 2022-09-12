using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = allStates;
        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = allStates;

        foreach (KeyValuePair<string, int> beliefState in beliefStates)
        {
            if (!this.state.ContainsKey(beliefState.Key))
            {
                this.state.Add(beliefState.Key, beliefState.Value);
            }
        }
        
        this.action = action;
    }
}


public class GPlanner 
{

    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (a.isAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.getWorld().getStates(), beliefStates.getStates(), null);
        bool success = buildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            //Debug.LogError("[GPlanner] Could not build graph.");
            return null;
        }

        Node cheapest = null;
        
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if(leaf.cost < cheapest.cost) 
            {
                cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The plan is:");
        foreach (GAction a in queue)
        {
            Debug.Log(string.Format("Q:  {0}", a.actionName));    
        }


        return queue;
    }

    private bool buildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach (GAction action in usableActions)
        {
            if (action.isAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.effects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);
                if (goalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else 
                {
                    List<GAction> subset = actionSubset(usableActions, action);
                    bool found = buildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    private bool goalAchieved(Dictionary<string, int> goals, Dictionary<string, int> currentState)
    {
        foreach (KeyValuePair<string, int> goal in goals)
        {
            if (!currentState.ContainsKey(goal.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> actionSubset(List<GAction> usableActions, GAction actionToRemove)
    {
        List<GAction> result = new List<GAction>();
        foreach (GAction action in usableActions)
        {
            if (!action.Equals(actionToRemove))
            {
                result.Add(action);
            }
        }

        return result;
    }
}
