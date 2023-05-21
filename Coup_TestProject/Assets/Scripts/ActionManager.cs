using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;

    public List<Action> actions;

    private void Awake()
    {
        instance = this;
    }

    public void ChooseAction(Action action, string performer, string target)
    {
        action.PerformAction(performer, target);
    }

    public Action GetActionByName(string name)
    {
        foreach(Action a in actions)
        {
            if (a.actionName == name)
                return a;
        }
        return null;
    }
}
