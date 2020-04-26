/**
 * @file Fade.cs
 * @brief フェード
 * @author K.Tsuboi
 * @date 2020/04/22 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * @class Fade
 * @brief フェードクラス
 */
public class Fade : MonoBehaviour
{
    Material _appliedMat;
    [SerializeField] Material[] _fadeMat;
    [Range(0, 1)] [SerializeField] float _fadeSpeed;


    /**
    * @brief 初期化処理
    * @return なし
    */
    void Start()
    {
        _appliedMat = _fadeMat[0];
        _appliedMat.SetFloat("_Timer", 0);

    }

    /**
    * @brief 表示処理
    * @return なし
    */
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _appliedMat);
    }



    /**
    * @brief フェードアウト開始処理
    * @return なし
    */
    public IEnumerator FadeOut(UnityAction callback)
    {
        for (float i = 0; i <= 1.1f; i += _fadeSpeed)
        {
            _appliedMat.SetFloat("_Timer", Mathf.Clamp(i, 0, 1.0f));
            yield return null;
        }
        callback();
    }

    /**
    * @brief フェードイン開始処理
    * @return なし
    */
    public IEnumerator FadeIn()
    {
        for (float i = 1; i > 0.0f; i -= _fadeSpeed)
        {
            _appliedMat.SetFloat("_Timer", Mathf.Clamp(i, 0, 1.0f));
            yield return null;
        }
    }

    /**
    * @brief フェードタイプ変更
    * @return なし
    */
    public void SetFadeType(int Type)
    {
        _appliedMat = _fadeMat[Type];
    }
}

