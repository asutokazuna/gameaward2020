using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    bool _LogoAnimFinish;
    bool _demoPlayState;

    void Start()
    {
        _LogoAnimFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_LogoAnimFinish)
        {
            _demoPlayState = GameObject.Find("DemoPlayer").GetComponent<PlayDemo>()._playMovie;
            Debug.Log(_demoPlayState);
            if (!_demoPlayState)
            {
                if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isAnyTrigger())
                {
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