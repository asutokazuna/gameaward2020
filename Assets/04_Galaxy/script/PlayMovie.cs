/**
 * @file   PlayMovie.cs
 * @brief   動画再生用
 * @author  柳井魁星
 * @date    2020/05/18(火)  作成
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
	void Start()
	{
		_rawImage = GameObject.Find("PlayVideo").GetComponent<RawImage>();
	}

 	public void SetVideo(int _stageNo)
	{
		_stageNo -= 1;
		var videoPlayer = screen.GetComponent<VideoPlayer>();   // videoPlayerコンポーネントの追加
		SetAlpha0();
		videoPlayer.source = VideoSource.VideoClip; // 動画ソースの設定
		videoPlayer.clip = _videoClip[_stageNo];
		videoPlayer.isLooping = true;   // ループの設定
		videoPlayer.playOnAwake = false;
		VPControl();
		SetAlpha1();
	}

	void VPControl()
	{
		var videoPlayer = GetComponent<VideoPlayer>();

		if (!videoPlayer.isPlaying) // ボタンを押した時の処理
			videoPlayer.Play(); // 動画を再生する。
	}

	public void SetAlpha0()//フェードアウト
	{
		var videoPlayer = screen.GetComponent<VideoPlayer>();   // videoPlayerコンポーネントの追加
		if (videoPlayer.isPlaying) // ボタンを押した時の処理
			videoPlayer.Stop(); // 動画を停止する。
		_rawImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void SetAlpha1()//フェードイン
	{
		_rawImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}
}
