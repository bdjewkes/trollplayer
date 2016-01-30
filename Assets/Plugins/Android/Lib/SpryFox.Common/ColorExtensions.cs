using UnityEngine;
using System.Collections;

namespace SpryFox.Common {

    public static class ColorExtensions {
        public static Color SetR(this Color self, float r) {
            return new Color(r, self.g, self.b, self.a);
        }

        public static Color SetG(this Color self, float g) {
            return new Color(self.r, g, self.b, self.a);
        }

        public static Color SetB(this Color self, float b) {
            return new Color(self.r, self.g, b, self.a);
        }

        public static Color SetA(this Color self, float a) {
            return new Color(self.r, self.g, self.b, a);
        }

        public static Color Invert(this Color self) {
            return new ColorHSV(self).Invert().ToColor();
        }

        public static Color32 ToColor32(this Color self) {
            return new Color32((byte)(self.r * 255), (byte)(self.g * 255), (byte)(self.b * 255), (byte)(self.a * 255));
        }

        // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
        
        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r,g,b, 255);
        }
    }

    public static class Color32Extensions {
        public static bool FastEquals(this Color32 self, Color32 o) {
            return self.r == o.r && self.g == o.g && self.b == o.b && self.a == o.a;
        }
    }

}