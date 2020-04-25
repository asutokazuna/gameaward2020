using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> _stageObject;
    CameraMove _cameraMove;
    SceneMgr _sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_cameraMove._currentID);
        RayTest();
    }
    void RayTest()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Vector3 _center = new Vector3(Screen.width / 2, Screen.height / 2);
        //Rayが当たったオブジェクトの情報を入れる箱   
        Ray _ray = Camera.main.ScreenPointToRay(_center);
        //Debug.Log(center);

        foreach (RaycastHit hit in Physics.RaycastAll(_ray))
        {
            Debug.Log(hit.collider.GetComponent<revolution>().PlanetID);
            for (int i = 0; i < _stageObject.Count; i++)
            {

                if (hit.collider.GetComponent<revolution>().PlanetID == _cameraMove._currentID)
                {
                    if (hit.collider.name == _stageObject[i].name)
                    {
                        if (Input.GetKey(KeyCode.Return))
                        {
                            //Debug.Log("Enter");
                            _sceneManager.SetScene((E_SCENE)i + 3);
                        }
                    }
                }
            }
            // Debug.Log(hit.collider.gameObject.name);
        }
    }
}
