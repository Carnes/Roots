using UnityEngine;

namespace Roots
{
    public class GameSettings : MonoBehaviour
    {
        public void Start()
        {
            Application.targetFrameRate = 300;
            QualitySettings.vSyncCount = 0;            
        }
    }
}