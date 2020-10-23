using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public enum BGM_Type
    {
        nitijou,
    }

    // Update is called once per frame
    public enum SE_Type
    {
        
    }

    public const float CROSS_FADE_TIME = 1.0f;

    
    
    public bool Mute = false;

    public AudioClip[] BGM_Clips;
    public AudioClip[] SE_Clips;

    public AudioMixer audioMixer;

    private AudioSource[] BGM_Sources = new AudioSource[2];
    private AudioSource[] SE_Sources = new AudioSource[16];

    private bool isCrossFanding;

    private int currentBgmIndex = 999;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < SE_Sources.Length; i++)
        {
            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if(!isCrossFanding)
        {
            BGM_Sources[0].volume = GameData.instance.BGM_Volume;
            BGM_Sources[1].volume = GameData.instance.BGM_Volume;
        }

        foreach(AudioSource source in SE_Sources)
        {
            source.volume = GameData.instance.SE_Volume;
        }
    }

    public void PlayBGM(BGM_Type bgmType, bool loopFig = true)
    {
        int index = (int)bgmType;
        currentBgmIndex = index;

        if (index < 0 || BGM_Clips.Length <= index)
        {
            return;
        }
        if(BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
        {
            BGM_Sources[0].loop = loopFig;
            BGM_Sources[0].clip = BGM_Clips[index];
            BGM_Sources[0].Play();
        }
        else
        {
            StartCoroutine(CrossFadeChangeBGM(index, loopFig));
        }
    }

    private IEnumerator CrossFadeChangeBGM(int index, bool loopFig)
    {
        isCrossFanding = true;
        if(BGM_Sources[0].clip != null)
        {
            BGM_Sources[1].volume = 0;
            BGM_Sources[1].clip = BGM_Clips[index];
            BGM_Sources[1].loop = loopFig;
            BGM_Sources[1].Play();
            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            BGM_Sources[1].Stop();
            BGM_Sources[1].clip = null;
        }
        isCrossFanding = false;
    }
    public void StopBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
        BGM_Sources[0].clip = null;
        BGM_Sources[1].clip = null;
    }

    public void PlaySE(SE_Type seType)
    {
        int index = (int)seType;
        if (index < 0 || SE_Clips.Length <= index)
        {
            return;
        }

        foreach (AudioSource source in SE_Sources)
        {
            if(false == source.isPlaying)
            {
                source.clip = SE_Clips[index];
                source.Play();
                return;
            }
        }
    }

    public void StopSE()
    {
        foreach (AudioSource source in SE_Sources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    public void MuteBGM()
    {
        BGM_Sources[0].Stop();
        BGM_Sources[1].Stop();
    }

    public void ResumeBGM()
    {
        BGM_Sources[0].Play();
        BGM_Sources[1].Play();
    }
    public void SetAudioMixerVolume(float vol)
    {
        if (vol == 0)
        {
            audioMixer.SetFloat("volumeSE", -80);
        }
        else
        {
            audioMixer.SetFloat("volumeSE", 0);
        }
    }
}
