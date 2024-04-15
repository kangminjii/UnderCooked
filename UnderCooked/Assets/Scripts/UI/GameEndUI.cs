using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameEndUI : MonoBehaviour
{
    GameObject _endImage;
    string _endScene = "[3]Ending";


    public delegate void TimeOver();
    public event TimeOver GameEnd;


    void Start()
    {
        _endImage = Managers.UI.FindDeepChild(transform, "Image").gameObject;

        GameTimerUI.GameEnd += AppearEndingObject;
    }


    void OnDestroy()
    {
        GameTimerUI.GameEnd -= AppearEndingObject;
    }


    void AppearEndingObject()
    {
        _endImage.SetActive(true);
        Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();
        StartCoroutine(LoadNextScene());
    }


   IEnumerator LoadNextScene()
   {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
   }


}
