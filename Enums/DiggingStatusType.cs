using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Enums
{
    public enum DiggingStatusType
    {
        STARTED_DIGGING = 0,
        CANCELLED_DIGGING = 1,
        FINISHED_DIGGING = 2,
        DROP_ITEM_STACK = 3,
        DROP_ITEM = 4,
        SHOOT_ARROW_FINISHED_EATING = 5,
        SWAP_ITEM_IN_HAND = 6
    }
}
