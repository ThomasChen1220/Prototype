///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Vintage Vision Tool.
    /// </summary>
    public sealed class VintageVision : EditorWindow
    {
      private Camera[] allCameras;
      private int selectedCameraIndex = -1;

      private readonly List<Type> allVintages = new List<Type>();

      private bool autoRefresh = false;

      private Vector2 scrollPosition = Vector2.zero;

      private string productID;
      private readonly Dictionary<string, bool> foldoutDisplay = new Dictionary<string, bool>();

      private readonly Dictionary<string, RenderTexture> renderTexturesCache = new Dictionary<string, RenderTexture>();

      private readonly string SelectedCameraNameKey = "Nephasto.Vintage.SelectedCameraName";
      private readonly string AutoRefreshKey = "Nephasto.Vintage.AutoRefresh";

      [MenuItem("Window/Nephasto/Vintage/Vintage Vision")]
      private static void Launch()
      {
        VintageVision vintageVision = EditorWindow.GetWindow<VintageVision>();
        vintageVision.titleContent = new GUIContent("Vintage Vision");
        vintageVision.minSize = new Vector2(800.0f, 400.0f);
        vintageVision.autoRepaintOnSceneChange = true;
        vintageVision.ShowUtility();
      }

      private void OnEnable()
      {
        productID = GetType().ToString().Replace("EditorWindow", string.Empty);

        Type[] types = Assembly.GetAssembly(typeof(VintageBase)).GetTypes();
        for (int i = 0; i < types.Length; ++i)
        {
          if (types[i].IsClass == true && types[i].IsAbstract == false && (types[i].IsSubclassOf(typeof(VintageBase)) == true))
            allVintages.Add(types[i]);
        }

        string defaultCameraName = EditorPrefs.GetString(SelectedCameraNameKey, "Main Camera");
        allCameras = GameObject.FindObjectsOfType<Camera>();

        for (int i = 0; i < allCameras.Length; ++i)
        {
          if (allCameras[i].name.Equals(defaultCameraName) == true)
          {
            selectedCameraIndex = i;

            break;
          }
        }

        autoRefresh = EditorPrefs.GetBool(AutoRefreshKey, false);
      }

      private void OnDisable()
      {
        foreach (var kv in renderTexturesCache)
          RenderTexture.ReleaseTemporary(kv.Value);

        renderTexturesCache.Clear();

        if (selectedCameraIndex != -1)
        {
          Camera camera = allCameras[selectedCameraIndex];

          VintageBase[] vintages = camera.gameObject.GetComponents<VintageBase>();
          for (int i = 0; i < vintages.Length; ++i)
            DestroyImmediate(vintages[i]);
        }
      }

      private void Update()
      {
        if (autoRefresh == true)
          renderTexturesCache.Clear();
      }

      private void OnGUI()
      {
        Inspector.BeginVertical();
        {
          VisorGUI();

          Inspector.Separator();

          CommandsGUI();
        }
        Inspector.EndVertical();
      }

      private void VisorGUI()
      {
        Inspector.BeginVertical("box");
        {
          if (allVintages.Count > 0 && selectedCameraIndex < allCameras.Length)
          {
            Camera camera = selectedCameraIndex != -1 ? allCameras[selectedCameraIndex] : null;

            if (camera != null && camera.gameObject.activeSelf == true && camera.enabled == true)
            {
              scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
              {
                for (int i = 0; i < allVintages.Count; ++i)
                {
                  VintageBase vintage = camera.gameObject.GetComponent(allVintages[i]) as VintageBase;
                  if (vintage == null)
                  {
                    vintage = camera.gameObject.AddComponent(allVintages[i]) as VintageBase;
                    vintage.hideFlags = HideFlags.HideInInspector;
                  }

                  float widthSlot = this.position.width - 45.0f;

                  Inspector.BeginVertical("box");
                  {
                    EffectGUI(vintage, Inspector.HumanizeName(allVintages[i].Name.Replace("Vintage", string.Empty)), widthSlot, widthSlot / camera.aspect);
                  }
                  Inspector.EndVertical();

                  Inspector.Separator();
                }
              }
              EditorGUILayout.EndScrollView();
            }
            else
              CenterMessage("No camera selected.");
          }
          else
            CenterMessage("No Vintage effects found!");
        }
        Inspector.EndVertical();
      }

      private void EffectGUI(VintageBase vintage, string vintageName, float width, float height)
      {
        GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
        {
          GUILayout.Label($"{vintageName}: {vintage.ToString()}", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

          Rect rect = GUILayoutUtility.GetLastRect();
          if (rect.yMin > scrollPosition.y && rect.yMin < (scrollPosition.y + Screen.height) ||
              rect.yMax > scrollPosition.y && rect.yMax < (scrollPosition.y + Screen.height))
          {
            Camera currentCamera = allCameras[selectedCameraIndex];

            Texture texture = null;

            string key = vintageName;
            if (renderTexturesCache.ContainsKey(key) == true)
              texture = renderTexturesCache[key];
            else
            {
              RenderTexture renderTexture = RenderTexture.GetTemporary((int)width - 10, (int)height - 30, 16, RenderTextureFormat.ARGB32);

              vintage.enabled = true;

              currentCamera.targetTexture = renderTexture;
              currentCamera.Render();
              currentCamera.targetTexture = null;

              renderTexturesCache.Add(key, renderTexture);

              texture = renderTexture;

              vintage.enabled = false;
            }

            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin + 20.0f, width - 10.0f, height - 30.0f), texture);
          }
        }
        GUILayout.EndVertical();
      }

      private void CommandsGUI()
      {
        Inspector.LabelWidth = 85.0f;

        Inspector.BeginHorizontal("box");
        {
          Inspector.BeginVertical();
          {
            allCameras = GameObject.FindObjectsOfType<Camera>();

            List<string> cameraNames = new List<string>();
            List<int> cameraIds = new List<int>();

            for (int i = 0; i < allCameras.Length; ++i)
            {
              cameraNames.Add(allCameras[i].name);
              cameraIds.Add(i);
            }

            selectedCameraIndex = EditorGUILayout.IntPopup("Camera", selectedCameraIndex, cameraNames.ToArray(), cameraIds.ToArray(), GUILayout.Width(Screen.width / 3.0f));
            if (selectedCameraIndex < allCameras.Length)
              EditorPrefs.SetString(SelectedCameraNameKey, allCameras[selectedCameraIndex].name);

            autoRefresh = EditorGUILayout.Toggle("Auto refresh", autoRefresh);
            EditorPrefs.SetBool(AutoRefreshKey, autoRefresh);
          }
          Inspector.EndVertical();

          Inspector.FlexibleSpace();

          Inspector.EnableGUI = autoRefresh == false;

          if (GUILayout.Button("Refresh", GUILayout.Width(Screen.width / 5.0f), GUILayout.Height(40.0f)) == true)
            renderTexturesCache.Clear();

          Inspector.EnableGUI = true;
        }
        Inspector.EndHorizontal();
      }

      private void CenterMessage(string text)
      {
        Inspector.FlexibleSpace();

        Inspector.BeginHorizontal();
        {
          Inspector.FlexibleSpace();

          GUILayout.Label(text);

          Inspector.FlexibleSpace();
        }
        Inspector.EndHorizontal();

        Inspector.FlexibleSpace();
      }

      private bool Foldout(string text)
      {
        bool display = GetFoldoutDisplay(text);

        Rect rect = GUILayoutUtility.GetRect(16.0f, 22.0f, Inspector.FoldoutStyle);
        GUI.Box(rect, text, Inspector.FoldoutStyle);

        Rect toggleRect = new Rect(rect.x + 4.0f, rect.y + 2.0f, 13.0f, 13.0f);
        if (Event.current.type == EventType.Repaint)
          EditorStyles.foldout.Draw(toggleRect, false, false, display, false);

        Event e = Event.current;
        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition) == true)
        {
          display = !display;
          e.Use();
        }

        SetFoldoutDisplay(text, display);

        return display;
      }

      private bool GetFoldoutDisplay(string foldoutName)
      {
        string key = $"{productID}.display{foldoutName}";
        bool value = true;

        if (foldoutDisplay.ContainsKey(key) == false)
        {
          value = PlayerPrefs.GetInt(key, 0) == 1;

          foldoutDisplay.Add(key, value);
        }
        else
          value = foldoutDisplay[key];

        return value;
      }

      private void SetFoldoutDisplay(string foldoutName, bool value)
      {
        string key = $"{productID}.display{foldoutName}";

        if (foldoutDisplay.ContainsKey(key) == false)
          foldoutDisplay.Add(key, value);
        else
          foldoutDisplay[key] = value;

        PlayerPrefs.SetInt(key, value == true ? 1 : 0);
      }
    }
  }
}