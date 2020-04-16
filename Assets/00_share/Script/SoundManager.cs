//=======================================
// SoundManager
// 担当　浅原絵理華
//
// 仕様　BGMとSEの音量を変更するときは
// スクリプトの22行目の音量調整から行ってください
// SEのループのON・OFFは64行目から行ってください
//=======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    private const string BGM_PATH = "00_share/Audio/BGM";
    private const string SE_PATH = "00_share/Audio/SE";
    private const string SOUND_OBJECT_NAME = "SoundManager";
    private const int BGM_SOURCE_NUM = 1;
    private const int SE_SOURCE_NUM = 5;
    private const float FADE_OUT_SECONDO = 0.5f;

    private const float BGM_VOLUME = 0.5f;      //音量調整
    private const float SE_VOLUME = 0.3f;       //音量調整

    private bool isFadeOut = false;
    private float fadeDeltaTime = 0f;
    private int nextSESourceNum = 0;
    private BGMLabel currentBGM = BGMLabel.None;
    private BGMLabel nextBGM = BGMLabel.None;

    //BGMは一つづつ、SEは複数同時に鳴る
    private AudioSource bgmSource;
    private List<AudioSource> seSourceList;
    private Dictionary<string, AudioClip> seClipDic;
    private Dictionary<string, AudioClip> bgmClipDic;
    private static SoundManager singletonInstance = null;

    public static SoundManager SingletonInstance
    {
        get
        {
            if (!singletonInstance)
            {
                GameObject obj = new GameObject(SOUND_OBJECT_NAME);
                singletonInstance = obj.AddComponent<SoundManager>();
                DontDestroyOnLoad(obj);
            }
            return singletonInstance;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < SE_SOURCE_NUM + BGM_SOURCE_NUM; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }
        //                                                                                                                     ↓ BGMのループON・OFF
        IEnumerable<AudioSource> audioSources = GetComponents<AudioSource>().Select(a => { a.playOnAwake = false; a.volume = BGM_VOLUME; a.loop = true; return a; });
        bgmSource = audioSources.First();
        seSourceList = audioSources.Skip(BGM_SOURCE_NUM).ToList();
        //                                             　　↓SEのループON・OFF    
        seSourceList.ForEach(a => { a.volume = SE_VOLUME; a.loop = false; });
        //false
        bgmClipDic = (Resources.LoadAll(BGM_PATH) as Object[]).ToDictionary(bgm => bgm.name, bgm => (AudioClip)bgm);
        seClipDic = (Resources.LoadAll(SE_PATH) as Object[]).ToDictionary(se => se.name, se => (AudioClip)se);
    }

    // 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    public void PlaySE(SELabel seLabel, float delay = 0.0f) => StartCoroutine(DelayPlaySE(seLabel, delay));

    private IEnumerator DelayPlaySE(SELabel seLabel, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource se = seSourceList[nextSESourceNum];
        se.PlayOneShot(seClipDic[seLabel.ToString()]);
        nextSESourceNum = (++nextSESourceNum < SE_SOURCE_NUM) ? nextSESourceNum : 0;
    }

    // 指定したBGMを流す。すでに流れている場合はNextに予約し、流れているBGMをフェードアウトさせる
    public void PlayBGM(BGMLabel bgmLabel)
    {
        if (!bgmSource.isPlaying)
        {
            currentBGM = bgmLabel;
            nextBGM = BGMLabel.None;
            if (bgmClipDic.ContainsKey(bgmLabel.ToString()))
            {
                bgmSource.clip = bgmClipDic[bgmLabel.ToString()];
            }
            else
            {
                Debug.LogError($"bgmClipDicに{bgmLabel.ToString()}というKeyはありません");
            }
            bgmSource.Play();
        }
        else if (currentBGM != bgmLabel)
        {
            isFadeOut = true;
            nextBGM = bgmLabel;
            fadeDeltaTime = 0f;
        }
    }

    // BGMを止める
    public void StopSound()
    {
        bgmSource.Stop();
        seSourceList.ForEach(a => { a.Stop(); });
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            fadeDeltaTime += Time.deltaTime;
            bgmSource.volume = (1.0f - fadeDeltaTime / FADE_OUT_SECONDO) * BGM_VOLUME;

            if (fadeDeltaTime >= FADE_OUT_SECONDO)
            {
                isFadeOut = false;
                bgmSource.Stop();
            }
        }
        else if (nextBGM != BGMLabel.None)
        {
            bgmSource.volume = BGM_VOLUME;
            PlayBGM(nextBGM);
        }
    }

    public enum BGMLabel
    {
        None,
        Home,
        Game,
        Library
    }

    public enum SELabel
    {
        Start,
        PlayGame,
        TapButton,
        Spell,
        Win,
        Lose,
        ItemGet
    }

}
