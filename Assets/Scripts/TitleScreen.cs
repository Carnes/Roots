using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TitleScreen : MonoBehaviour
    {
        public CinemachineVirtualCamera StartCam;
        public CinemachineVirtualCamera FlowerCam;
        public CinemachineVirtualCamera DirtCam;
        public CinemachineVirtualCamera UndergroundCam;

        public float DelayFromFlowerToDirt = 2;
        public float DelayFromDirtToUnderground = 2;

        public void Start()
        {
            FlowerCam.Priority = 0;
            DirtCam.Priority = 0;
            StartCam.Priority = 1000;
        }

        public void Play()
        {
            StartCoroutine(PlayIntroAnimation());
        }

        private IEnumerator PlayIntroAnimation()
        {
            MoveCamera(StartCam, FlowerCam);
            yield return new WaitForSeconds(DelayFromFlowerToDirt);
            MoveCamera(FlowerCam, DirtCam);
            yield return new WaitForSeconds(DelayFromDirtToUnderground);
            SceneManager.LoadScene("Prototype");
            //MoveCamera(DirtCam, UndergroundCam);

            //yield return new WaitForSeconds(3);
            //Application.Quit();
        }

        private void MoveCamera(CinemachineVirtualCamera currentCam, CinemachineVirtualCamera newCam)
        {
            // Set new camera as highest priority to start the transition
            newCam.Priority = 1000;
            currentCam.Priority = 0;
        }


    }
}