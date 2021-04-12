using System.Linq;
using Core.Attributes;
using Core.Resource;
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

	private ResourceService _resourceService;

	[Inject]
	public PlayerServiceExample(ResourceService resourceService)
	{
		_resourceService = resourceService;
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
		var names = _resourceService.TestRepository.Data.Values.ToList();
		var random = Random.Range(0, names.Count);
		return names[random].name;
	}

}




