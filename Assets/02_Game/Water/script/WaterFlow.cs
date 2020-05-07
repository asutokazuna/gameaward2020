/*
 * @file	WaterFlow.cs
 * @brief   水の流れの管理
 *
 * @author  Shun Kato
 * @date    2020/04/13      作成
 * @date    2020/04/23      水漏れ関係を追加
 * @date    2020/05/05      満タン時にFullWaterPS再生の追加
 * @version	1.00
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


/*
 * @enum 向き情報
 */
public enum E_DIRECTION
{
    FRONT = 0,            
    RIGHT,            
    BACK,         
    LEFT,         
    UP,     
    DOWN,          
}


public class WaterFlow : MonoBehaviour
{

    /*
     * 方向について
     * xが+方向が1、そこから左回り
     */

    //穴の方向の情報
    [NamedArrayAttribute(new string[] { "FRONT", "RIGHT", "BACK", "LEFT", "UP", "DOWN" })]
    public bool[] direction = new bool[6];

    //水漏れの方向
    [NamedArrayAttribute(new string[] { "FRONT", "RIGHT", "BACK", "LEFT", "UP", "DOWN" })]
    public bool[] WaterLeak = new bool[6];

    //回転補正値
    [SerializeField]
    private int RotateDirection;

    //補正後の方向
    [SerializeField]
    int TargetDirection;

    private GameObject MapCtrl;
    private Map CountBoxScript;

    private GameObject UICtrl;
    private CountBoxUI UIScript;

    private BoxEmission EmissionScript;

    public int  _currentWater;
    public int   _maxWater;
    [SerializeField]
    public bool _isFullWater;
    private bool _isMinusWater;

    [SerializeField]
    private bool _isWaterSource;

    [SerializeField]
    private bool _isFullPS;

    private float _adjust = 0.5f;

    AudioSource _audioSource;
    public AudioClip SEFullWater;
    public AudioClip SEFullWaterLift;

    // Start is called before the first frame update
    void Start()
    {
        _isMinusWater = false;

        RotateDirection = (int)(this.gameObject.transform.localEulerAngles.y / 90.0f + 0.5f);

        for (int i = 0;i < 6;i++)
        {
            WaterLeak[i] = false;
        }

        for (int i = 0; i < 6; i++)
        {
            WaterLeak[i] = false;
        }

        _isFullPS = false;

        MapCtrl = GameObject.Find("Map");
        CountBoxScript = MapCtrl.GetComponent<Map>();

        UICtrl = GameObject.Find("UICanvas");
        UIScript = UICtrl.GetComponent<CountBoxUI>();

        _audioSource = GetComponent<AudioSource>();

        EmissionScript = this.GetComponent<BoxEmission>();

        CreateWaterLeak();
        StopChildParticle();
       // PlayChildFullPS();
    }

