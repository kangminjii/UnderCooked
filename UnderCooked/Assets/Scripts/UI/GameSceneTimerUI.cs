using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameSceneTimerUI : MonoBehaviour
{
    float       _timeLimit = 100f;
    float       _currentTime;
    bool        _canUpdate = true;
    string      _endScene = "[3]End";
    [SerializeField]
    Image       _timerProgressBar;
    [SerializeField]
    Text        _timeText;
    [SerializeField]
    GameObject  _endImage;
    

    /*
     * ���� �ð� �ʱ�ȭ
     */
    void Awake()
    {
        _currentTime = _timeLimit;
        _timeText.text = FormatTime(_currentTime);
    }


    /*
     * Time�� �带 ���� ���� �ð��� ��ȭ��Ŵ
     * -> ����� AppearEndingObject() �Լ� ȣ��
     * -> FormatTime�� ���� _timeText ��ȭ
     * -> �ð��� ���� Ÿ�̸ӹ��� �̹��� ��ȭ
     */
    void Update()
    {
        if (_canUpdate)
        {
            if (Time.timeScale > 0)
            {
                _currentTime -= Time.deltaTime;
            }

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _canUpdate = false;

                AppearEndingObject();
            }

            _timeText.text = FormatTime(_currentTime);
            _timerProgressBar.fillAmount = _currentTime / _timeLimit;
        }
    }


    /*
     * �ð����� 00:00 ���信 ���� �ٲ��ִ� �Լ�
     */
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    /*
     * ���� ����� ������ GameObject �Լ�
     * -> bgm ���� �� ����Ʈ ȿ���� ���
     * -> ������ �ε�
     */
    void AppearEndingObject()
    {
        _endImage.SetActive(true);
        
        Managers.Sound.Play("AudioClip/TimesUpSting", Define.Sound.Effect);
        Managers.Sound.GetAudio(Define.Sound.Bgm).Stop();
        
        StartCoroutine(LoadNextScene());
    }


    /*
     * 5�� �� ���� �� �ε�
     */
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(_endScene);
    }

}
