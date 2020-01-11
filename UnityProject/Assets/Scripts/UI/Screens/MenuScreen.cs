using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : ScreenBase
{
    public override void OnActive()
    {
        base.OnActive();
    }

    public void OnClick_Play(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void OnClick_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();        
#endif
    }
}
