using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Extensions
{
	#region String manipulations

	public static string Colored(this string str, string color)
		=> $"<color={color}>{str}</color>";

	public static string Size(this string str, int size)
		=> $"<size={size}>{str}</size>";

	public static string Bold(this string str) => $"<b>{str}</b>";

	public static string Italic(this string str) => $"<i>{str}</i>";

	public static string BoldItalic(this string str) => str.Bold().Italic();

	public static bool IsNonEmpty(this string str)
		=> !string.IsNullOrEmpty(str);


	public static int AsInt(this string str, int defaultValue = 0)
	{
		if (!string.IsNullOrEmpty(str))
		{
			int val = 0;
			if (int.TryParse(str, out val))
			{
				return val;
			}
		}

		return defaultValue;
	}

	public static float AsFloat(this string str, float defaultValue = 0f)
	{
		if (!string.IsNullOrEmpty(str))
		{
			float val = 0f;
			if (float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
			{
				return val;
			}
		}

		return defaultValue;
	}

	public static double AsDouble(this string str, double defaultValue = 0)
	{
		if (str.IsValid())
		{
			double val = 0.0;
			if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
			{
				return val;
			}
		}

		return defaultValue;
	}

	public static bool AsBool(this string str, bool defaultValue = false)
	{
		if (!string.IsNullOrEmpty(str))
		{
			bool val = false;
			if (bool.TryParse(str, out val))
			{
				return val;
			}

			int iVal = 0;
			if (int.TryParse(str, out iVal))
			{
				return (iVal != 0);
			}
		}

		return defaultValue;
	}

	public static string Format(this double value, string format)
	{
		return string.Format(format, value);
	}

	public static bool IsValid(this string str)
		=> !string.IsNullOrEmpty(str);

	#endregion

	public static void SetListener(this Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public static void SetListener(this Toggle toggle, UnityAction<bool> action)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(action);
    }

    public static void SetPointerListeners(this EventTrigger trigger,
        UnityAction<BaseEventData> onDown = null,
        UnityAction<BaseEventData> onClick = null,
        UnityAction<BaseEventData> onUp = null)
    {
        trigger.triggers.Clear();

        if (onDown != null)
        {
            EventTrigger.TriggerEvent onDownEvent = new EventTrigger.TriggerEvent();
            onDownEvent.AddListener(onDown);
            trigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown,
                callback = onDownEvent
            });
        }

        if (onClick != null)
        {
            EventTrigger.TriggerEvent onClickEvent = new EventTrigger.TriggerEvent();
            onClickEvent.AddListener(onClick);
            trigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick,
                callback = onClickEvent
            });
        }

        if (onUp != null)
        {
            EventTrigger.TriggerEvent onUpEvent = new EventTrigger.TriggerEvent();
            onUpEvent.AddListener(onUp);
            trigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp,
                callback = onUpEvent
            });
        }
    }

	public static void Deactivate(this GameObject gameObject)
    {
        if (gameObject)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public static void ToggleActivity(this GameObject gameObject, bool isActivate)
    {
        if (isActivate)
        {
            gameObject.Activate();
        }
        else
        {
            gameObject.Deactivate();
        }
    }

    public static void Activate(this GameObject gameObject)
    {
        if (gameObject)
        {
            if (false == gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
    }

    public static void Activate(this GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            obj.Activate();
        }
    }

    public static void Deactivate(this GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            obj.Deactivate();
        }
    }

    public static void Deactivate(this MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour && monoBehaviour.gameObject)
        {
            Deactivate(monoBehaviour.gameObject);
        }
    }

    public static void Activate(this MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour && monoBehaviour.gameObject)
        {
            Activate(monoBehaviour.gameObject);
        }
    }

    public static void SetInteractable(this Button button, bool value)
    {
        if (button.interactable != value)
        {
            button.interactable = value;
        }
    }

    public static void SetInteractableWithShader(this Button button, bool value)
    {
        if (button.interactable != value)
        {
            button.interactable = value;
        }

        if (button.image != null && button.image.material != null)
        {
            button.image.material.SetFloat("_Enabled", value ? 0 : 1);
        }
    }

    public static T GetOrAdd<T>(this GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (!comp)
        {
            comp = obj.AddComponent<T>();
        }

        return comp;
    }

    private static NumberFormatInfo doubleFloatNumberFormat = null;

    private static NumberFormatInfo DoubleFloatNumberFormat
    {
        get
        {
            if (doubleFloatNumberFormat == null)
            {
                CultureInfo cultureInfo = new CultureInfo("en-us");
                NumberFormatInfo formatInfo = cultureInfo.NumberFormat;
                formatInfo.NumberDecimalSeparator = ",";
                doubleFloatNumberFormat = formatInfo;
            }

            return doubleFloatNumberFormat;
        }
    }

    public static double ToDouble(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0.0;
        }

        double result;
        if (!double.TryParse(str, NumberStyles.Any, DoubleFloatNumberFormat, out result))
        {
            result = 0.0;
        }

        return result;
    }

    public static float ToFloat(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0.0f;
        }

        float result;
        if (!float.TryParse(str, NumberStyles.Any, DoubleFloatNumberFormat, out result))
        {
            result = 0.0f;
        }

        return result;
    }

    public static int ToInt(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        int result;
        if (false == int.TryParse(str, out result))
        {
            result = 0;
        }

        return result;
    }

    public static string AsString<T>(this T[] array)
    {
        if (array == null)
        {
            return string.Empty;
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (T element in array)
        {
            stringBuilder.Append(element.ToString());
            stringBuilder.Append(",");
        }

        if (stringBuilder.Length > 0)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        return stringBuilder.ToString();
    }

    public static bool IsWhole(this float num)
	{
		return Mathf.Approximately(num - Mathf.FloorToInt(num), 0f);
	}

	public static void SetUniformScale(this RectTransform rectTransform, float value)
        => rectTransform.localScale = new Vector3(value, value, 1f);

    public static void SetPosition(this RectTransform rectTransform, Vector2 pos)
        => rectTransform.anchoredPosition = pos;

    public static void SetPosition(this RectTransform rectTransform, float x, float y)
        => rectTransform.SetPosition(new Vector2(x, y));

    public static void SetColor(this RectTransform rectTransform, Color c)
    {
        Graphic g = rectTransform.GetComponent<Graphic>();
        if (g != null)
        {
            g.color = c;
        }
    }

    public static string GetLocalizedString(this string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("GetLocalizedString: string is null");
            return "string is null";
        }
		//     var result = Locator.Service<ResourceService>().Localization.GetString(key);
		// return result;
		return null;
    }

    public static string AsString<K, V>(this Dictionary<K, V> dict, Func<K, string> keyToString,
        Func<V, string> valueToString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in dict)
        {
            sb.AppendLine($"{keyToString(kvp.Key)} => {valueToString(kvp.Value)}");
        }

        return sb.ToString();
    }

    public static Color ChangeAlpha(this Color color, float newAlpha)
    {
        return new Color(color.r, color.g, color.b, newAlpha);
    }

    public static Action<float, GameObject> UpdateZRotation(this RectTransform trs, Action action = null)
    {
        return (v, o) =>
        {
            trs.localRotation = Quaternion.Euler(0, 0, v);
            action?.Invoke();
        };
    }

    public static Action<float, float, GameObject> UpdateZRotationTimed(this RectTransform trs)
        => (v, t, o) => trs.localRotation = Quaternion.Euler(0, 0, v);

    public static Action<Vector3, GameObject> UpdateScaleFunctor(this RectTransform trs, Action action = null)
        => (s, o) =>
        {
            trs.localScale = s;
            action?.Invoke();
        };

    public static Action<Vector3, float, GameObject> UpdateScaleTimedFunctor(this RectTransform trs)
        => (s, t, o) => { trs.localScale = s; };

    public static Action<Vector2, GameObject> UpdatePositionFunctor(this RectTransform trs, Action action = null)
        => (p, o) =>
        {
            trs.anchoredPosition = p;
            action?.Invoke();
        };

    public static Action<Vector2, float, GameObject> UpdatePositionTimedFunctor(this RectTransform trs)
        => (p, t, o) => { trs.anchoredPosition = p; };

    public static Action<Color, GameObject> UpdateColorFunctor(this Graphic trs, Action action = null)
        => (c, o) =>
        {
            trs.color = c;
            action?.Invoke();
        };

    public static Action<Color, float, GameObject> UpdateColorTimedFunctor(this Graphic graphic, Action action = null)
        => (c, t, o) => { graphic.color = c; };

    public static Action<float, GameObject> UpdateAlphaFunctor(this CanvasGroup group, Action action = null)
        => (v, o) =>
        {
            group.alpha = v;
            action?.Invoke();
        };

    public static Action<float, float, GameObject> UpdateAlphaTimedFunctor(this CanvasGroup group)
        => (v, t, o) => { group.alpha = v; };


    public static void CopyFrom<K, T>(this Dictionary<K, T> destination, Dictionary<K, T> source)
    {
        destination.Clear();
        if (source != null)
        {
            foreach (var kvp in source)
            {
                destination.Add(kvp.Key, kvp.Value);
            }
        }
    }

    public static List<T> WrapToList<T>(this T obj)
    {
        return new List<T> {obj};
    }
}