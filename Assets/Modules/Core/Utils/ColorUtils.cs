using System.Collections.Generic;
using UnityEngine;

public enum ColorName
{
    aqua,
    black,
    blue,
    brown,
    cyan,
    darkblue,
    fuchsia,
    green,
    grey,
    lightblue,
    lime,
    magenta,
    maroon,
    navy,
    olive,
    orange,
    purple,
    red,
    silver,
    teal,
    white,
    yellow,
    error,
    exception,
    info,
    warning
}

public static class ColorUtils
{
    private static readonly Dictionary<ColorName, string> colorPalette = new Dictionary<ColorName, string>
    {
        [ColorName.aqua] = "#00ffffff",
        [ColorName.black] = "#000000ff",
        [ColorName.blue] = "#0000ffff",
        [ColorName.brown] = "#a52a2aff",
        [ColorName.cyan] = "#00ffffff",
        [ColorName.darkblue] = "#0000a0ff",
        [ColorName.fuchsia] = "#ff00ffff",
        [ColorName.green] = "#008000ff",
        [ColorName.grey] = "#808080ff",
        [ColorName.lightblue] = "#add8e6ff",
        [ColorName.lime] = "#00ff00ff",
        [ColorName.magenta] = "#ff00ffff",
        [ColorName.maroon] = "#800000ff",
        [ColorName.navy] = "#000080ff",
        [ColorName.olive] = "#808000ff",
        [ColorName.orange] = "#ffa500ff",
        [ColorName.purple] = "#800080ff",
        [ColorName.red] = "#ff0000ff",
        [ColorName.silver] = "#c0c0c0ff",
        [ColorName.teal] = "#008080ff",
        [ColorName.white] = "#ffffffff",
        [ColorName.yellow] = "#ffff00ff",
        [ColorName.error] = "#ff0000ff",
        [ColorName.exception] = "#ff0000ff",
        [ColorName.warning] = "#ffff00ff",
        [ColorName.info] = "#ffffffff"
    };

    public static string GetColorString(ColorName colorName)
    {
        return colorPalette[colorName];
    }

    public static Color GetColor(ColorName colorName)
    {
        var colorString = colorPalette[colorName];
        if (ColorUtility.TryParseHtmlString(colorString, out var color)) return color;

        return Color.white;
    }
}

public static class ColorExtensions
{
    public static string ToColorString(this Color c)
    {
        return "#" + ColorUtility.ToHtmlStringRGBA(c);
    }
}