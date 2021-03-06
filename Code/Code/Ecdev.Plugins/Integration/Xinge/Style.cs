﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ecdev.Plugins.Integration.Xinge
{
    public class Style
    {
        public Style(int builderId) : this(builderId, 0, 0, 1, 0, 1, 0, 1)
        {
            
        }
        public Style(int builderId, int ring, int vibrate, int clearable, int nId)
        {
            this.BuilderId = builderId;
            this.Ring = ring;
            this.Vibrate = vibrate;
            this.Clearable = clearable;
            this.Nid = nId;
        }
        public Style(int builderId, int ring, int vibrate, int clearable,
                int nId, int lights, int iconType, int styleId)
        {
            this.BuilderId = builderId;
            this.Ring = ring;
            this.Vibrate = vibrate;
            this.Clearable = clearable;
            this.Nid = nId;
            this.Lights = lights;
            this.IconType = iconType;
            this.StyleId = styleId;
        }

        public bool IsValid()
        {
            if (Ring < 0 || Ring > 1) return false;
            if (Vibrate < 0 || Vibrate > 1) return false;
            if (Clearable < 0 || Clearable > 1) return false;
            if (Lights < 0 || Lights > 1) return false;
            if (IconType < 0 || IconType > 1) return false;
            if (StyleId < 0 || StyleId > 1) return false;

            return true;
        }

        public int BuilderId { get; set; }
        public int Ring { get; set; }
        public int Vibrate { get; set; }
        public int Clearable { get; set; }
        public int Nid { get; set; }
        public string RingRaw { get; set; }
        public int Lights { get; set; }
        public int IconType { get; set; }
        public string IconRes { get; set; }
        public int StyleId { get; set; }
        public string SmallIcon { get; set; }
    }
}
