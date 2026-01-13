using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core
{
    public class MinecraftClient
    {
        public string Locale { get; set; } = string.Empty;          //Language
        public byte ViewDistance { get; set; }                      //Client-side render distance, in chunks
        public int ChatMode { get; set; }                           //0: enabled, 1: commands only, 2: hidden 
        public bool ChatColors { get; set; }                        //cum
        public byte DisplayedSkinParts { get; set; }
        public int MainHand { get; set; }                           //0: Left, 1: Right
    }
}
