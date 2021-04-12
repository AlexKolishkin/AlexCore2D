using System.Collections.Generic;
using System.Linq;


namespace Core.View
{
	public class WindowInstanceCollection
	{
		private readonly List<ViewType> _stack = new List<ViewType>();

		private readonly Dictionary<ViewType, TypedView> _typedViewInstances = new Dictionary<ViewType, TypedView>();
		public List<TypedView> Values => _typedViewInstances.Values.ToList();
		public List<ViewType> Keys => _typedViewInstances.Keys.ToList();
		public int Count => _stack.Count;

		public void Add(ViewType viewType, TypedView view)
		{
			if (!_typedViewInstances.ContainsKey(viewType))
			{
				_typedViewInstances[viewType] = view;
				_stack.Add(viewType);
			}
			else
			{
				_typedViewInstances[viewType] = view;
			}
		}

		public void Remove(ViewType viewType)
		{
			if (_typedViewInstances.ContainsKey(viewType))
			{
				_typedViewInstances.Remove(viewType);
				_stack.Remove(viewType);
			}
		}

		public TypedView Get(ViewType viewType)
		{
			if (_typedViewInstances.ContainsKey(viewType)) return _typedViewInstances[viewType];

			return null;
		}

		public ViewType GetLast()
		{
			return _stack.LastOrDefault();
		}

		public void Clear()
		{
			foreach (var key in Keys)
			{
				_typedViewInstances.Remove(key);
				_stack.Remove(key);
			}
		}

		public bool ContainsKey(ViewType viewType)
		{
			return _typedViewInstances.ContainsKey(viewType);
		}

		public bool TryGetValue(ViewType key, out TypedView instance)
		{
			return _typedViewInstances.TryGetValue(key, out instance);
		}
	}
}