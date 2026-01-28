using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums
{
    public enum GameStateType
    {
        INVALID_BED,
        END_RAINING,
        BEGIN_RAINING,
        CHANGE_GAMEMODE,                        //0: Survival, 1: Creative, 2: Adventure, 3: Spectator
        EXIT_END,
        DEMO_MESSAGE,
        ARROW_HITTING_PLAYER,
        FADE_VALUE,
        FADE_TIME,
        PLAY_ELDER_GUARDIAN_MOB_APPEARANCE      //Effect & Sound
    }
}
