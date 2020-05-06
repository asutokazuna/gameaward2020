using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySystem : MonoBehaviour
{
   // [SerializeField] private List<GameObject> _stageObject = default;
    CameraMove _cameraMove;
    SceneMgr _sceneManager;
    private E_SCENE _stageID = 0;//ID置き場

    public float dist;
    public GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneMgr>();
        _cameraMove = GameObject.Find("CameraObj").GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_cameraMove._isOrbital)
        {
            RayTest();
        }
        else
        {
            SetID(0);
        }
    }
    void RayTest()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向    ↓位置をずらす
        Vector3 _center = new Vector3(Screen.width / 2, Screen.height / 2 - 100);
        //Rayが当たったオブジェクトの情報を入れる箱   
        Ray _ray = Camera.main.ScreenPointToRay(_center);

        //Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(_center);



        RaycastHit target;
        if (Physics.Raycast(_ray, out target))
        {
            if (target.collider.tag != "mask")
            {
                pointer.transform.position = target.point;  //当たった場合

                if(target.collider.gameObject.GetComponent<StageID>())
                {
                    Debug.Log(target.collider.gameObject.GetComponent<StageID>()._stageID);
                    SetID(target.collider.gameObject.GetComponent<StageID>()._stageID);
                    if (Input.GetKey(KeyCode.Return))
                    {
                        
                        //_sceneManager.SetScene(E_SCENE._1_1);
                        _sceneManager.SetScene(GetID());
                    }
                }

               
            }
            else
            {
                pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f); //マスクとあたった場合
                SetID(0);
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
}
