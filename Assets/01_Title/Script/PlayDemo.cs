using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayDemo : MonoBehaviour
{
    public bool _playMovie = false;
    bool _changeScene;
    float _timer = 0;
    int _waitTime = 6;
    VideoPlayer _videoPlayer;   // videoPlayerコンポーネントの取得
    Animator _fadeAnim;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GameObject.Find("DemoPlayer").GetComponent<VideoPlayer>();
        _fadeAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DemoManager();
    }

    void DemoManager()
    {
        _changeScene = GameObject.Find("Logo").GetComponent<Title>()._ChangeScene;
        if (!_changeScene)
        {
            _timer += Time.deltaTime;
            //Debug.Log(_timer);
        }
        if (_timer > _waitTime)
        {
            if (_playMovie == false)
            {
                //フェードイン
                _fadeAnim.SetBool("FadeOut", false);
                _fadeAnim.SetBool("FadeIn", true);

                _videoPlayer.Play(); // 動画を再生する。

                _playMovie = true;
            }
            if (_timer > _waitTime + 1 && !_videoPlayer.isPlaying)//isPlayingがディレイ持たせないと正常に動かないので
            {
                //フェードアウト
                _fadeAnim.SetBool("FadeOut", true);
                _fadeAnim.SetBool("FadeIn", false);

                _waitTime = 10;
                _timer = 0;
            }

            // 動画が再生されていたら
            if (_videoPlayer.isPlaying)
            {
                //何かキーが押されたらデモ終了
                if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isAnyTrigger())
                {
                    //フェードアウト
                    _fadeAnim.SetBool("FadeOut", true);
                    _fadeAnim.SetBool("FadeIn", false);
                }
            }
        }
        else if (_timer > 1) //ディレイ
        {
                _playMovie = false;
        }

        // フェードアウト後に動画を停止
        if(_fadeAnim.GetBool("FinishFadeOut"))
        {
            _fadeAnim.SetBool("FinishFadeOut", false);
            _videoPlayer.Stop();
        }

    }

    /**
    * @brief        フェードアウトの終了をセット
    * @return       なし
    * @details      フェードアウトの終了をセットするアニメーションイベント関数です
    */
    void FinishFadeOut()
    {
        _fadeAnim.SetBool("FinishFadeOut", true);
    }

}
