using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> _stageObject = default;
    CameraMove _cameraMove;
    SceneMgr _sceneManager;

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
        
        RayTest();
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
            if(target.collider.tag != "mask")
            {
                pointer.transform.position = target.point;  //当たった場合
            }
            else
            {
                pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f); //マスクとあたった場合
            }
            
           // Debug.Log(Vector3.Distance(target.point, transform.position));
        }
        else
        {
            pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f); //何も当たらなかった場合
        }
            

        

        //foreach (RaycastHit hit in Physics.RaycastAll(_ray))
        //{
        //    //Debug.Log(hit.collider);
        //   // Debug.Log(hit.collider.transform.parent.parent.name);
        //    Debug.Log(hit.collider.name);
        //    for (int i = 0; i < _stageObject.Count; i++)
        //    {
        //        if (!hit.collider.transform.parent)
        //        {// 取り合えずこいつでエラー文が一個減る
        //            continue;
        //        }
        //        Debug.Log(hit.collider.transform.parent.parent.GetComponent<revolution>().PlanetID + " = ID");
        //        if (hit.collider.transform.parent.parent.GetComponent<revolution>().PlanetID == _cameraMove._currentID)
        //        {
        //            //if (hit.collider.name == _stageObject[i].name)
        //            //{
        //            //        Debug.Log("Enter");
        //            //    if (Input.GetKey(KeyCode.Return))
        //            //    {
        //            //        _sceneManager.SetScene((E_SCENE)i + 3);
        //            //    }
        //            //}

        //            //ベータ版用プログラム↓
        //            if (hit.collider.transform.parent.parent.name == _stageObject[i].name)
        //            {
        //               // Debug.Log("Enter");
        //                if (Input.GetKey(KeyCode.Return))
        //                {
        //                    //_sceneManager.SetScene((E_SCENE)i + 2);
        //                }
        //            }
        //        }
        //    }
        //    // Debug.Log(hit.collider.gameObject.name);
        //}
    }
}
