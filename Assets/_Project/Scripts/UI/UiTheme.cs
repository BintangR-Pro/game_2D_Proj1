using UnityEngine;

/// <summary>
/// Satu tempat untuk warna UI navy-emas (P5) + konversi koordinat desain.
/// Koordinat desain = layar demo 320 x 180 piksel, dikali 4 (Canvas Scaler 1280 x 720).
/// </summary>
public static class UiTheme
{
    public static readonly Color Ink = Hex("#0F1722");
    public static readonly Color Panel = Hex("#162234");
    public static readonly Color Gold = Hex("#FFD13B");
    public static readonly Color Text = Hex("#F0F4F8");
    public static readonly Color Muted = Hex("#9FB3C8");
    public static readonly Color Disabled = Hex("#52667A");
    public static readonly Color Exp = Hex("#4CC9F0");
    public static readonly Color BarBack = Hex("#090E16");
    public static readonly Color Shadow = Hex("#0A1018");
    public static readonly Color Cream = Hex("#FFF3D1");

    /// <summary>1 piksel demo = 4 unit kanvas.</summary>
    public const float Px = 4f;

    public static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    /// <summary>x, y dari kiri-atas (piksel demo) -> anchor &amp; pivot (0, 1), anchoredPosition (x*4, -y*4), sizeDelta (w*4, h*4).</summary>
    public static void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(x * Px, -y * Px);
        rt.sizeDelta = new Vector2(w * Px, h * Px);
    }
}
