using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PopupManager : MonoBehaviour
{
    [SerializeField] Camera popupCam;
    [SerializeField] Camera hudCam;

    private List<PopupBase> listCurrentPopup = new List<PopupBase>();

    static PopupManager instance = null;

    public static PopupManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PopupManager>();

                if (instance != null && instance.popupCam == null)
                {
                    instance.popupCam = instance.GetComponentInParent<Camera>();
                }
            }
            return instance;
        }
    }

    public void Add(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        listCurrentPopup.Add(obj.GetComponent<PopupBase>());
    }

    public bool IsPopupMenuShowing()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }

    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        listCurrentPopup = new List<PopupBase>();
    }

    void Update()
    {
        HideCameraHUDWhenOpenPopup();
    }

    public bool hideHUDWhileShowingPopup = true;

    void HideCameraHUDWhenOpenPopup()
    {
        if (!hideHUDWhileShowingPopup)
            return;

        if (hudCam == null)
            return;

        hudCam.enabled = listCurrentPopup.Count <= 0;
    }

    public void RemovePopup(PopupBase popup)
    {
        if (listCurrentPopup.Contains(popup))
        {
            listCurrentPopup.Remove(popup);
        }
    }

    public void RemoveAllPopup()
    {
        if (listCurrentPopup == null || listCurrentPopup.Count == 0)
            return;

        listCurrentPopup.Clear();
    }

    public bool IsOnTop(PopupBase popup)
    {
        if (listCurrentPopup.Count == 0)
            return false;

        return (listCurrentPopup[listCurrentPopup.Count - 1] == popup);
    }
}
