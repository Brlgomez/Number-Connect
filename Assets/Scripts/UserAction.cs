using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAction : MonoBehaviour
{
    Stack<Action> actions = new Stack<Action>();

    public void AddAction(Action action)
    {
        if (action != null)
        {
            actions.Push(action);
        }
    }

    public Action PopAction()
    {
        if (actions.Count > 0)
        {
            return actions.Pop();
        }
        return null;
    }

    public bool HaveActions()
    {
        return actions.Count > 0;
    }

    public void ClearActions()
    {
        actions.Clear();
    }

    public class Action
    {
        public bool added;
        public int value;
        public int hinted;
        public Vector2 position;

        public bool removedOther;
        public int otherValue;
        public int otherHinted;
        public Vector2 otherPosition;

        public bool removedOtherOther;
        public int otherOtherValue;
        public int otherOtherHinted;
        public Vector2 otherOtherPosition;
    }
}
