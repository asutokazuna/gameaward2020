using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    public bool ClearSwitch;
    public float WaitTime;
    public float DelayTime;

    private Map FlgScript;
    private Clear ClearScript;
    private bool _isFirst;
    private float _timer;
    private float _changeDelay;

    private GameObject ParticleLeft;
    private GameObject ParticleRight;
    private GameObject ParticleTop;

    private GameObject[] _player;
    private bool _fade;


    // Start is called before the first frame update
    void Start()
    {
        FlgScript = GameObject.Find("Map").GetComponent<Map>();
        ClearScript = GameObject.Find("Clear").GetComponent<Clear>();
        _player = GameObject.FindGameObjectsWithTag("Player");
        _isFirst = true;
        _timer = WaitTime;
        _changeDelay = DelayTime;
        _fade = false;

        ParticleLeft = GameObject.Find("ParticleLeft");
        ParticleRight = GameObject.Find("ParticleRight");
        ParticleTop = GameObject.Find("ParticleTop");

        ParticleLeft.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleLeft.GetComponent<ParticleSystem>().Clear(true);

        ParticleRight.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleRight.GetComponent<ParticleSystem>().Clear(true);

        ParticleTop.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        ParticleTop.GetComponent<ParticleSystem>().Clear(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(ClearSwitch && FlgScript._gameClear)
        {
            if(_timer <= 0.0f)
            {
                SetClear();


                foreach (GameObject obj in _player)
                {
                    obj.GetComponent<Player>().GameClear();
                }
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        if (ClearScript._finishClear)
        {
            if ((_changeDelay <= 0.0f || Input.anyKey) && !_fade)
            {
                //if (isGameClear())
                //{// クリアやで
                //    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE.CLEAR);
                //}
                //シーン遷移
                //else
                if (Input.GetKeyDown(KeyCode.Z) ||
                    GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
                {// 次のステージ
                    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE_MODE.NEXT_STAGE);
                    _fade = true;
                }
                else if (Input.GetKeyDown(KeyCode.C) ||
                    GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.B))
                {// ステージ選択
                    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                    _fade = true;
                }
                //GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene();
            }
            else
            {
                _changeDelay -= Time.deltaTime;
            }
        }

    }

    private void SetClear()
    {
        if (_isFirst && !FlgScript._gameOver)
        {
            ClearScript.StartClear();
            _isFirst = false;

            ParticleLeft.GetComponent<ParticleSystem>().Play(true);
            ParticleRight.GetComponent<ParticleSystem>().Play(true);
            ParticleTop.GetComponent<ParticleSystem>().Play(true);

            // ここでクリアしたよを呼ぶ
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetClear();
            //_stageClear[(int)GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().NowScene] = true;
        }
    }


    //public bool isGameClear()
    //{// 取り合えずベータ版クリア
    //    for (int n = (int)E_SCENE._1_1; n < (int)E_SCENE.CLEAR; n++)
    //    {
    //        if (!_stageClear[n])
    //        {// まだ未クリアがあるよ
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}
