using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public delegate void ActionEvent(Action action, string performer, string target);
    public static ActionEvent ActionBegun, ActionSucceeded, ActionFailed;

    public string actionName;

    public string performableBy;
    public List<string> CounteractableBy;
    public bool isUnchallengeable;

    public int minCoinsRequired;
    public bool requiresTarget;

    public string performer { protected set; get; }
    public PlayerController performerController { protected set; get; }

    public string target { protected set; get; }
    public PlayerController targetController { protected set; get; }

    public virtual void PerformAction(string performer, string target)
    {
        ActionBegun?.Invoke(this, performer, target);
        //pay coins

        //start counteraction opportunity
    }

    public virtual void Success()
    {
        //recieve coins or whatever the goal is
        ActionSucceeded?.Invoke(this, performer, target);
        TurnManager.instance.ActionComplete();
        ResetAction();
    }

    public virtual void Failed(bool getRefund)
    {
        //if counteractioned, lose payment
        //if challenged, return payment

        ActionFailed?.Invoke(this, performer, target);
        TurnManager.instance.ActionComplete();
        ResetAction();
    }

    protected void ResetAction()
    {
        performer = "";
        target = "";
        performerController = null;
        targetController = null;
    }
}
