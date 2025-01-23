using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;

    [System.Serializable]
    public class AudioClipInfo
    {
        public AudioSource audioSource;
        public AudioClip audioClip;
        public Vector3 audioPosition;
    }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public static void SetVolume(string parameter, float volume)
        => Instance._AudioMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    public static void SetMute(string parameter, bool isMute)
        => Instance._AudioMixer.SetFloat(parameter, isMute ? -80f : 0f);
    public bool IsMainMusicPlay
    {
        get { return _isPlayMainMusic; }
        set { _isPlayMainMusic = value; }
    }
    #region 변수선언
    [Header("오디오 리스너 자동설정됨 (확인용)")]
    [SerializeField] private AudioListener _AudioListener;
    [Header("재생 중인 오디오 클립 정보 디버그용")]
    [SerializeField] private List<AudioClipInfo> _UsedClip;
    [Header("믹서그룹 설정")]
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private AudioMixerGroup _MusicEffect;
    [SerializeField] private AudioMixerGroup _SoundEffect;
    [Header("메인음악 플레이 플래그")]
    [SerializeField] private bool _isPlayMainMusic = false;
    [SerializeField][Range(0, 1)] float musicVolume = 0.2f;
    [SerializeField][Range(0, 1)] float sFXVolume = 1f;
    [SerializeField] private bool _isMute = false;
    [Header("초기 AudioSource 풀 크기")]
    [SerializeField] private int initialPoolSize = 10;

    private AudioSource MainMusicSource;
    private List<AudioSource> _AudioSource;
    private Queue<AudioSource> _AudioUnusedQueue = new Queue<AudioSource>();
    private Dictionary<string, AudioClip> _loadedAudioClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _loadedMainMusicClips = new Dictionary<string, AudioClip>();
    private float previousMusicVolume;
    private float previousSFXVolume;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioListener가 인스펙터에서 설정되지 않았으면 자동 할당
        if (_AudioListener == null)
        {
            _AudioListener = Camera.main.gameObject.GetComponent<AudioListener>();
            if (_AudioListener == null)
            {
                Debug.LogError("AudioListener가 씬에 없습니다. AudioListener를 추가하세요.");
            }
        }

        InitailizeAudioSource();
        InitailizeMusicSource();
        _UsedClip = new List<AudioClipInfo>();

        LoadAudioClip();
        LoadMainMusicClip();
    }

    private void Update()
    {
        PlayingCheck();
        CheckVolumes();
        if (_isMute) SetMute("MasterVolume", true);
        else SetMute("MasterVolume", false);
    }

    private void PlayMainMusic()
    {
        _isPlayMainMusic = true;
    }

    private void CheckVolumes()
    {
        if (Mathf.Abs(musicVolume - previousMusicVolume) > Mathf.Epsilon)
        {
            SetVolume("MusicVolume", musicVolume);
            MainMusicSource.volume = musicVolume;
            previousMusicVolume = musicVolume;
        }
        if (Mathf.Abs(sFXVolume - previousSFXVolume) > Mathf.Epsilon)
        {
            SetVolume("SFXVolume", sFXVolume);
            previousSFXVolume = sFXVolume;
        }
    }

    private void InitailizeMusicSource()
    {
        MainMusicSource = this.gameObject.AddComponent<AudioSource>();
        MainMusicSource.loop = true;
        MainMusicSource.volume = musicVolume;
        MainMusicSource.mute = false;
        MainMusicSource.playOnAwake = false;
        MainMusicSource.priority = 128;
        MainMusicSource.minDistance = 1f;
        MainMusicSource.maxDistance = 20f;
        MainMusicSource.outputAudioMixerGroup = _MusicEffect;
        _isPlayMainMusic = false;
    }

    private void InitailizeAudioSource()
    {
        _AudioSource = new List<AudioSource>();

        while (_AudioSource.Count < initialPoolSize)
        {
            AudioSource audioSource = AudioSourceAdd();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.mute = false;
            audioSource.volume = sFXVolume;
            audioSource.pitch = 1f;
            audioSource.priority = 120;
            //audioSource.spatialBlend = 1f;          // 3D 사운드 활성화
            // audioSource.dopplerLevel = 1f;         // 도플러 효과 설정
            // audioSource.spread = 360f;             // 소리 확산 각도
            audioSource.minDistance = 1f;          // 최대 음량이 들리는 최소 거리
            audioSource.maxDistance = 20f;         // 소리가 사라지는 최대 거리
                                                   //audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // 감쇠 설정
            audioSource.outputAudioMixerGroup = _SoundEffect; // 믹서 그룹
        }
        _AudioUnusedQueue = new Queue<AudioSource>(_AudioSource);
    }

    //private void UpdateAudioListenerPosition()
    //{
    //    if (_AudioListener != null && Camera.main != null)
    //    {
    //        _AudioListener.transform.position = Camera.main.transform.position;
    //        _AudioListener.transform.rotation = Camera.main.transform.rotation;
    //    }
    //}

    private void PlayingCheck()
    {
        if (_isPlayMainMusic && !MainMusicSource.isPlaying && _loadedMainMusicClips.Count > 0)
        {
            var clip = _loadedMainMusicClips["Golden Serpant Tavern (LOOP)"];
            MainMusicSource.clip = clip;
            MainMusicSource.volume = musicVolume;
            MainMusicSource.Play();
        }
        else if (!_isPlayMainMusic && MainMusicSource.isPlaying)
        {
            MainMusicSource.Stop();
        }

        for (int i = _UsedClip.Count - 1; i >= 0; i--)
        {
            AudioClipInfo clipInfo = _UsedClip[i];
            if (!clipInfo.audioSource.isPlaying || clipInfo.audioSource.clip == null)
            {
                ReturnSourceToQueue(clipInfo.audioSource);
                _UsedClip.RemoveAt(i);
            }
        }
    }

    private AudioSource GetAvailableSource()
    {
        AudioSource newSource = _AudioUnusedQueue.Count > 0 ? _AudioUnusedQueue.Dequeue() : AudioSourceAdd();
        newSource.enabled = true;
        return newSource;
    }

    private void ReturnSourceToQueue(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.enabled = false;
        _AudioUnusedQueue.Enqueue(source);
    }

    private AudioSource AudioSourceAdd()
    {
        GameObject _obj = transform.GetChild(0).gameObject;
        AudioSource newSource = _obj.AddComponent<AudioSource>();
        newSource.enabled = false;
        _AudioSource.Add(newSource);
        return newSource;
    }

    public void PlayClip(string clipKey, float speed = 1f)
    {
        if (_loadedAudioClips.TryGetValue(clipKey, out var clip))
        {
            AudioClipInfo newUsed = new AudioClipInfo
            {
                audioSource = GetAvailableSource(),
                audioClip = clip,
                audioPosition = Vector3.zero
            };
            newUsed.audioSource.clip = clip;
            newUsed.audioSource.pitch = speed;
            newUsed.audioSource.Play();
            _UsedClip.Add(newUsed);

        }
        else
        {
            Debug.LogWarning($"클립 키 {clipKey}를 찾을 수 없습니다.");
        }
    }

    public void PlayClipAtPoint(string clipKey, Vector3 position, float volume = 1f, bool isLoop = false)
    {
        if (_loadedAudioClips.TryGetValue(clipKey, out var clip))
        {
            AudioSource source = GetAvailableSource();
            source.transform.position = position;
            source.clip = clip;
            source.volume = volume;
            source.loop = isLoop;
            source.Play();

            _UsedClip.Add(new AudioClipInfo
            {
                audioSource = source,
                audioClip = clip,
                audioPosition = position
            });
        }
        else
        {
            Debug.LogWarning($"클립 키 {clipKey}를 찾을 수 없습니다.");
        }
    }

    private void LoadAudioClip()
    {
        Addressables.LoadAssetsAsync<AudioClip>("SFX", clip =>
        {
            if (!_loadedAudioClips.ContainsKey(clip.name))
            {
                _loadedAudioClips[clip.name] = clip;
            }
        }).Completed += handle =>
        {
            //Debug.Log($"{_loadedAudioClips.Count}개의 사운드 클립을 로드했습니다.");
        };
    }

    private void LoadMainMusicClip()
    {
        Addressables.LoadAssetsAsync<AudioClip>("Music", clip =>
        {
            if (!_loadedMainMusicClips.ContainsKey(clip.name))
            {
                _loadedMainMusicClips[clip.name] = clip;
            }
        }).Completed += handle =>
        {
            //Debug.Log($"{_loadedMainMusicClips.Count}개의 메인 음악을 로드했습니다.");
        };
    }
}

