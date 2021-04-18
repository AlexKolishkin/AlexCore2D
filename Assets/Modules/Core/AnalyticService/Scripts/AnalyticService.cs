using Core.Attributes;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Core.Analytic
{
	public interface IAnalyticService : IService 
	{
		void LogTotalAdCount(string contentType);
	}
	
	public class AnalyticService : IAnalyticService, IAutoPersistent
	{
		[Persistent]
		private Dictionary<string, int> _eventsCount = new Dictionary<string, int>();
		
		private IAnalyticImplementation[] _analyticImplementations;

		private const string AD_KEY = "ad";
		private const string IAP_KEY = "iap";


		public void OnLoaded()
		{
			Init();
		}

		private void Init()
		{
			_analyticImplementations = new IAnalyticImplementation[]
			{
				//new AdjustAnalyticImplementation(), 
			};
			foreach (var analytic in _analyticImplementations)
			{
				analytic.Initialize();
			}
			
			Debug.Log("AnalyticService Initialize");
		}

		#region Ad
	
		public void LogTotalAdCount(string contentType)
		{
			IncrementEventCount(AD_KEY);

			if (string.IsNullOrEmpty(contentType))
			{
				contentType = "HasNotInfo";
			}

			foreach (var analytic in _analyticImplementations)
			{
				analytic.LogTotalAdCount(contentType, GetEventCount(AD_KEY));
			}

			if (IsFirstEvent(AD_KEY))
			{
				foreach (var analytic in _analyticImplementations)
				{
					analytic.LogFirstAdViewed(contentType);
				}
			}
		}

		#endregion

		private void IncrementEventCount(string name)
		{
			if (_eventsCount.ContainsKey(name))
			{
				_eventsCount[name]++;
			}
			else
			{
				_eventsCount.Add(name, 1);
			}
		}

		private int GetEventCount(string name)
		{
			return _eventsCount.ContainsKey(name) ? _eventsCount[name] : 0;
		}
		
		private bool IsFirstEvent(string name)
		{
			return _eventsCount.ContainsKey(name) && _eventsCount[name] <= 1;
		}
	}

	public class AnalyticIapProduct
	{
		public string ProductId;
		public string TransactionId;
		public string IsoCurrencyCode;
		public decimal Price;

		public AnalyticIapProduct(string productId, string transactionId, string isoCurrencyCode, decimal price)
		{
			ProductId = productId;
			TransactionId = transactionId;
			IsoCurrencyCode = isoCurrencyCode;
			Price = price;
		}
	}
}