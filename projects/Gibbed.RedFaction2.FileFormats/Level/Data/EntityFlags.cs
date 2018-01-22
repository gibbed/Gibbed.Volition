using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [Flags]
    public enum EntityFlags : ulong
    {
        None = 0,
        RunWaypointList = 1ul << 0,
        StartHidden = 1ul << 1,
        Unknown2 = 1ul << 2,
        EndGameIfKilled = 1ul << 3,
        CowerFromWeapon = 1ul << 4,
        QuestionUnarmedPlayer = 1ul << 5,
        DoNotHum = 1ul << 6,
        NoShadow = 1ul << 7,
        AlwaysSimulate = 1ul << 8,
        HasPerfectAim = 1ul << 9,
        PermanentCorpse = 1ul << 10,
        NeverFlee = 1ul << 11,
        NeverLeave = 1ul << 12,
        NoPersonaMessages = 1ul << 13,
        Unknown14 = 1ul << 14,
        FadeCorpseImmediately = 1ul << 15,
        NeverCollideWithPlayer = 1ul << 16,
        UseCustomAttackRange = 1ul << 17,
        Unknown18 = 1ul << 18,
        IsBoarded = 1ul << 19,
        ReadyToFireState = 1ul << 20,
        OnlyAttackPlayer = 1ul << 21,
        Unknown22 = 1ul << 22,
        IsDeaf = 1ul << 23,
        IgnoreTerrainWhenFiring = 1ul << 24,
        StartCrouched = 1ul << 25,
        Unknown26 = 1ul << 26, //
        Unknown27 = 1ul << 27, //
        Unknown28Mask = 1ul << 28 | 1ul << 29,
        Unknown30 = 1ul << 30, //
        Unknown31 = 1ul << 31, //

        Unknown32 = 1ul << 32 + 0, //
        Unknown33 = 1ul << 32 + 1, //
        Unknown34 = 1ul << 32 + 2, //
        Unknown35 = 1ul << 32 + 3, //
        Unknown36 = 1ul << 32 + 4, //
        Unknown37 = 1ul << 32 + 5,
        Unknown38 = 1ul << 32 + 6, //
        Unknown39 = 1ul << 32 + 7, //
        Unknown40 = 1ul << 32 + 8, //
        Unknown41 = 1ul << 32 + 9, //
        Unknown42Mask = 1ul << 32 + 10 | 1ul << 32 + 11 | 1ul << 32 + 12,
        Unknown45 = 1ul << 32 + 13, //
        Unknown46 = 1ul << 32 + 14, //

        Invalid = 1ul << 32 + 15 |
                  1ul << 32 + 16 |
                  1ul << 32 + 17 |
                  1ul << 32 + 18 |
                  1ul << 32 + 19 |
                  1ul << 32 + 20 |
                  1ul << 32 + 21 |
                  1ul << 32 + 22 |
                  1ul << 32 + 23 |
                  1ul << 32 + 24 |
                  1ul << 32 + 25 |
                  1ul << 32 + 26 |
                  1ul << 32 + 27 |
                  1ul << 32 + 28 |
                  1ul << 32 + 29 |
                  1ul << 32 + 30 |
                  1ul << 32 + 31,
    }
}
