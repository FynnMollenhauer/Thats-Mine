using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameoverPopup : PopupBase
{
    #region Popup setup
    private static GameoverPopup instance;

    public static GameoverPopup Create(bool win = false, int stars = 0)
    {
        GameObject popupObject = PopupBase.GereralCreate("GameoverPopup");

        instance = popupObject.GetComponent<GameoverPopup>();

        if (win)
        {
            instance.losePanel.SetActive(false);
            instance.winPanel.SetActive(true);
        }
        else
        {
            instance.losePanel.SetActive(true);
            instance.winPanel.SetActive(false);
        }

        return instance;
    }

    public static void DestroyPopup()
    {
        PopupBase.DestroyPopup(instance);
        if (instance != null)
        {
            instance.gameObject.SetActive(false);
            Destroy(instance.gameObject);
        }
        instance = null;
    }
    #endregion

    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject winPanel;

    public void OnClick_Replay()
    {
        DestroyPopup();
        SceneManager.LoadScene("Level1");
    }

    public void OnClick_Back()
    {
        DestroyPopup();
        SceneManager.LoadScene("Main");
    }
}
