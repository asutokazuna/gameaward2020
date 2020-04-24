using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Raytest : MonoBehaviour
{
    public Vector3 line;
    [SerializeField] private List<GameObject> _stageObject;
    SceneMgr _sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneMgr>();
    }

    // Update is called once per frame
    void Update()
    {
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
            for (int i = 0; i < _stageObject.Count; i++)
            {
                if (hit.collider.name == _stageObject[i].name)
                {
                    if (Input.GetKey(KeyCode.Return))
                    {
                        _sceneManager.SetScene((E_SCENE)i + 3);
                    }
                }
            }
           // Debug.Log(hit.collider.gameObject.name);
        }
    }
}
