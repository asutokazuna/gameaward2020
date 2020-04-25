using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    bool _LogoAnimFinish;

    void Start()
    {
        _LogoAnimFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_LogoAnimFinish)
        {
            if (Input.anyKey)
            {
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
            }
        }
    }

    void SetLogoAnimFinish()
    {
        _LogoAnimFinish = true;
    }
}