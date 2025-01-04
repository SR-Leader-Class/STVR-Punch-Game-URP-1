using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveBag{
    public enum CharStyle{
        None,
        CuteChar
    }
    [System.Serializable][CreateAssetMenu(fileName = "StyleDataPreset", menuName = "StyleDataPreset", order = 0)]
    public class StyleDataPreset : ScriptableObject
    {
        public CharStyle charStyle = CharStyle.None;
        [Range(1, 6)]
        public int bodies = 0;
        [Range(0, 10)]
        public int bodyParts = 0;
        [Range(0, 11)]
        public int eyes = 0;
        [Range(0, 10)]
        public int gloves = 0;
        [Range(0, 4)]
        public int headParts = 0;
        [Range(1, 15)]
        public int mouthnNoses = 0;
        [Range(0, 8)]
        public int tails = 0;
        public int otherDress = 0;
        public int additionEyeEars = 0;
    }
}

