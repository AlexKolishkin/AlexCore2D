using System;
using System.Diagnostics;
using UnityEngine;

public static class TimeConverter
{
	public static string ToFullTimer(this TimeSpan span)
	{
		var s = "timer.s".GetLocalizedString();
		var m = "timer.m".GetLocalizedString();
		var h = "timer.h".GetLocalizedString();
		var d = "timer.d".GetLocalizedString();

		if (span.TotalSeconds < 0)
		{
			return $"00 {m} : 00 {s}";
		}

		if (span.TotalHours < 1)
		{
			return $"{span.Minutes:00} {m} : {span.Seconds:00} {s}";
		}

		if (span.TotalDays < 1)
		{
			return $"{span.Hours:00} {h} : {span.Minutes:00} {m} : {span.Seconds:00} {s}";
		}

		return $"{(int)span.TotalDays:00} {d} : {span.Hours:00} {h} : {span.Minutes:00} {m}";
	}

	public static string ToTimer(this TimeSpan span)
	{
		var s = "timer.s".GetLocalizedString();
		var m = "timer.m".GetLocalizedString();
		var h = "timer.h".GetLocalizedString();
		var d = "timer.d".GetLocalizedString();

		if (span.TotalSeconds < 0)
		{
			return $"00 {m} : 00 {s}";
		}

		if (span.TotalHours < 1)
		{
			return $"{span.Minutes:00} {m} : {span.Seconds:00} {s}";
		}

		if (span.TotalDays < 1)
		{
			return $"{span.Hours:00} {h} : {span.Minutes:00} {m}";
		}

		return $"{(int)span.TotalDays:00} {d} : {span.Hours:00} {h}";
	}

	public static string ToTimerWithoutSpaces(this TimeSpan span)
	{
		var s = "timer.s".GetLocalizedString();
		var m = "timer.m".GetLocalizedString();
		var h = "timer.h".GetLocalizedString();
		var d = "timer.d".GetLocalizedString();

		if (span.TotalSeconds < 0)
		{
			return $"00{m}:00{s}";
		}

		if (span.TotalHours < 1)
		{
			return $"{span.Minutes:00}{m}:{span.Seconds:00}{s}";
		}

		if (span.TotalDays < 1)
		{
			return $"{span.Hours:00}{h}:{span.Minutes:00}{m}";
		}

		return $"{(int)span.TotalDays:00}{d}:{span.Hours:00}{h}";
	}

	public static string ToTimerWithoutSymbol(this TimeSpan span)
	{
		if (span.TotalSeconds < 0)
		{
			return "00:00";
		}

		if (span.TotalHours < 1)
		{
			return $"{span.Minutes:00}:{span.Seconds:00}";
		}

		if (span.TotalDays < 1)
		{
			return $"{span.Hours:00}:{span.Minutes:00}";
		}

		return $"{(int)span.TotalDays:00}:{span.Hours:00}";
	}

	public static string ToSuperShortTimer(this TimeSpan span)
	{
		var s = "timer.s".GetLocalizedString();
		var m = "timer.m".GetLocalizedString();
		var h = "timer.h".GetLocalizedString();
		var d = "timer.d".GetLocalizedString();

		if (span.TotalSeconds < 0)
		{
			return $"00 {s}";
		}

		if (span.TotalMinutes < 1)
		{
			return $"{span.TotalSeconds:00} {s}";
		}

		if (span.TotalHours < 1)
		{
			return $"{span.TotalMinutes:00} {m}";
		}

		if (span.TotalDays < 1)
		{
			return $"{span.TotalHours:00} {h}";
		}

		return $"{(int)span.TotalDays:00} {d}";
	}

	public static float GetFlickerForTimer(float timeLeft, float flickerStart)
	{
		if (timeLeft > flickerStart)
			return 1;

		float visible = timeLeft / flickerStart;

		const float fadeToGrey = 0.07f;
		const float frameIndexForFlicker = 0.2f;
		const float greyValue = 0.5f;

		if (visible > 0.001f)
		{
			const float frequancy = 50f;

			float sin01 = visible > frameIndexForFlicker
					? (Mathf.Sin(frequancy / visible) + 1) * 0.5f
					: (Time.frameCount % 2)
				;


			float alpha = visible + (1 - visible) * sin01;

			if (visible > fadeToGrey)
				return alpha;
			else
			{
				float grey = (fadeToGrey - visible) / (fadeToGrey);
				return greyValue * grey + (1 - grey) * alpha;
			}
		}
		else
			return greyValue;
	}

	public static float ToSeconds(this Stopwatch stopwatch)
	{
		return stopwatch.ElapsedMilliseconds / (float)1000;
	}
}
