using Core.Alert;
using Core.Audio;
using Core.Utils;
using Core.View;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using Zenject;

public class MainMenuWindowViewExample : TypedView
{
	public Button PlusButton;

	public Text ScoreCellText;
	public Text ScoreStreamText;
	public Text ScoreEmptyStream;

	public Text RandomName;

	public Button ClickClipButton;
	public Button CancelClipButton;
	public Button JazzClipButton;
	public Button PianoButton;

	public AlertView AlertView;
	
	private PlayerServiceExample _playerServiceExample; 
	private ISoundService _soundService;
	private IMusicService _musicService;

	[Inject]
	public void Construct(PlayerServiceExample playerServiceExample, ISoundService soundService, IMusicService musicService)   
	{
		_playerServiceExample = playerServiceExample;
		_soundService = soundService;
		_musicService = musicService;
	}

	public override void Setup(ViewData data)
	{
		base.Setup(data);

		PlusButton.BindClick(() =>
		{
			_playerServiceExample.AddScore();
			RandomName.text = $"{"name_id".GetLocalizedString()} {_playerServiceExample.GetRandomName()}";
			_soundService.Play(AudioPaths.UIGroup.ClickClip);
		}).AddTo(Collector); 


		ClickClipButton.BindClick(() =>
		{
			_soundService.Play(AudioPaths.UIGroup.ClickClip);
		}).AddTo(Collector);

		CancelClipButton.BindClick(() =>
		{
			_soundService.Play(AudioPaths.UIGroup.CancelClip);
		}).AddTo(Collector);

		JazzClipButton.BindClick(() =>
		{
			_musicService.Play(AudioPaths.MusicGroup.JazzAmbientClip);
		}).AddTo(Collector);

		PianoButton.BindClick(() =>
		{
			_musicService.Play(AudioPaths.MusicGroup.PianoAmbientClip);
		}).AddTo(Collector);


		_playerServiceExample.ScoreCell.Subscribe(val => 
		{
			ScoreCellText.text = $"ScoreCell: {val}";
			AlertView.Check(AlertType.PlayerScoreAlertExample);
		}).AddTo(Collector); 

		_playerServiceExample.ScoreStream.Subscribe(val => 
		{
			ScoreStreamText.text = $"ScoreStream: {val}";
		}).AddTo(Collector);
	}

	public override ViewType Type => ViewType.MainMenuWindowViewExample; 
	public override CanvasType CanvasType => CanvasType.Window; 
}