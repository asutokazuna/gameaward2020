/**
 * @file BlockDestoroy.cs
 * @brief ゲームオーバー時の箱の処理
 * @author Kondo katsutoshi
 * 
 * @date 2020/04/25 パリン
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockDestroy : MonoBehaviour
{

    void Start()
    {

    }

    /**
     * @brief 縮小
     */
    void Update()
    {
        if (this.gameObject.transform.localScale.y >= 0)
        {
            this.gameObject.transform.localScale -= Vector3.one;
        }
    }

    /**
     * @brief 破片を飛び散らせるしょり
     * @param[in] ハコの位置
     * @return なし
     * @details 箱がエリア内なら破片を生成し力を加えて吹っ飛ばす
     */
    public void destroyObject(Vector3 _pos)
    {
        if (_pos.y > 0)
        {
            var _random = new System.Random();
            var _min = -3;
            var _max = 3;

            this.gameObject.SetActive(false);       //割れた箱を非表示に
            GameObject _Debris = Instantiate((GameObject)Resources.Load("Debris"), _pos, Quaternion.identity);       //箱の破片を生成

            foreach (Rigidbody r in _Debris.GetComponentsInChildren<Rigidbody>())
            {
                r.isKinematic = false;
                r.transform.SetParent(null);
                var vect = new Vector3(_random.Next(_min, _max), _random.Next(0, _max), _random.Next(_min, _max));
                r.AddForce(vect, ForceMode.Impulse);
                r.AddTorque(vect, ForceMode.Impulse);
            };
            Destroy(_Debris);
        }
    }
}
