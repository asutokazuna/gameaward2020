/*
 * @file	WaterFlow.cs
 * @brief   水の流れの管理
 *
 * @author  Shun Kato
 * @date    2020/04/13      作成
 * @version	1.00
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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

    public int add;
    public int minus;
    public int test;

    private float _adjust = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _isMinusWater = false;

        RotateDirection = (int)(this.gameObject.transform.localEulerAngles.y / 90.0f + 0.5f);

        for (int i = 0;i < 6;i++)
        {
            WaterLeak[i] = false;
        }

        add = 0;
        minus = 0;


        MapCtrl = GameObject.Find("Map");
        CountBoxScript = MapCtrl.GetComponent<Map>();

        UICtrl = GameObject.Find("UICanvas");
        UIScript = UICtrl.GetComponent<CountBoxUI>();

        EmissionScript = this.GetComponent<BoxEmission>();
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
                minus++;
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
                _currentWater = _maxWater;
                CountBoxScript._fullWaterBlockCnt++; //全体の水箱+
                UIScript.AddFullBox(+1);
                EmissionScript.BoxEmissionOn();

                add = 0;
                minus = 0;
            }
            else if (_currentWater < _maxWater - 10 && _isFullWater.Equals(true))
            {
                _isFullWater = false;
                _currentWater = 0;
                CountBoxScript._fullWaterBlockCnt--; //全体の水箱-
                UIScript.AddFullBox(-1);
            }


            if (_isFullWater)
            {
                for (int i = 0; i < 6; i++)
                {
                    WaterLeak[i] = true;
                }
                WaterLeak[5] = false;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    WaterLeak[i] = false;
                }
            }
        }
        else
        {
            _currentWater = _maxWater;
            _isFullWater = true;
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

    private void OnTriggerStay(Collider other)
    {
        //相手のタグが箱かつ、自分自身が満水だったら
        if(other.gameObject.tag == "WaterBlock" && _isFullWater)
        {
            
            for (int i = 0; i < 4; i++)
            {
                
                if (direction[i])
                {
                    TargetDirection = (i + RotateDirection) % 4;
                    if(gameObject.tag == "WaterBlock")
                    {
                        //Debug.Log(TargetDirection);
                    }

                    
                    
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
                                test = TargetDirection;
                               // other.GetComponent<WaterFlow>()._currentWater++;
                                if (other.GetComponent<WaterFlow>().direction[(TargetDirection + 2 + other.GetComponent<WaterFlow>().RotateDirection) % 4])
                                    


                                {
                                    //繋がる
                                    WaterLeak[TargetDirection] = false;
                                    if(_currentWater > other.GetComponent<WaterFlow>()._currentWater)
                                    {
                                        other.GetComponent<WaterFlow>()._currentWater++;
                                        other.GetComponent<WaterFlow>().add++;
                                        MinusWater();
                                       // Debug.Log("0");
                                    }
                                    
                                }
                                else
                                {
                                    //繋がらない
                                }
                            }
                            break;

                        case 1:
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
                                        other.GetComponent<WaterFlow>().add++;
                                        MinusWater();
                                       // Debug.Log("1");
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
                                        other.GetComponent<WaterFlow>().add++;
                                        MinusWater();
                                       // Debug.Log("2");
                                    }
                                }
                                else
                                {
                                    //繋がらない
                                }
                            }
                            
                            break;

                        case 3:
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
                                        other.GetComponent<WaterFlow>().add++;
                                        MinusWater();
                                       // Debug.Log("3");
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
                            other.GetComponent<WaterFlow>().add++;
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
                    other.transform.position.y >= this.transform.position.y - 0.9f &&
                    other.transform.position.y <= this.transform.position.y - 1.1f &&
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
                            other.GetComponent<WaterFlow>().add++;
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
}
