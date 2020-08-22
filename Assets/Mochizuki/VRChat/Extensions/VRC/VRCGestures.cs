/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR
#if VRC_SDK_VRCSDK3

namespace Mochizuki.VRChat.Extensions.VRC
{
    // ReSharper disable once InconsistentNaming
    public enum VRCGestures : uint
    {
        Neutral = 0,

        Fist = 1,

        HandOpen = 2,

        FingerPoint = 3,

        Victory = 4,

        RockNRoll = 5,

        HandGun = 6,

        ThumbsUp = 7
    }
}

#endif
#endif