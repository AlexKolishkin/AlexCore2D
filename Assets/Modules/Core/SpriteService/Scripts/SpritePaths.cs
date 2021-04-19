using System;
using UnityEngine;

namespace Core.Sprites
{
    public static class AtlasUtils
    {
        public static string GetSpriteName(this CurrencyIconType type)
        {
            switch (type)
            {
                case CurrencyIconType.Money: return "CoinCurrency";
                case CurrencyIconType.Hearts: return "HeartCurrency";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
	}

    public enum CurrencyIconType
    {
        Money,
        Hearts,
    }
}