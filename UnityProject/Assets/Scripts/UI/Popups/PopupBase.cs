using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public abstract class PopupBase : MonoBehaviour
{
    public bool ResetOnBegin = true;

    protected void Awake()
    {
        if (ResetOnBegin)
        {
        }
    }

    public static void DestroyPopup(PopupBase popup)
    {
        if (PopupManager.Instance)
            PopupManager.Instance.RemovePopup(popup);
    }

    public static GameObject GereralCreate(string popupStrName)
    {
        GameObject prefab = Resources.Load("GUI/Popups/" + popupStrName) as GameObject;
        GameObject go = Instantiate(prefab) ;

        if (go == null)
            return null;

        PopupManager.Instance.Add(go);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchoredPosition = prefab.GetComponent<RectTransform>().anchoredPosition;

        return go;
    }
}
