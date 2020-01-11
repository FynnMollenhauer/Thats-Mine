using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GUIManager : MonoBehaviour
{
    public ScreenManager screenManager;
    public PopupManager popupManager;

    public static GUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject prefab = Resources.Load("GUI/GUIManager") as GameObject;
                GameObject instance = Instantiate(prefab);

                GUIManager.instance = instance.GetComponent<GUIManager>();
            }

            return instance;
        }
    }
    private static GUIManager instance;
}
