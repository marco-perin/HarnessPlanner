using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    public Dictionary<KeyCode, EventList> actionsKeyDown;
    public Dictionary<KeyCode, EventList> actionsKeyUp;

    protected override void PostAwake()
    {
        if (IsThisInstance())
        {
            actionsKeyDown = new Dictionary<KeyCode, EventList>();
            actionsKeyUp = new Dictionary<KeyCode, EventList>();
        }
    }

    void Update()
    {
        var arr = actionsKeyUp;
        if (!Input.GetMouseButtonUp(0))
            if (arr.ContainsKey(KeyCode.None) && arr[KeyCode.None]?.actions.Count > 0)
            {
                ExecuteKeyCodeForActionsDict(KeyCode.None, arr);
            }

        foreach (KeyCode keyCode in arr.Keys.Where(kc => kc != KeyCode.None))
        {
            if (Input.GetKeyUp(keyCode))
            {
                ExecuteKeyCodeForActionsDict(keyCode, arr);
            }
        }

        if (IsNotPressingAnyButton()) return;

        arr = actionsKeyDown;

        if (!Input.GetMouseButtonDown(0))
            if (arr.ContainsKey(KeyCode.None) && arr[KeyCode.None]?.actions.Count > 0)
            {
                ExecuteKeyCodeForActionsDict(KeyCode.None, arr);
            }

        foreach (KeyCode keyCode in arr.Keys.Where(kc => kc != KeyCode.None))
        {
            if (Input.GetKeyDown(keyCode))
            {
                ExecuteKeyCodeForActionsDict(keyCode, arr);
            }
        }

    }

    private void ExecuteKeyCodeForActionsDict(KeyCode keyCode, Dictionary<KeyCode, EventList> actionsArray)
    {
        Debug.Assert(actionsArray.ContainsKey(keyCode));

        foreach (Action action in actionsArray[keyCode].actions)
        {
            Debug.Assert(action != null);
            if (actionsArray == actionsKeyUp)
                Debug.Log("Executing KeyUp Action for key " + keyCode);
            action?.Invoke();
        }
    }


    private static bool IsNotPressingAnyButton()
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
    public void AddKeyDownAction(KeyCode key, Action action, bool exclusive = false)
    {
        // Prevent the None key to be exclusive
        Debug.Assert(!(key == KeyCode.None && exclusive == true), "You cannot add to the Keycode.None key an exlusive event");
        //Debug.Log($"Adding action to Key {key} as {(exclusive ? "" : "not")} exclusive");
        if (!actionsKeyDown.ContainsKey(key))
        {
            actionsKeyDown[key] = new EventList(exclusive);
        }
        else if (exclusive || actionsKeyDown[key].exclusive)
            throw new ArgumentException(
                "Exclusive event was trying to be added, " +
                "but another one with the same key was already present!");

        actionsKeyDown[key].actions.Add(action);
    }

    public void AddKeyUpAction(KeyCode key, Action action, bool exclusive = false)
    {
        // Prevent the None key to be exclusive
        Debug.Assert(!(key == KeyCode.None && exclusive == true), "You cannot add to the Keycode.None key an exlusive event");

        if (!actionsKeyUp.ContainsKey(key))
        {
            actionsKeyUp[key] = new EventList(exclusive);
        }
        else if (exclusive || actionsKeyUp[key].exclusive)
            throw new ArgumentException(
                "Exclusive event was trying to be added, " +
                "but another one with the same key was already present!");

        actionsKeyUp[key].actions.Add(action);
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
