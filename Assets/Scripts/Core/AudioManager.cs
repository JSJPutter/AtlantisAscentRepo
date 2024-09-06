using System;
using UnityEngine;

namespace Core {
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        
        
    }
}
