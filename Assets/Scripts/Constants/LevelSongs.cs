using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Constants
{
    public static class LevelSongs
    {
        public static Dictionary<String, String> dict = new Dictionary<string, string>
        {
             { LevelNames.FirstLevel, SoundNames.FirstLevel },
             { LevelNames.SecondLevel, SoundNames.SecondLevel },
             { LevelNames.ThirdLevel, SoundNames.ThirdLevel}
        };
    }
}