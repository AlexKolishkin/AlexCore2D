using System.Collections.Generic;
using UnityEngine;

namespace Core.Analytic
{
	public class AdjustAnalyticImplementation : IAnalyticImplementation
	{
		public void Initialize()
		{
			Debug.Log("AdjustAnalyticImplementation Initialized");
		}

		public void LogTotalAdCount(string contentType, int count)
		{
		}

		public void LogFirstAdViewed(string contentType)
		{
		}

		public void SimpleEvent(string eventName, Dictionary<string, object> eventParams)
		{
		}
	}
}