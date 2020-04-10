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
        Ray ray = new Ray(transform.position, line);

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの飛ばせる距離
        int distance = 25;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        if (Physics.Raycast(ray, out hit, distance))
        {
            //Rayが当たったオブジェクトのtagがBoxだったら
            //if (hit.collider.tag == "Box")
           //     Debug.Log("RayがBoxに当たった");

           // if (hit.collider.name == "1")
             //   Debug.Log("Rayが1に当たった");
             //boxコライダー参照なので若干サイズがないとダメ。また、なぜか判定がずれてる
            if (hit.collider.name == "1-1")
                Debug.Log("Rayが1-1に当たった");




        }
    }
}
