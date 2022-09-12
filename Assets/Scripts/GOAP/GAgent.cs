using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    public Dictionary<string, int> subgoals;
    public bool remove;
    public SubGoal(string key, int value, bool remove)
    {
        subgoals = new Dictionary<string, int>();
        subgoals.Add(key, value);
        this.remove = remove;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    public GPlanner planner;

    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentSubGoal;

    public WorldStates beliefs = new WorldStates();

    private bool invoked = false;



    protected virtual void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction action in acts)
        {
            actions.Add(action);
        }
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            if (currentAction.target != null)
            {
                float distance = Vector3.Distance(currentAction.target.transform.position, transform.position);
                if (distance < 0.05)
                {
                    if (!invoked)
                    {
                        Debug.Log(string.Format("Current action invoke: {0} , {1}", currentAction.target, currentAction.duration));
                        Invoke("completeAction", currentAction.duration);
                        invoked = true;
                    }
                }
            }
            else 
            {
                if (!invoked)
                {
                    Debug.Log(string.Format("Current action invoke: {0} , {1}", currentAction.target, currentAction.duration));
                    Invoke("completeAction", currentAction.duration);
                    invoked = true;
                }
            }

            return; //TODO 
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();
            //Debug.Log(string.Format("Make new planner {0}", planner.ToString()));

            IOrderedEnumerable<KeyValuePair<SubGoal, int>> sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
            {
                actionQueue = planner.plan(actions, subGoal.Key.subgoals, beliefs);
                if (actionQueue != null)
                {
                    Debug.Log(string.Format("   queue {0}  subgoals {1} ", actionQueue.Count, subGoal.Key.subgoals.Keys.ToList()[0]));
                    currentSubGoal = subGoal.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            Debug.Log(string.Format("Last action done, stopping"));
            if (currentSubGoal.remove)
            {
                goals.Remove(currentSubGoal);
            }
            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            Debug.Log(string.Format("Got new action: {0}   {1}  {2}", currentAction.actionName, currentAction.target, currentAction.duration));
            if (currentAction.prePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                //if (currentAction.target != null)
                //{
                    currentAction.running = true;
                //}
            }
            else
            {
                actionQueue = null;
            }
        }


    }

    private void completeAction()
    {
        Debug.LogWarning(string.Format(">>> call to complete action {0} ", currentAction.actionName));
        currentAction.running = false;
        currentAction.postPerform();
        invoked = false;
    }
}
