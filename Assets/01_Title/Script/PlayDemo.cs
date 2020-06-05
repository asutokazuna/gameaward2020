using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayDemo : MonoBehaviour
{
    public bool _playMovie = false;
    float _timer = 0;
    VideoPlayer _videoPlayer;   // videoPlayerコンポーネントの取得
    int _waitTime = 3;
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
    }

    void DemoManager()
    {
        _timer += Time.deltaTime;
        Debug.Log(_timer);
        if (_timer > _waitTime)
        {
            if (_playMovie == false)
            {
                _videoPlayer.Play(); // 動画を再生する。
                //フェードイン
                _rawImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                _playMovie = true;
            }
            if (_timer > 4 && !_videoPlayer.isPlaying)//isPlayingがディレイ持たせないと正常に動かないので
            {
                //フェードアウト
                _rawImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                _playMovie = false;
                _waitTime = 10;
                _timer = 0;
            }
        }

    }

}
