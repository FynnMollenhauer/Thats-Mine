using UnityEngine;
using System.Collections;

public enum GameScreen
{
    None,
    Menu,
    Loading,
    Gameplay,
}

public abstract class ScreenBase : MonoBehaviour
{
    bool isFirstTimeInitialized = false;

    public virtual void OnActive()
    {
        if (!isFirstTimeInitialized)
        {
            isFirstTimeInitialized = true;
            InitFirstTime();
        }
    }

    public virtual void OnDeactive() { }

    protected virtual void InitFirstTime() { }

    public virtual void OnGoBack()
    {
        ScreenManager.Instance.GoBack();
    }
}