    private void FixedUpdate()
    {

        RotateDirection = (int)(this.gameObject.transform.localEulerAngles.y / 90.0f + 0.5f);
        if (!_isWaterSource)
        {
            if (_isFullWater)
            {
                MinusWater();
                //_currentWater--;
            }
            _isMinusWater = false;

            if (_currentWater <= 0)
            {
                _currentWater = 0;
            }

            if (_currentWater > _maxWater)
            {
                _currentWater = _maxWater;
            }

            if (_currentWater >= _maxWater - 10 && _isFullWater.Equals(false))
            {// 溜まったら
                _isFullWater = true;
                _isFullPS = true;
                _currentWater = _maxWater;
                CountBoxScript._fullWaterBlockCnt++; //全体の水箱+
                UIScript.AddFullBox(+1);

                EmissionScript.StartEmission(); //光らせる

                PlayChildParticle();
                CreateFullPS();

                _audioSource.PlayOneShot(SEFullWater);
                //PlayChildFullPS();
            }
            else if (_currentWater < _maxWater - 10 && _isFullWater.Equals(true))
            {
                _isFullWater = false;
                _isFullPS = false;
                _currentWater = 0;
                CountBoxScript._fullWaterBlockCnt--; //全体の水箱-
                UIScript.AddFullBox(-1);

                _audioSource.PlayOneShot(SEFullWaterLift);
                StopChildParticle();
            }


            
        }
        else
        {
            _currentWater = _maxWater;
            _isFullWater = true;
        }

        if (_isFullWater)
        {
            for (int i = 0; i < 6; i++)
            {
                if (direction[i])
                {
                    WaterLeak[i] = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                WaterLeak[i] = false;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

        RotateDirection = (int)(this.gameObject.transform.localEulerAngles.y / 90.0f + 0.5f);





    }


    public void MinusWater()
    {
        if(_isMinusWater)
        {
            return;
        }
        else
        {
            _isMinusWater = true;
            _currentWater--;

            if (_currentWater <= 0)
            {
                _currentWater = 0;
            }
        }

       
    }

    private void CreateHuta()
    {
        
    }
    private void CreateWaterLeak()
    {
        GameObject LeakSide = (GameObject)Resources.Load("WaterLeak_side");
        GameObject LeakSource = (GameObject)Resources.Load("WaterLeak_Source");
        GameObject LeakTop = (GameObject)Resources.Load("WaterLeak_top");

        GameObject huta = (GameObject)Resources.Load("suigen_huta");

        //Vector3 pos = this.transform.position;
        //pos.y -= 0.2f;
        //Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));


        for (int i = 0;i < 5;i++)
        {
            switch (i)
            {
                case 0:
                    if(direction[(i + RotateDirection) % 4])
                    {
                        Vector3 pos = this.transform.position;
                        pos.x += 0.5f;
                        pos.y -= 0.1f;

                        GameObject Obj;
                        if(_isWaterSource)
                        {
                            Obj = Instantiate(LeakSource, pos, Quaternion.Euler(0, 90, 0));
                        }
                        else
                        {
                            Obj = Instantiate(LeakSide, pos, Quaternion.Euler(0, 90, 0));
                        }
                        
                        Obj.transform.parent = this.transform;
                        Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        if(this.tag == "WaterSourceBlock")
                        {
                            GameObject Obj;
                            Obj = Instantiate(huta, this.transform.position, Quaternion.Euler(0, 270, 0));
                            Obj.transform.parent = this.transform;
                            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }
                    
                    break;

                case 1:
                    if (direction[(i + RotateDirection) % 4])
                    {
                        Vector3 pos = this.transform.position;
                        pos.z += 0.5f;
                        pos.y -= 0.1f;

                        GameObject Obj;
                        if (_isWaterSource)
                        {
                            Obj = Instantiate(LeakSource, pos, Quaternion.Euler(0, 0, 0));
                        }
                        else
                        {
                            Obj = Instantiate(LeakSide, pos, Quaternion.Euler(0, 0, 0));
                        }
                        
                        Obj.transform.parent = this.transform;
                        Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        if (this.tag == "WaterSourceBlock")
                        {
                            GameObject Obj;
                            Obj = Instantiate(huta, this.transform.position, Quaternion.Euler(0, 180, 0));
                            Obj.transform.parent = this.transform;
                            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }

                    break;

                case 2:
                    if (direction[(i + RotateDirection) % 4])
                    {
                        Vector3 pos = this.transform.position;
                        pos.x += -0.5f;
                        pos.y -= 0.1f;

                        GameObject Obj;
                        if (_isWaterSource)
                        {
                            Obj = Instantiate(LeakSource, pos, Quaternion.Euler(0, 270, 0));
                        }
                        else
                        {
                            Obj = Instantiate(LeakSide, pos, Quaternion.Euler(0, 270, 0));
                        }
                        
                        Obj.transform.parent = this.transform;
                        Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        if (this.tag == "WaterSourceBlock")
                        {
                            GameObject Obj;
                            Obj = Instantiate(huta, this.transform.position, Quaternion.Euler(0, 90, 0));
                            Obj.transform.parent = this.transform;
                            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }

                    break;

                case 3:
                    if (direction[(i + RotateDirection) % 4])
                    {
                        Vector3 pos = this.transform.position;
                        pos.z += -0.5f;
                        pos.y -= 0.1f;

                        GameObject Obj;
                        if (_isWaterSource)
                        {
                            Obj = Instantiate(LeakSource, pos, Quaternion.Euler(0, 180, 0));
                        }
                        else
                        {
                            Obj = Instantiate(LeakSide, pos, Quaternion.Euler(0, 180, 0));
                        }
                        
                        Obj.transform.parent = this.transform;
                        Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        if (this.tag == "WaterSourceBlock")
                        {
                            GameObject Obj;
                            Obj = Instantiate(huta, this.transform.position, Quaternion.Euler(0, 0, 0));
                            Obj.transform.parent = this.transform;
                            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }

                    break;

                case 4:
                    if (direction[i])
                    {
                        Vector3 pos = this.transform.position;
                        pos.y += 0.6f;

                        GameObject Obj;
                        Obj = Instantiate(LeakTop, pos, Quaternion.Euler(-90, 00, 0));
                        Obj.transform.parent = this.transform;
                        Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        if (this.tag == "WaterSourceBlock")
                        {
                            GameObject Obj;
                            Obj = Instantiate(huta, this.transform.position, Quaternion.Euler(90, 0, 0));
                            Obj.transform.parent = this.transform;
                            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }

                    break;
            }
        }
      

    }

    private void PlayChildParticle()
    {
        var childTransforms = this.transform.GetComponentsInChildren<Transform>()
          .Where(t => t.tag == "WaterLeak");

        foreach (var item in childTransforms)
        {
            item.GetComponent<ParticleSystem>().Play(true);
            Debug.Log("play");
        }
    }

    private void StopChildParticle()
    {
        Debug.Log("in");
        foreach (Transform childTransform in this.transform)
        {
            if (childTransform.tag == "WaterLeak")
            {
                childTransform.GetComponent<ParticleSystem>().Play(true);
                Debug.Log("stop  1");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //相手のタグが箱かつ、自分自身が満水かつ、持たれていなかったら
        if(other.gameObject.tag == "WaterBlock" && _isFullWater && 
            this.GetComponent<BlockTank>()._lifted == E_HANDS_ACTION.NONE)
        {
            
            for (int i = 0; i < 4; i++)
            {
                
                if (direction[i])
                {
                    TargetDirection = (i + RotateDirection) % 4;
                   
                    switch (TargetDirection)
                    {
                        case 0:
                            if(other.transform.position.x >= this.transform.position.x + 0.9f &&
                               other.transform.position.x <= this.transform.position.x + 1.1f &&
                               other.transform.position.y >= this.transform.position.y - _adjust &&
                               other.transform.position.y <= this.transform.position.y + _adjust &&
                               other.transform.position.z >= this.transform.position.z - _adjust &&
                               other.transform.position.z <= this.transform.position.z + _adjust)
                            {
                                
                               // other.GetComponent<WaterFlow>()._currentWater++;
                                if (other.GetComponent<WaterFlow>().direction[(TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4])
                                {
                                    //繋がる
                                    WaterLeak[TargetDirection] = false;
                                    if(_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                                    {
                                        other.GetComponent<WaterFlow>()._currentWater++;
                                       
                                        MinusWater();

                                        WaterLeak[i] = false;
                                    }
                                    
                                }
                                else
                                {
                                    //繋がらない
                                }
                            }
                            break;

                        case 1:
                            if (other.transform.position.z >= this.transform.position.z + 0.9f &&
                               other.transform.position.z <= this.transform.position.z + 1.1f &&
                               other.transform.position.y >= this.transform.position.y - _adjust &&
                               other.transform.position.y <= this.transform.position.y + _adjust &&
                               other.transform.position.x >= this.transform.position.x - _adjust &&
                               other.transform.position.x <= this.transform.position.x + _adjust)
                            {
                                if (other.GetComponent<WaterFlow>().direction[(TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4])
                                {
                                    //繋がる
                                    WaterLeak[TargetDirection] = false;
                                    if (_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                                    {
                                        other.GetComponent<WaterFlow>()._currentWater++;
                                        
                                        MinusWater();

                                        WaterLeak[i] = false;
                                    }
                                }
                                else
                                {
                                    //繋がらない
                                }
                            }
                            break;

                        case 2:

                            // Debug.Log(other.transform.position.x);

                            if (other.transform.position.x <= this.transform.position.x - 0.9f &&
                               other.transform.position.x >= this.transform.position.x - 1.1f &&
                               other.transform.position.y >= this.transform.position.y - _adjust &&
                               other.transform.position.y <= this.transform.position.y + _adjust &&
                               other.transform.position.z >= this.transform.position.z - _adjust &&
                               other.transform.position.z <= this.transform.position.z + _adjust)
                            {
                                //Debug.Log("in");
                                //int debug = (TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4;
                                //Debug.Log(debug);
                                if (other.GetComponent<WaterFlow>().direction[(TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4])
                                {
                                    //繋がる
                                    WaterLeak[TargetDirection] = false;
                                    if (_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                                    {
                                        other.GetComponent<WaterFlow>()._currentWater++;
                                       
                                        MinusWater();

                                        WaterLeak[i] = false;
                                    }
                                }
                                else
                                {
                                    //繋がらない
                                }
                            }
                            
                            break;

                        case 3:
                            if (other.transform.position.z <= this.transform.position.z - 0.9f &&
                               other.transform.position.z >= this.transform.position.z - 1.1f &&
                               other.transform.position.y >= this.transform.position.y - _adjust &&
                               other.transform.position.y <= this.transform.position.y + _adjust &&
                               other.transform.position.x >= this.transform.position.x - _adjust &&
                               other.transform.position.x <= this.transform.position.x + _adjust)
                            {
                                if (other.GetComponent<WaterFlow>().direction[(TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4])
                                {
                                    //繋がる
                                    WaterLeak[TargetDirection] = false;
                                    if (_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                                    {
                                        other.GetComponent<WaterFlow>()._currentWater++;
                                       
                                        MinusWater();

                                        WaterLeak[i] = false;
                                    }
                                }
                                else
                                {
                                    //繋がらない 
                                }
                            }
                            break;
                    }
                    
                }
            }

            if (direction[4])
            {
                if (other.transform.position.z >= this.transform.position.z - _adjust &&
                    other.transform.position.z <= this.transform.position.z + _adjust &&
                    other.transform.position.y >= this.transform.position.y + 0.9f &&
                    other.transform.position.y <= this.transform.position.y + 1.1f &&
                    other.transform.position.x >= this.transform.position.x - _adjust &&
                    other.transform.position.x <= this.transform.position.x + _adjust)
                {
                    if (other.GetComponent<WaterFlow>().direction[5])
                    {
                        //繋がる
                        WaterLeak[TargetDirection] = false;
                        if (_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                        {
                            other.GetComponent<WaterFlow>()._currentWater++;
                           
                            MinusWater();
                        }

                    }
                    else
                    {
                        //繋がらない
                    }
                }

            }

            if (direction[5])
            {
                if (other.transform.position.z >= this.transform.position.z - _adjust &&
                    other.transform.position.z <= this.transform.position.z + _adjust &&
                    other.transform.position.y <= this.transform.position.y - 0.9f &&
                    other.transform.position.y >= this.transform.position.y - 1.1f &&
                    other.transform.position.x >= this.transform.position.x - _adjust &&
                    other.transform.position.x <= this.transform.position.x + _adjust)
                {
                    if (other.GetComponent<WaterFlow>().direction[4])
                    {
                        //繋がる
                        WaterLeak[TargetDirection] = false;
                        if (_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                        {
                            other.GetComponent<WaterFlow>()._currentWater++;
                           
                            MinusWater();
                        }

                    }
                    else
                    {
                        //繋がらない
                    }
                }

            }

        }
        //if (other.gameObject.tag == "WaterSourceBlock")
        //{
        //    _currentWater++;
        //    add++;
        //}
    }

    private void CreateFullPS()
    {
        if (this.tag == "WaterBlock")
        {
            GameObject FullPS = (GameObject)Resources.Load("FullWaterPS");

            GameObject Obj;
            Obj = Instantiate(FullPS, this.transform.position, Quaternion.Euler(0, 0, 0));
            Obj.transform.parent = this.transform;
            Obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void PlayChildFullPS()
    {
        if (_isFullPS)
        {
            var childTransforms = this.transform.GetComponentsInChildren<Transform>()
              .Where(t => t.tag == "FullWaterParticle");
            foreach (var item in childTransforms)
            {
                item.GetComponent<ParticleSystem>().Play(true);
                _isFullPS = false;
            }
            Debug.Log("play  FullPs");
        }
        else
        {
            var childTransforms = this.transform.GetComponentsInChildren<Transform>()
          .Where(t => t.tag == "FullWaterParticle");
            foreach (var item in childTransforms)
            {
                item.GetComponent<ParticleSystem>().Play(false);
            }
            Debug.Log("stop  FullPs");

        }
    }
}
