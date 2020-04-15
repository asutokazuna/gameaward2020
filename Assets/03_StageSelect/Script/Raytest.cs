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
        //Ray ray = new Ray(transform.position, line);
        //Debug.Log(Input.mousePosition) ;
        // Ray ray = Camera.main.ScreenPointToRay(new Vector2(0,0));

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(center);
        Debug.Log(center);
       // Debug.Log(Input.mousePosition);
        //RaycastHit hit;

        //Rayが当たったオブジェクトの情報を入れる箱   
        // RaycastHit hit;

        //Rayの飛ばせる距離
        int distance = 25;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        
        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
            //もしRayにオブジェクトが衝突したら
            //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
         
            /*if (Physics.Raycast(ray, out hit, distance))
        {
            //Rayが当たったオブジェクトのtagがBoxだったら
            //if (hit.collider.tag == "Box")
            //     Debug.Log("RayがBoxに当たった");

            // if (hit.collider.name == "1")
            //   Debug.Log("Rayが1に当たった");
          
                if (hit.collider.name == "pPlane1")
                    Debug.Log("Rayが pPlane1に当たった");
                if (hit.collider.name == "pPlane2")
                    Debug.Log("Rayが pPlane2に当たった");
                if (hit.collider.name == "pPlane3")
                    Debug.Log("Rayが pPlane3に当たった");
                if (hit.collider.name == "pPlane4")
                    Debug.Log("Rayが pPlane4に当たった");
                if (hit.collider.name == "pPlane5")
                    Debug.Log("Rayが pPlane5に当たった");
                if (hit.collider.name == "pPlane6")
                    Debug.Log("Rayが pPlane6に当たった");
                if (hit.collider.name == "pPlane7")
                    Debug.Log("Rayが pPlane7に当たった");
                if (hit.collider.name == "pPlane8")
                    Debug.Log("Rayが pPlane1に当たった");
            if (hit.collider.gameObject.name == "pPlane3")
                Debug.Log("Rayが pPlane3に当たった");
            if (hit.collider.gameObject.name == "2-1")
                Debug.Log("Rayが2-1に当たった");

         

        }*/


    }
}
