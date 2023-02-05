using UnityEngine;

namespace Audio
{
    public class MuteAudioSource : MonoBehaviour
    {
        public void Mute()
        {
            SetMute(true);
        }
        
        public void Unmute()
        {
            SetMute(false);
        }

        private void SetMute(bool value)
        {
            GetComponent<AudioSource>().mute = value;
        }

    }
}