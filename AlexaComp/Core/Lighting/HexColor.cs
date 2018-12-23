using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RGB.NET.Core;

namespace AlexaComp.Core.Lighting {
    class HexColor {

        public string color;

        public HexColor() {
            this.color = "#ffffff";
        }

        public HexColor(string color) {
            this.color = color;
        }
        
        public HexColor(int R, int G, int B) {
            this.color = R.ToString("X2") + G.ToString("X2") + B.ToString("X2");
        }

        public override string ToString() => this.color;

        public string ToStringAsRGB() {
            Color thisColor = this.ToRGB();
            return thisColor.R + " " + thisColor.G + " " + thisColor.B;
        }

        public Color ToRGB() {
            var color = "";
            if (this.color.StartsWith("#")) { // Strip leading # if it exists
                color = this.color.Substring(1);
            }
            string[] splitHex = color.SplitEveryN(2);
            return new Color(Convert.ToInt32(splitHex[0], 16), Convert.ToInt32(splitHex[1], 16), Convert.ToInt32(splitHex[2], 16));
        }
    }
}
