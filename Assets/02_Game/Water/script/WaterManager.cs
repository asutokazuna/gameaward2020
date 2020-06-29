/**
 * @file     WaterManager.cs
 * @brief    水の流れる道筋の管理
 * @author   kotakota
 * @date     2020/06/28     作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class WaterManager
 * @brief 水の道筋を管理するクラス
 */
public class WaterManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _tree;          //!< バランスツリーみたいなリスト
    GameObject _waterSource;                 //!< 水源ブロック
    public static WaterManager _instance;    //!< インスタンス    

    /**
     * @brief 一番最初の初期化処理
     * @return なし
     */
    private void Awake()
    {
        // シングルトン
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /**
     * @breif 初期化処理
     * @return なし
     */
    void Start()
    {
        _waterSource = GameObject.FindWithTag("WaterSourceBlock");
        _tree = new List<GameObject>();
    }


    void Update()
    {
        // とりあえずめんどくさいからここ
        MakeBalanceTree();
    }

    /**
     * @brief バランスツリーみたいなリストの構築
     * @return なし
     * @details 箱が繋がっているツリーを作成し、水が流れるかどうかの判定を行う
     */
    public void MakeBalanceTree()
    {
        GameObject _objWork;            // ワーク
        int _count = 0;                 // 無限ループ回避用カウンター

        // 初期化
        for (int i = 0; i < _tree.Count; i++)
        {
            _tree[i].GetComponent<WaterFlow>()._isList = false;        //繋がっているフラグを下す（重複チェック用）
        }
        _objWork = _waterSource;
        _tree.Clear();

        // ツリーの構築
        while (true)
        {
            for (int i = 0; i < _objWork.GetComponent<WaterFlow>()._sideObjectList.Count; i++)
            {
                if (_objWork.GetComponent<WaterFlow>()._sideObjectList[i].tag == "WaterSourceBlock") {
                    continue;
                }

                if (_objWork.GetComponent<WaterFlow>()._sideObjectList[i].GetComponent<WaterFlow>()._isList == false &&
                        _objWork.GetComponent<WaterFlow>()._sideObjectList[i].GetComponent<BlockTank>()._lifted == E_HANDS_ACTION.NONE) // 当たり判定でやっており、離れるのが遅いため
                {
                    _tree.Add(_objWork.GetComponent<WaterFlow>()._sideObjectList[i]);
                    _objWork.GetComponent<WaterFlow>()._sideObjectList[i].GetComponent<WaterFlow>()._isList = true;
                }
            }

            if (_count >= _tree.Count) {
                break;
            }

            _objWork = _tree[_count];
            _count++;
        }
    }
}

// EOF