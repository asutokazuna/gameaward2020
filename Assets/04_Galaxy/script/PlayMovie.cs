/**
 * @file   PlayMovie.cs
 * @brief   動画再生用
 * @author  柳井魁星
 * @date    2020/05/18(火)  作成
 * @date    2020/05/19(火)  周りに合わせてアルファ値が変わるように変更　(伊藤)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayMovie : MonoBehaviour
{
	[SerializeField] List<VideoClip> _videoClip;//リスト
	public GameObject screen;
	RawImage _rawImage;
    Image _alphaReference;
    Color _oldColor;
    Color _setColor;

	void Start()
	{
		_rawImage = GameObject.Find("PlayVideo").GetComponent<RawImage>();
        _alphaReference = GameObject.Find("Stage").GetComponent<Image>();
        _oldColor = _alphaReference.color;

    }

    void Update()
    {
        _setColor = _alphaReference.color;

        if (_oldColor != _setColor)
        {
            SetAlpha();
            _oldColor = _setColor;
        }
    }

    public void SetVideo(int _stageNo)
	{
        // 例外処理
        if(_stageNo > _videoClip.Count - 1)
        {
            _stageNo = _videoClip.Count - 1;
        }
        else if(_videoClip == null)
        {
            return;
        }

		var videoPlayer = screen.GetComponent<VideoPlayer>();   // videoPlayerコンポーネントの追加		
		videoPlayer.source = VideoSource.VideoClip; // 動画ソースの設定
		videoPlayer.clip = _videoClip[_stageNo];
		videoPlayer.isLooping = true;   // ループの設定
		videoPlayer.playOnAwake = false;
		VPControl();
	}

	void VPControl()
	{
		var videoPlayer = GetComponent<VideoPlayer>();

		if (!videoPlayer.isPlaying) // ボタンを押した時の処理
			videoPlayer.Play(); // 動画を再生する。
	}

	public void SetAlpha()//フェードアウト
	{
		var videoPlayer = screen.GetComponent<VideoPlayer>();   // videoPlayerコンポーネントの追加
        if (_setColor.a < 0.01f)
        {
            if (videoPlayer.isPlaying) // ボタンを押した時の処理
                videoPlayer.Stop(); // 動画を停止する。
        }
		_rawImage.color = new Color(1.0f, 1.0f, 1.0f, _setColor.a);
	}
}
