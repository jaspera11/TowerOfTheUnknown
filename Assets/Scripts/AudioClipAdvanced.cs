using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AudioHelper.AudioControl
{
    [System.Serializable]
    public class AudioClipAdvanced : MonoBehaviour
    {
        public AudioClip source;
        public bool Loop = false;
    }
}
