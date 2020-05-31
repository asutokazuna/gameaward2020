using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = default;
    public int _currentID;
    public float _distanceToPlanet;
    public float _cameraMoveSpd1;
    public float _cameraMoveSpd2;
    public float _cameraRotateSpd;

    private GameObject[] _planetData;
    private GameObject _currentPlanet;
    private Transform _camera;

  
    public bool _isOrbital;
    [SerializeField]
    private float _cameraSpd;
    private Vector3 _angles;

    public float _rotateSpeed;
    public float _rotateControllerSpeed;
    [SerializeField]
    private float _angleX;
    [SerializeField]
    private float _angleY;

    [SerializeField] private float mouseY;
    private GameObject UI;
    private GameObject StageSelectManager;
    //private SceneMgr SceneManager;

    public float _offsetY = 0.0f;

    private RaySystem _rayScript = default;

    [SerializeField]
    private GameObject _particleUI = default;

    private SceneMgr _sceneManager = default;

    [SerializeField]
    private Vector2[] _cameraPos = new Vector2[(int)E_SCENE.MAX];

    private UICange _uiChange;

    // Start is called before the first frame update
    void Start()
    {
        StageSelectManager = GameObject.Find("StageSelectManager");
        UI = GameObject.Find("CanvasUI");
        _uiChange = StageSelectManager.GetComponent<UICange>();

        //SceneManager = GameObject.Find("SceneManager").GetComponent<SceneMgr>();

        _planetData = GameObject.FindGameObjectsWithTag("Planet");
        _camera = this.transform.GetComponentInChildren<Transform>();
        _isOrbital = true;
        _currentID = 1;

        _rayScript = this.gameObject.GetComponent<RaySystem>();

        _particleUI = GameObject.Find("Concentrate");
        if(_particleUI)
        {
            _particleUI.GetComponent<ParticleSystem>().Stop();
        }

        _sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>();

        if(_sceneManager._lastScene == E_SCENE.TITLE)
        {
            _angleX = _cameraPos[(int)E_SCENE._1_1].x;
            _angleY = _cameraPos[(int)E_SCENE._1_1].y;
           // Debug.Log("title_true");
        }
        else
        {
            _angleX = _cameraPos[(int)_sceneManager._lastScene].x;
            _angleY = _cameraPos[(int)_sceneManager._lastScene].y;
           // Debug.Log("title_false");
        }

        if(_sceneManager._lastScene < E_SCENE._2_1)
        {
            _currentID = 1;
        }
        else if (_sceneManager._lastScene < E_SCENE._3_1)
        {
            _currentID = 2;
        }
        else if (_sceneManager._lastScene < E_SCENE._4_1)
        {
            _currentID = 3;
        }
        else if (_sceneManager._lastScene < E_SCENE._5_1)
        {
            _currentID = 4;
        }
        else if (_sceneManager._lastScene < E_SCENE._6_1)
        {
            _currentID = 5;
        }
        else
        {
            _currentID = 6;
        }

        for (int i = _planetData.Length - 1; i >= 0; i--)
        {
            if (_planetData[i].GetComponent<revolution>().PlanetID == _currentID)
            {
                _currentPlanet = _planetData[i];
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_rayScript._isSelect && _isOrbital)
        {
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().
                isInput(E_INPUT_MODE.TRIGGER, E_INPUT.LB))
            {
                _currentID--;
                _isOrbital = false;
                _uiChange.ChangePlanetName(true);
            }
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().
                isInput(E_INPUT_MODE.TRIGGER, E_INPUT.RB))
            {
                _currentID++;
                _isOrbital = false;
               _uiChange.ChangePlanetName(false);
            }
        }
           

        if(_currentID >= 7)
        {
            _currentID = 1;
        }
        if (_currentID <= 0)
        {
            _currentID = 6;
        }       

        for (int i = _planetData.Length - 1;i >= 0;i--)
        {
            if(_planetData[i].GetComponent<revolution>().PlanetID == _currentID)
            {
                _currentPlanet = _planetData[i];
                break;
            }
        }
        
        if (!_isOrbital)
        {
            if (_particleUI)
            {
                _particleUI.GetComponent<ParticleSystem>().Play(); //Debug.Log("play");
            }
            
            _agent.destination = _currentPlanet.transform.position;
            _cameraSpd = _cameraRotateSpd;
            //UI.SetActive(false);
            _uiChange.PlanetNameOff();
            
        }
        else
        {
            if (_particleUI)
            {
                _particleUI.GetComponent<ParticleSystem>().Stop();
                _particleUI.GetComponent<ParticleSystem>().Clear();
            }

            //UI.SetActive(true);
            _uiChange.PlanerNameOn();
            _cameraSpd = 1.0f;

            if(!_rayScript._isSelect)
            {
                _angleX += Input.GetAxis("Mouse X") * _rotateSpeed;
                _angleX += GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()._LStick._now_H * _rotateControllerSpeed;
                CheckAngle(_angleX);

                _angleY += Input.GetAxis("Mouse Y") * _rotateSpeed;
                _angleY += GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()._LStick._now_V * _rotateControllerSpeed;
                if (_angleY * _rotateSpeed <= 0)
                {
                    _angleY = 0;
                }
                if (_angleY >= 3)
                {
                    _angleY = 3;
                }
            }

           

            mouseY = _angleY;

            Vector3 _pos;
            _pos.x = _distanceToPlanet * Mathf.Sin(_angleX) + _currentPlanet.transform.position.x;
            _pos.y = _distanceToPlanet * Mathf.Cos(_angleY) + _currentPlanet.transform.position.y;
            _pos.z = _distanceToPlanet * Mathf.Cos(_angleX) + _currentPlanet.transform.position.z;


            transform.position = _pos;

            
        }

        Vector3 _relativePos = _currentPlanet.transform.position - _camera.position;
        Quaternion _rotation = Quaternion.LookRotation(_relativePos);
        _camera.rotation = Quaternion.Slerp(_camera.rotation, _rotation, _cameraSpd);

        if(_isOrbital)
        {
            Vector3 pos = _camera.position;
            pos.y += _offsetY;

            _camera.position = pos;

        }

        ////テスト用
        //if ((Input.GetKeyDown(KeyCode.Space)
        //    || GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
        //    && _isOrbital)   //シーン遷移処理
        //{
        //    SceneManager.LoadScene("Stage01");

        //    //switch (_currentID)
        //    //{
        //    //    case 1:     //森

        //    //        //ここにシーン遷移の処理
        //    //        SceneManager.SetScene(E_SCENE._1_1);

        //    //        break;

        //    //    case 2:     //火山

        //    //        //ここにシーン遷移の処理
        //    //        SceneManager.SetScene(E_SCENE._1_2);

        //    //        break;

        //    //    case 3:     //キノコ

        //    //        //ここにシーン遷移の処理
        //    //        SceneManager.SetScene(E_SCENE._1_3);

        //    //        break;
        //    //}
        //}

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;
        if(other.GetComponent<revolution>() && !_isOrbital)
        {
            if (other.GetComponent<revolution>().PlanetID == _currentID)
            {
                _isOrbital = true;
                //Vector3 _axis = _camera.position - _currentPlanet.transform.position;

                _angleX = -0.5f;
                _angleY = 1.3f;
            }
        }
        if (other.CompareTag("FadeColl") && !_isOrbital)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Fade>().SetFadeSpeed(0.2f);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Fade>().StartFadeOut(OnFinishedCoroutine);
        }
    }

    public void OnFinishedCoroutine()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Fade>().SetFadeSpeed(0.05f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Fade>().StartFadeIn();
    }


    private float CheckAngle(float angle)
    {
        if(angle < 360.0f)
        {
            angle += 360.0f;
        }
        if (angle > 360.0f)
        {
            angle -= 360.0f;
        }

        return angle;
    }

    //private Vector2 GetStartPos(E_SCENE _oldScene)
    //{
    //    Vector2 _pos;

    //    _pos = _
    //}
}
