using System.Collections.Generic;

namespace Core.Analytic
{
	public interface IAnalyticImplementation
	{
		void Initialize();
		void LogTotalAdCount(string contentType, int count);
		void LogFirstAdViewed(string contentType);
		void SimpleEvent(string eventName, Dictionary<string, object> eventParams);
	}
}