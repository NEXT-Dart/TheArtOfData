using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData
{
    class HSVColor
    {
        private float h;
        private float s;
        private float v;

        public HSVColor(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            h = color.GetHue();
            s = ((max == 0) ? 0 : 1f - (1f * min / max));
            v = (max / 255f);
        }

        public HSVColor(float h, float s, float v)
        {
            this.v = v;
            this.s = s;
            this.h = h;
        }

        public float Hue
        {
            get { return h; }
            set { h = value; }
        }

        public float Saturation
        {
            get { return s; }
            set { s = value; }
        }

        public float Value
        {
            get { return v; }
            set { v = value; }
        }

        public Color ToRGB()
        {
            int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);

            v = v * 255;
            int _v = Convert.ToInt32(v);
            int p = Convert.ToInt32(v * (1 - s));
            int q = Convert.ToInt32(v * (1 - f * s));
            int t = Convert.ToInt32(v * (1 - (1 - f) * s));

            if (hi == 0)
                return Color.FromArgb(255, _v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, _v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, _v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, _v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, _v);
            else
                return Color.FromArgb(255, _v, p, q);
        }
    }
}
