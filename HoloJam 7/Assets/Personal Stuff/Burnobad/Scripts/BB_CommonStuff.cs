using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace BB_CommonStuff
{
    [Serializable]
    public struct Stats
    {
        public int strenght;
        public int dexterity;
        public int intellect;
        public int charisma;

        public Stats(int min, int max)
        {
            strenght = UnityEngine.Random.Range(min, max);
            dexterity = UnityEngine.Random.Range(min, max);
            intellect = UnityEngine.Random.Range(min, max);
            charisma = UnityEngine.Random.Range(min, max);
        }
    }
}
