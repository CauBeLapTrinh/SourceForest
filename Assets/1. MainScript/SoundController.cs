using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}
[System.Serializable]
public class SoundEffectList
{
    public string name;
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume = 1f;
}

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Header("BgMusic")]
    public AudioSource audioBgMusic;
    [Range(0f, 1f)] public float bgVolume = 1f;
    public AudioClip[] backgroundClip;
    [Header("Sound Effect")]
    public AudioSource audioEffect;
    public SoundEffect[] soundEffects;
    public SoundEffectList[] soundEffectList;

    void Start()
    {
        PlayRandomBackgroundMusic();
    }
    void OnDestroy()
    {
        instance = null;
    }

    // Phát nhạc nền ngẫu nhiên khi bắt đầu
    public void PlayRandomBackgroundMusic()
    {
        if (backgroundClip.Length == 0 || audioBgMusic == null) return;
        int idx = Random.Range(0, backgroundClip.Length);
        audioBgMusic.clip = backgroundClip[idx];
        audioBgMusic.loop = true;
        audioBgMusic.volume = bgVolume;
        audioBgMusic.Play();
    }

    // Phát hiệu ứng âm thanh theo tên
    public void PlayOneShotByName(string soundName)
    {
        foreach (var sfx in soundEffects)
        {
            if (sfx.name == soundName && sfx.clip != null)
            {
                audioEffect.PlayOneShot(sfx.clip, sfx.volume);
                return;
            }
        }
        Debug.LogWarning($"SoundEffect '{soundName}' not found!");
    }
    public void PlayOneShotListByName(string soundName)
    {
        foreach (var sfx in soundEffectList)
        {
            if (sfx.name == soundName && sfx != null)
            {
                if (sfx.clips.Length == 0) return;
                int idx = Random.Range(0, sfx.clips.Length);
                audioEffect.PlayOneShot(sfx.clips[idx], sfx.volume);
                return;
            }
        }
        Debug.LogWarning($"SoundEffect '{soundName}' not found!");
    }

    // Tắt nhạc nền
    public void StopAudioBgMusic()
    {
        if (audioBgMusic != null) audioBgMusic.Stop();
    }
}