/**
 * @file BlockDestoroy.cs
 * @brief ぶっ壊れた箱の処理
 * @author Kondo katsutoshi
 * 
 * @date 2020/04/25 パリン
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroy : MonoBehaviour
{
    /**
     * @brief 生成直後に破壊処理を呼び出す
     * @return なし
     */
    void Start()
    {
        destroyObject();
    }

    /**
     * @brief なにも
     */
    void Update()
    {

    }

    /**
     * @brief 破片を飛び散らせるしょり
     * @return なし
     * @details 子の情報を取得し力を加えて吹っ飛ばす
     */
    public void destroyObject()
{
        var _random = new System.Random();
        var _min = -3;
        var _max = 3;
        foreach (Rigidbody r in gameObject.GetComponentsInChildren<Rigidbody>())
            {
            r.isKinematic = false;
            r.transform.SetParent(null);
            var vect = new Vector3(_random.Next(_min, _max), _random.Next(0, _max), _random.Next(_min, _max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        };
        Destroy(gameObject);
    }
}
