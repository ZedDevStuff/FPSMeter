using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UKUIHelper;

namespace FPSMeter
{
    [BepInPlugin("zed.fpsmeter", "FPS Meter", "1.0.0")]
    [BepInDependency("zed.uk.uihelper")]
    public class Plugin : BaseUnityPlugin
    {
        bool isActive = false;
        bool initialized = false;
        float fps;
        float fpsUpdateTime;
        float fpsUpdateInterval = 0.5f;
        GameObject fpsText;
        private void Awake()
        {
            StartCoroutine(Wait(0));
            //UnityEngine.SceneManagement.SceneManager.sceneLoaded += Scene;
        }
        void Scene(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            initialized = false;
            if(scene.name.Contains("Level") || !scene.name.Contains("Menu"))
            {
                StartCoroutine(Wait(0.5f));
            }
        }
        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            fpsText = UIHelper.CreateText();
            GameObject canvas = UIHelper.CreateOverlay(true);
            canvas.GetComponent<Canvas>().sortingOrder = 1000;
            fpsText.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>());
            fpsText.GetComponent<RectTransform>().SetAnchor(AnchorPresets.TopLeft);
            fpsText.GetComponent<RectTransform>().SetPivot(PivotPresets.TopLeft);
            fpsText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            fpsText.GetComponent<Text>().fontSize = 30;
            fpsText.GetComponent<Text>().fontStyle = FontStyle.Bold;
            fpsText.GetComponent<Text>().color = Color.white;
            fpsText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            fpsText.GetComponent<Text>().text = "FPS: ";
            fpsText.SetActive(false);
            initialized = true;
        }
        void ToggleFPS()
        {
            isActive = !isActive;
            fpsText.SetActive(isActive);
        }
        void Update()
        {
            if(initialized)
            {
                fpsUpdateTime += Time.deltaTime;
                if (fpsUpdateTime > fpsUpdateInterval)
                {
                    fpsUpdateTime = 0;
                    fps = 1 / Time.deltaTime;
                    fpsText.GetComponent<Text>().text = "FPS: " + Mathf.FloorToInt(fps);
                }
                if(Input.GetKeyDown(KeyCode.F3))
                {
                    ToggleFPS();
                }
            }
        }
    }
}
