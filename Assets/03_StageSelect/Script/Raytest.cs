using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raytest : MonoBehaviour
{
    public Vector3 line;
    // Start is called before the first frame update
    void Start()
    {
        
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
           // Debug.Log(hit.collider.gameObject.name);
        }
    }
}
