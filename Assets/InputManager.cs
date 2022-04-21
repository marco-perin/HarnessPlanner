using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Dictionary<KeyCode, EventList> actions;

    protected override void PostAwake()
    {
        if (IsThisInstance())
        {
            actions = new Dictionary<KeyCode, EventList>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsNotPressingAnyKey()) return;

        if (!Input.GetMouseButtonDown(0))
            if (actions[KeyCode.None]?.actions.Count > 0)
            {
                ExecuteKeyCodeActions(KeyCode.None);
            }

        foreach (KeyCode keyCode in actions.Keys.Where(kc => kc != KeyCode.None))
        {
            if (Input.GetKeyDown(keyCode))
            {
                ExecuteKeyCodeActions(keyCode);
            }
        }

    }

    private void ExecuteKeyCodeActions(KeyCode keyCode)
    {
        Debug.Assert(actions.ContainsKey(keyCode));

        foreach (Action action in actions[keyCode].actions)
        {
            Debug.Assert(action != null);
            action();
        }
    }

    private static bool IsNotPressingAnyKey()
    {
        return !Input.anyKeyDown;
    }

    /// <summary>
    /// Add an action to the onKeyDown of "key" event.
    /// Add an action to the KeyCode.None to be called at the start of the program
    /// </summary>
    /// <param name="key">The KeyCode for which to add the action</param>
    /// <param name="action"></param>
    /// <param name="exclusive"></param>
    /// <exception cref="ArgumentException">If the exclusivity is violated</exception>
    public void AddAction(KeyCode key, Action action, bool exclusive = false)
    {
        // Prevent the None key to be exclusive
        Debug.Assert(!(key == KeyCode.None && exclusive == true), "You cannot add to the Keycode.None key an exlusive event");

        if (!actions.ContainsKey(key))
        {
            actions[key] = new EventList(exclusive);
        }
        else if (exclusive || actions[key].exclusive)
            throw new ArgumentException(
                "Exclusive event was trying to be added, " +
                "but another one with the same key was already present!");

        actions[key].actions.Add(action);
    }

    public class EventList
    {
        public List<Action> actions;
        public bool exclusive;

        public EventList(bool exclusive = false) : this(new List<Action>(), exclusive) { }

        public EventList(Action action, bool exclusive = false)
            : this(new List<Action> { action }, exclusive)
        { }

        public EventList(List<Action> actions, bool exclusive = false)
        {
            this.actions = actions;
            this.exclusive = exclusive;
        }

    }
}
