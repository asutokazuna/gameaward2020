using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySystem : MonoBehaviour
{
   // [SerializeField] private List<GameObject> _stageObject = default;
    CameraMove _cameraMove;
    SceneMgr _sceneManager;
    private E_SCENE _stageID = 0;   //ID置き場
    private int     _level = 1;     //レベル置き場

    public float dist;
    public GameObject pointer;
    private GameObject Ring_FX = default;

    RaycastHit _newTarget;
    RaycastHit _oldTarget;
    E_SCENE _oldStageID;

    public bool _isSelect = false;

    [SerializeField]
    private float _pointerDelay = 0.0f;
    private float _pointerTimer;
    private bool _isEmit;

    private MenuUI _menuUI;

    private RayTarget _rayTarget = default;
    private GameObject _targetObj = default;
    private GameObject _OldTargetObj = default;

    private RayTarget _oldRayTarget = default;//SE多重再生回避用
    //SE再生用変数
    public AudioClip _SECol;
    public AudioClip _SEStart;
    AudioSource _audioSource;

    bool _firstSound = false;// フェード中にならないよう

    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneMgr>();
        _cameraMove = GameObject.Find("CameraObj").GetComponent<CameraMove>();
        Ring_FX = (GameObject)Resources.Load("Ring_FX");

        _menuUI = GameObject.Find("Menu").GetComponent<MenuUI>();

        _audioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _OldTargetObj = _targetObj;
        _oldStageID = _stageID;
        if (_cameraMove._isOrbital)
        {
            RayTest();
        }
        else
        {
            SetID(0);
        }

        if(_pointerTimer > _pointerDelay)
        {
            if(!_isEmit)
            {
                pointer.GetComponent<ParticleSystem>().Play(false);
                _isEmit = true;
            }
            
            
        }
        else
        {
            _pointerTimer += Time.deltaTime;
        }

        if(_stageID != _oldStageID)
        {
            if(_stageID != 0)
            {
                _targetObj.GetComponent<OutlineOnOff>().OutlineOn();
                _rayTarget.LandMove(true);

                _pointerTimer = 0.0f;
                pointer.GetComponent<ParticleSystem>().Stop();
                pointer.GetComponent<ParticleSystem>().Clear();
                _isEmit = false;

                GameObject _obj = Instantiate(Ring_FX, pointer.transform.position, pointer.transform.rotation);
                _obj.transform.parent = pointer.transform;
            }
            else
            {
                _OldTargetObj.GetComponent<OutlineOnOff>().OutlineOff();
                _rayTarget.LandMove(false);
            }
        }

    }
    void RayTest()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向    ↓位置をずらす
        Vector3 _center = new Vector3(Screen.width / 2, Screen.height / 2 - (Screen.height / 5.0f));
        //Rayが当たったオブジェクトの情報を入れる箱   
        Ray _ray = Camera.main.ScreenPointToRay(_center);

        //Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(_center);

        if (Physics.Raycast(_ray, out _newTarget))
        {
            if (_newTarget.collider.tag != "mask")
            {
                pointer.transform.position = _newTarget.point;  //当たった場合
                pointer.transform.LookAt(transform.position);


                if (_newTarget.collider.gameObject.GetComponent<RayTarget>())
                {
                    _rayTarget = _newTarget.collider.gameObject.GetComponent<RayTarget>();
                    _targetObj = _rayTarget._targetObj;
                    if (_rayTarget != _oldRayTarget)
                    {
                        if (_firstSound != false)
                        {
                            _audioSource.PlayOneShot(_SECol);
                        }
                            _oldRayTarget = _rayTarget;
                            _firstSound = true;
                    }
                }

                if (_targetObj.GetComponent<StageID>())
                {
                    //Debug.Log(_newTarget.collider.gameObject.GetComponent<StageID>()._stageID);
                    SetID(_targetObj.GetComponent<StageID>()._stageID);
                    _level = _targetObj.GetComponent<StageID>()._level; // レベル取得

                    if (!_menuUI._isMenu)
                    {
                        if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A) &&
                            !_isSelect)
                        {
                            //_sceneManager.SetScene(E_SCENE._1_1);
                            _audioSource.PlayOneShot(_SEStart);
                            _sceneManager.SetScene(GetID());
                            _isSelect = true;
                        }
                    }
                }
            }
            else
            {
                pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f); //マスクとあたった場合
                SetID(0);
                _oldRayTarget = null;
            }

            // Debug.Log(Vector3.Distance(target.point, transform.position));
        }
        else
        {
            pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f); //何も当たらなかった場合
        }
    }

    public void SetID(E_SCENE _stageId)
    {
        _stageID = _stageId;
    }

   public E_SCENE GetID()
    {
        return _stageID;
    }

    public int GetLevel()
    {
        return _level;
    }
}
