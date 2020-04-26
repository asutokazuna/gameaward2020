using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    public int _currentID;
    public float _distanceToPlanet;
    public float _cameraMoveSpd1;
    public float _cameraMoveSpd2;
    public float _cameraRotateSpd;

    private GameObject[] _planetData;
    private GameObject _currentPlanet;
    private Transform _camera;

    [SerializeField]
    private bool _isOrbital;
    [SerializeField]
    private float _cameraSpd;
    private Vector3 _angles;

    public float _rotateSpeed;
    public float _rotateControllerSpeed;
    private float _angleX;
    private float _angleY;

    [SerializeField] private float mouseY;
    private GameObject UI;
    private GameObject StageSelectManager;
    private SceneMgr SceneManager;

    public float _offsetY;

    // Start is called before the first frame update
    void Start()
    {
        StageSelectManager = GameObject.Find("StageSelectManager");
        UI = GameObject.Find("CanvasUI");
        UI.SetActive(false);

        SceneManager = GameObject.Find("SceneManager").GetComponent<SceneMgr>();

        _planetData = GameObject.FindGameObjectsWithTag("Planet");
        _camera = this.transform.GetComponentInChildren<Transform>();
        _isOrbital = false;
        _currentID = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)
            || GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_LEFT)
            )
        {
            _currentID--;
            _isOrbital = false;
            StageSelectManager.GetComponent<UICange>().ChangePlanetName(true);
        }
        if (Input.GetKeyDown(KeyCode.E)
            || GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_RIGHT)
            )
        {
            _currentID++;
            _isOrbital = false;
            StageSelectManager.GetComponent<UICange>().ChangePlanetName(false);
        }

        if(_currentID >= 4)
        {
            _currentID = 1;
        }
        if (_currentID <= 0)
        {
            _currentID = 3;
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
            _agent.destination = _currentPlanet.transform.position;
            _cameraSpd = _cameraRotateSpd;
            UI.SetActive(false);
        }
        else
        {
            UI.SetActive(true);
            _cameraSpd = 1.0f;

            _angleX += Input.GetAxis("Mouse X") * _rotateSpeed;
            _angleX += GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()._RStick._now_H * _rotateControllerSpeed;
            CheckAngle(_angleX);

            _angleY += Input.GetAxis("Mouse Y") * _rotateSpeed;
            _angleY += GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()._RStick._now_V * _rotateControllerSpeed;
            if(_angleY * _rotateSpeed <= 0)
            {
                _angleY = 0;
            }
            if (_angleY  >= 3)
            {
                _angleY = 3;
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

        if((Input.GetKeyDown(KeyCode.Space)
            || GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
            && _isOrbital)   //シーン遷移処理
        {
            switch(_currentID)  
            {
                case 1:     //森

                    //ここにシーン遷移の処理
                    SceneManager.SetScene(E_SCENE._1_1);

                    break;

                case 2:     //火山

                    //ここにシーン遷移の処理
                    SceneManager.SetScene(E_SCENE._1_2);

                    break;

                case 3:     //キノコ

                    //ここにシーン遷移の処理
                    SceneManager.SetScene(E_SCENE._1_3);

                    break;
            }
        }

        //switch
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<revolution>().PlanetID == _currentID)
        {
            _isOrbital = true;
            Vector3 _axis = _camera.position - _currentPlanet.transform.position;
        }
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
}
