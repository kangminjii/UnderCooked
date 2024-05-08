using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameReadyUI : MonoBehaviour
{
    Image _spaceBar;
    GameObject _recipe;
    GameObject _ready;
    GameObject _start;
    

    public static Action OrderStart;
    public static Action CameraAction;


    void Start()
    {
        Managers.Sound.Play("AudioClip/TheNeonCity", Define.Sound.Bgm);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();

        _spaceBar = Define.FindDeepChild(transform, "SpaceBarCount").GetComponent<Image>();
        _recipe = Define.FindDeepChild(transform, "Recipe").gameObject;
        _ready = Define.FindDeepChild(transform, "Ready").gameObject;
        _start = Define.FindDeepChild(transform, "Start").gameObject;

        StartCoroutine(SpaceBarCheck());
        Time.timeScale = 0;

        Cursor.visible = false;
    }
   

    IEnumerator SpaceBarCheck()
    {
        float startTime = Time.realtimeSinceStartup;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startTime = Time.realtimeSinceStartup;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _spaceBar.fillAmount = 0;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                _spaceBar.fillAmount = elapsedTime * 0.4f;
            }


            if (_spaceBar.fillAmount >= 1)
            {
                _recipe.SetActive(false);
                transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                
                Managers.Sound.Play("AudioClip/Tutorial_Pop_Out", Define.Sound.Effect, 1f, 0.2f);
                StartCoroutine(ResumeGame());

                break;
            }

            yield return null;
        }
    }


    IEnumerator ResumeGame()
    {
        CameraAction.Invoke();

        yield return WaitForRealSeconds(2f);

        _ready.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelReady_01", Define.Sound.Effect);

        yield return WaitForRealSeconds(2.5f);
        
        _ready.SetActive(false);
        _start.SetActive(true);
        Managers.Sound.Play("AudioClip/LevelGo", Define.Sound.Effect);
        
        Time.timeScale = 1;

        OrderStart.Invoke();

        yield return new WaitForSeconds(1.0f);

        _start.SetActive(false);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Play();
    }


    IEnumerator WaitForRealSeconds(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }
    }
}
