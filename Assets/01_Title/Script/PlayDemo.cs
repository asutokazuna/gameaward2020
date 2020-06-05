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
    int _waitTime = 3;
    VideoPlayer _videoPlayer;   // videoPlayerコンポーネントの取得
    RawImage _rawImage;
    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GameObject.Find("DemoPlayer").GetComponent<VideoPlayer>();
        _rawImage = GameObject.Find("DemoPlayer").GetComponent<RawImage>();
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
                _videoPlayer.Play(); // 動画を再生する。
                //フェードイン
                _rawImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                _playMovie = true;
            }
            if (_timer > _waitTime + 1 && !_videoPlayer.isPlaying)//isPlayingがディレイ持たせないと正常に動かないので
            {
                //フェードアウト
                _rawImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                _waitTime = 10;
                _timer = 0;
            }

            //何かキーが押されたらデモ終了
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>().isAnyTrigger())
            {
                _videoPlayer.Stop();
            }
        }
        else if (_timer > 1) //ディレイ
        {
                _playMovie = false;
        }

    }

}
