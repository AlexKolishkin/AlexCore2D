using System;
using UnityEngine;

namespace Core.Sprites
{
    public static class AtlasUtils
    {
        public static string GetSpriteName(this ResourceIconType type)
        {
            switch (type)
            {
                case ResourceIconType.Money: return "CoinCurrency";
                case ResourceIconType.Hearts: return "HeartCurrency";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }


    public enum ResourceIconType
    {
        Money,
        Hearts,
    }
}