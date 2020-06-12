using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    bool _LogoAnimFinish;
    bool _demoPlayState;
    public bool _ChangeScene = false;
    public AudioClip _SEStart;
    AudioSource _audioSource;
    void Start()
    {
        _LogoAnimFinish = false;
        _audioSource = GameObject.Find("Soundmanage").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_LogoAnimFinish)
        {
            _demoPlayState = GameObject.Find("DemoPlayer").GetComponent<PlayDemo>()._playMovie;
            if (!_demoPlayState)
            {
                if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isAnyTrigger())
                {
                    _ChangeScene = true;
                    _audioSource.PlayOneShot(_SEStart);
                    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                }
            }
        }
    }

    void SetLogoAnimFinish()
    {
        _LogoAnimFinish = true;
    }
}