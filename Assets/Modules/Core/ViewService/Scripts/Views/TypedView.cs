using Core.Animation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.View
{
	public abstract class TypedView : BaseView
	{
		[SerializeField]
		private CanvasGroup _interactableContent;
		[SerializeField] private List<AnimationBase> _appearAnimations;
		[SerializeField] private List<AnimationBase> _disappearAnimations;

		public abstract ViewType Type { get; }

		public override void OnViewRemove()
		{
			base.OnViewRemove();

			if (ViewData != null)
				if (ViewData.ParentView != ViewType.None && ViewData.ParentViewData != null)
					_viewService.Show(ViewData.ParentView, ViewData.ParentViewData);
		}

		public virtual void OnSetupPrepare()
		{
			if (_interactableContent != null)
			{
				_interactableContent.blocksRaycasts = false;
			}
			Debug.Log($"On Setup OnPrepare {Type}");
		}

		public void OnSetupDone()
		{
			if (_interactableContent != null)
			{
				_interactableContent.blocksRaycasts = true;
			}
			Debug.Log($"On Setup Done {Type}");
		}

		public virtual IEnumerable<Coro> AnimIn()
		{
			if (!_appearAnimations.Any())
			{
				return new List<Coro>();
			}

			return _appearAnimations
				.Select(appearAnimation => _coroutineService.Run(appearAnimation.Animate()));
		}

		public virtual IEnumerable<Coro> AnimOut()
		{
			if (!_disappearAnimations.Any())
			{
				return new List<Coro>();
			}

			return _disappearAnimations
				.Select(appearAnimation => _coroutineService.Run(appearAnimation.Animate()));
		}
	}
}