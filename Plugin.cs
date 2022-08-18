using BepInEx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            StartCoroutine(CheckForUpdates());
        }
        IEnumerator CheckForUpdates()
        {
            UnityWebRequest request = UnityWebRequest.Get("https://api.github.com/repos/ZedDevStuff/UKUIHElper/releases/latest");
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError)
            {
                Logger.LogError(request.error);
            }
            else
            {
                ReleaseData latestVersion = JsonUtility.FromJson<ReleaseData>(request.downloadHandler.text);
                if(latestVersion.tag_name != PluginInfo.PLUGIN_VERSION)
                {
                    Logger.LogInfo("New version available: " + latestVersion);
                    Logger.LogInfo("Download it at: " + latestVersion.browser_download_url);
                }
                else
                {
                    Logger.LogInfo("You have the latest version.");
                }
            }
            Init();
        }

        void Init()
        {
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
public class ReleaseData
{
    public string name;
    public string tag_name;
    public string browser_download_url;
}
public class UpdaterData
{
    public static string github = "ZedDevStuff/FPSMeter";
}
