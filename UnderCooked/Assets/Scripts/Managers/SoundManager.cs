using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public AudioSource[] AudioSources = new AudioSource[(int)Define.Sound.MaxCount];


    public void Init()
    {
        GameObject root = GameObject.Find("Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                AudioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            AudioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }


    public void Clear()
    {
        foreach(AudioSource audioSource in AudioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }


    public void BgmDown()
    {
        float newVolume = 0.2f;

        AudioSource bgmAudioSource = AudioSources[(int)Define.Sound.Bgm];
        bgmAudioSource.volume = newVolume;
    }


    public void Play(string path, Define.Sound type = Define.Sound.Effect , float pitch = 1.0f, float volume = 0.5f)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if(type == Define.Sound.Bgm)
        {
            AudioClip audioClip = GetorAddAudioClip(path);
            
            if(audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");
            }
          
            AudioSource audioSource = AudioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioClip audioClip = GetorAddAudioClip(path);
            
            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing ! {path}");
            }

            AudioSource audioSource = AudioSources[(int)Define.Sound.Effect];
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
     

    AudioClip GetorAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);
        }

        return audioClip;
    }

    public AudioSource GetAudio(Define.Sound type)
    {
        AudioSource audioSource = AudioSources[(int)type];

        return audioSource;
    }
}
