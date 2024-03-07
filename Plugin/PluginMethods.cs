﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Plugin
{
    public enum PluginMethods
    {
        OnInit,
        OnStop,
        OnTick,
        AddPlayer,
        RemovePlayer,
        UpdatePlayer,
        GetPlayers
    }
}