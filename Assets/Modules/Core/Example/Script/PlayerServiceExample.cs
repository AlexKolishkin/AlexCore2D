using System.Linq;
using Core.Attributes;
using Core.StaticData;
using UnityEngine;
using UniRx;
using Zenject;
using Core;

public class PlayerServiceExample : IService, IAutoPersistent
{
	[Persistent] 
	private int _score;

	public ReactiveProperty<int> ScoreCell { get; } = new ReactiveProperty<int>(0);

	public Subject<int> ScoreStream { get; } = new Subject<int>();

	private StaticDataService _staticDataService;

	[Inject]
	public PlayerServiceExample(StaticDataService staticDataService)
	{
		_staticDataService = staticDataService;
	}

	public void OnLoaded()
	{
		ScoreCell.Value = _score;
		ScoreStream.OnNext(_score);
	}

	public void AddScore()
	{
		_score++;
		ScoreCell.Value = _score; 
		ScoreStream.OnNext(_score); 
	}

	public string GetRandomName()
	{
		var names = _staticDataService.TestRepository.Data.Values.ToList();
		var random = Random.Range(0, names.Count);
		return names[random].name;
	}

}




