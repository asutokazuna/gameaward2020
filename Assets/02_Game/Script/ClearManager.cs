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

    private GameObject[] ParticleLeft = new GameObject[3];
   
    private GameObject[] ParticleRight = new GameObject[3];
   
    private GameObject[] ParticleTop = new GameObject[3];
   
    [SerializeField]
    private Material[] _color = new Material[3];
    [SerializeField]
    private float _rateMultiplier;
    [SerializeField]
    private float _size;


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

        ParticleLeft[0] = GameObject.Find("ParticleLeft");
        ParticleRight[0] = GameObject.Find("ParticleRight");
        ParticleTop[0] = GameObject.Find("ParticleTop");

        ParticleLeft[1] = Instantiate(ParticleLeft[0], ParticleLeft[0].transform.position, ParticleLeft[0].transform.rotation);
        ParticleLeft[2] = Instantiate(ParticleLeft[0], ParticleLeft[0].transform.position, ParticleLeft[0].transform.rotation);

        ParticleLeft[1].transform.parent = ParticleLeft[0].transform;
        ParticleLeft[2].transform.parent = ParticleLeft[0].transform;

        ParticleRight[1] = Instantiate(ParticleRight[0], ParticleRight[0].transform.position, ParticleRight[0].transform.rotation);
        ParticleRight[2] = Instantiate(ParticleRight[0], ParticleRight[0].transform.position, ParticleRight[0].transform.rotation);

        ParticleRight[1].transform.parent = ParticleLeft[0].transform;
        ParticleRight[2].transform.parent = ParticleLeft[0].transform;

        ParticleTop[1] = Instantiate(ParticleTop[0], ParticleTop[0].transform.position, ParticleTop[0].transform.rotation);
        ParticleTop[2] = Instantiate(ParticleTop[0], ParticleTop[0].transform.position, ParticleTop[0].transform.rotation);

        ParticleTop[1].transform.parent = ParticleLeft[0].transform;
        ParticleTop[2].transform.parent = ParticleLeft[0].transform;


        ParticleLeft[0].GetComponent<ParticleSystemRenderer>().material = _color[0];
        ParticleLeft[1].GetComponent<ParticleSystemRenderer>().material = _color[1];
        ParticleLeft[2].GetComponent<ParticleSystemRenderer>().material = _color[2];

        ParticleRight[0].GetComponent<ParticleSystemRenderer>().material = _color[0];
        ParticleRight[1].GetComponent<ParticleSystemRenderer>().material = _color[1];
        ParticleRight[2].GetComponent<ParticleSystemRenderer>().material = _color[2];

        ParticleTop[0].GetComponent<ParticleSystemRenderer>().material = _color[0];
        ParticleTop[1].GetComponent<ParticleSystemRenderer>().material = _color[1];
        ParticleTop[2].GetComponent<ParticleSystemRenderer>().material = _color[2];

        
        ParticleSystem.MainModule _main;
        ParticleSystem.EmissionModule _emission;
        for (int i = 0;i < 3;i++)
        {
            ParticleLeft[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ParticleLeft[i].GetComponent<ParticleSystem>().Clear(true);

            _main = ParticleLeft[i].GetComponent<ParticleSystem>().main;
            //_emission = ParticleLeft[i].GetComponent<ParticleSystem>().emission;
            _main.startSize = _size;
           // _emission.rateOverTimeMultiplier = _rateMultiplier;

            ParticleRight[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ParticleRight[i].GetComponent<ParticleSystem>().Clear(true);

            _main = ParticleRight[i].GetComponent<ParticleSystem>().main;
           // _emission = ParticleRight[i].GetComponent<ParticleSystem>().emission;
            _main.startSize = _size;
           // _emission.rateOverTimeMultiplier = _rateMultiplier;

            ParticleTop[i].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ParticleTop[i].GetComponent<ParticleSystem>().Clear(true);

            _main = ParticleTop[i].GetComponent<ParticleSystem>().main;
            _emission = ParticleTop[i].GetComponent<ParticleSystem>().emission;
            _main.startSize = _size;
            _emission.rateOverTimeMultiplier = _rateMultiplier;
        }


        
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
                //if (Input.GetKeyDown(KeyCode.Z) ||
                //    GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
                //{// 次のステージ
                //    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE_MODE.NEXT_STAGE);
                //    _fade = true;
                //}
                //else if (Input.GetKeyDown(KeyCode.C) ||
                //    GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.B))
                //{// ステージ選択
                //    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                //    _fade = true;
                //}
                if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isAnyTrigger())
                {
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

            for (int i = 0; i < 3; i++)
            {
                ParticleLeft[i].GetComponent<ParticleSystem>().Play(true);
                ParticleRight[i].GetComponent<ParticleSystem>().Play(true);
                ParticleTop[i].GetComponent<ParticleSystem>().Play(true);
            }
            
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
