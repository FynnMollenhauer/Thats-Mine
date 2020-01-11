using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;


public class ScreenManager : MonoBehaviour
{
    private static ScreenManager instance = null;

    public static ScreenManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ScreenManager>();

            return instance;
        }
    }

    private Stack<GameScreen> screenStack = new Stack<GameScreen>();

    public void Reset()
    {
        screenDict.Clear();
    }

    public void ClearBackStack()
    {
        screenStack.Clear();
    }

    Dictionary<GameScreen, ScreenBase> screenDict = new Dictionary<GameScreen, ScreenBase>();

    public Transform screenContainer;

    public GameScreen CurrentScreen { get; private set; }

    public GameScreen LastScreen
    {
        get { if (screenStack.Count > 0) return screenStack.Peek(); else return GameScreen.Menu; }
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        // Destroy all 
        ScreenBase[] screens = screenContainer.GetComponentsInChildren<ScreenBase>(true);
        foreach (ScreenBase sc in screens)
        {
            if (sc != null)
                GameObject.Destroy(sc.gameObject);
        }
    }

    public void Clear()
    {
        if (CurrentScreen != GameScreen.Menu)
        {
            SetScreen_Internal(GameScreen.Menu);
        }

        List<GameScreen> listRemove = new List<GameScreen>();
        foreach (var item in screenDict)
        {
            if (item.Key != GameScreen.Menu)
            {
                GameObject.Destroy(item.Value.gameObject);
                listRemove.Add(item.Key);
            }
        }

        foreach (var scrKey in listRemove)
        {
            screenDict.Remove(scrKey);
        }

        screenStack.Clear();
    }

    ScreenBase LoadScreen(GameScreen screenName)
    {
        Object o = Resources.Load("GUI/Screens/" + screenName.ToString());

        if (o == null)
        {
            return null;
        }

        GameObject go = (GameObject)GameObject.Instantiate(o);

        if (go == null)
        {
            return null;
        }

        Vector3 pos = go.transform.localPosition;
        go.transform.SetParent(screenContainer, false);
        go.transform.localPosition = pos;
        go.transform.localScale = Vector3.one;

        ScreenBase obj = go.GetComponent<ScreenBase>();

        if (obj == null)
        {
            return null;
        }

        if (screenDict.ContainsKey(screenName))
        {
            screenDict[screenName] = obj;
        }
        else
        {
            screenDict.Add(screenName, obj);
        }

        go.SetActive(false);

        return obj;
    }

    public ScreenBase GetScreen(GameScreen screen, bool isForceLoad = true)
    {
        if (screenDict.ContainsKey(screen) == false)
        {
            if (isForceLoad)
                LoadScreen(screen);
            else
                return null;
        }

        return screenDict[screen];
    }

    public void GoBack()
    {
        if (screenStack.Count > 0)
        {
            SetScreen_Internal(screenStack.Pop());
        }
        else
        {
            if (CurrentScreen != GameScreen.Menu)
            {
                SetScreen_Internal(GameScreen.Menu);
            }
        }

    }

    public GameScreen GetScreenGoBack()
    {
        GameScreen screenBack = GameScreen.Menu;

        if (screenStack.Count > 0)
        {
            screenBack = screenStack.Peek();
        }

        return screenBack;
    }

    public void SetScreen(GameScreen screen)
    {
        if (CurrentScreen == screen)
        {
            return;
        }

        if (screenStack.Count > 0 && screen == screenStack.Peek())
        {
            GoBack();
            return;
        }

        if (CurrentScreen != GameScreen.None &&
            CurrentScreen != GameScreen.Menu)
        {
            screenStack.Push(CurrentScreen);

        }

        SetScreen_Internal(screen);
    }

    private void SetScreen_Internal(GameScreen screen)
    {
        if (screenDict.ContainsKey(screen) == false)
        {
            LoadScreen(screen);
        }

        ScreenBase curScreen;
        screenDict.TryGetValue(CurrentScreen, out curScreen);

        if (curScreen != null)
        {
            curScreen.OnDeactive();
            curScreen.gameObject.SetActive(false);
        }

        screenDict[screen].gameObject.SetActive(true);
        screenDict[screen].OnActive();

        CurrentScreen = screen;

    }

    public static ScreenBase GetCurrentScreen()
    {
        if (Instance != null)
        {
            return Instance.GetScreen(Instance.CurrentScreen);
        }
        return null;
    }

    public static void PopLastScreen(GameScreen scr)
    {
        if (Instance != null)
        {
            if (Instance.LastScreen == scr)
            {
                Instance.screenStack.Pop();
            }
        }
    }
}
