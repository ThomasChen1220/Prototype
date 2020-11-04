///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

using UnityEngine;
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// About window.
    /// </summary>
    public sealed class VintageAbout : EditorWindow
    {
      public static readonly string assetID = "Nephasto.Vintage";
      private static readonly string description = "A big collection of effects (<b>+50</b>) that can give to your games a <b>unique</b> and <b>retro look</b>. Instagram, vintage cameras, old computers and more.";

      private static readonly string publisherURL = "https://assetstore.unity.com/publishers/8484";
      private static readonly string forumURL = "https://forum.unity.com/threads/vintage.909254/";
      private static readonly string storeURL = "https://www.assetstore.unity3d.com/#!/content/20938";
      private static readonly string twitterURL = "https://twitter.com/Nephasto";
      private static readonly string facebookURL = "https://www.facebook.com/nephasto";

      private Vector2 scroll = Vector2.zero;

      private GUIStyle descriptionStyle;
      private GUIStyle buttonStyle;
      private GUIStyle iconStyle;
      private GUIStyle scrollStyle;

      private string changelog = "Changelog not available :(";
      private string version = "1.0";

      private Texture2D headerTexture;
      private Texture2D forumTexture;
      private Texture2D twitterTexture;
      private Texture2D facebookTexture;

      [MenuItem("Help/Nephasto/Vintage/About", false, 0)]
      public static void MenuAbout()
      {
        Open();
      }

      [MenuItem("Help/Nephasto/Vintage/Documentation", false, 0)]
      public static void MenuDocumentation()
      {
        Application.OpenURL(VintageBase.Documentation);
      }

      public static string GetAssetPath()
      {
        string[] results = AssetDatabase.FindAssets("VintageAbout t:Script", null);
        if (results.Length > 0)
        {
          string assetPath = AssetDatabase.GUIDToAssetPath(results[0]);
          assetPath = assetPath.Replace("Editor/About/VintageAbout.cs", string.Empty);
          if (string.IsNullOrEmpty(assetPath) == false)
            return assetPath;
        }

        return string.Empty;
      }

      public static void Open()
      {
        VintageAbout window = EditorWindow.GetWindow<VintageAbout>(true, "About 'Vintage'", true);
        window.minSize = window.maxSize = new Vector2(700, 500);
        window.ShowUtility();
      }

      private void OnEnable()
      {
        string assetPath = GetAssetPath();
        if (string.IsNullOrEmpty(assetPath) == false)
        {
          TextAsset changeLogAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + "Changelog.txt");
          if (changeLogAsset != null && string.IsNullOrEmpty(changeLogAsset.text) == false)
          {
            changelog = changeLogAsset.text;

            version = changelog.Split(new char[] { '\n', '\r' })[0];
          }

          headerTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Header.png");
          forumTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Forum.png");
          twitterTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Twitter.png");
          facebookTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Facebook.png");
        }

        EditorPrefs.SetString(assetID, version);
      }

      private void OnGUI()
      {
        if (descriptionStyle == null)
        {
          descriptionStyle = new GUIStyle(GUI.skin.label);
          descriptionStyle.alignment = TextAnchor.MiddleLeft;
          descriptionStyle.fontSize = 17;
          descriptionStyle.richText = true;
          descriptionStyle.wordWrap = true;
        }

        if (buttonStyle == null)
        {
          buttonStyle = new GUIStyle(GUI.skin.button);
          buttonStyle.alignment = TextAnchor.MiddleLeft;
          buttonStyle.fontSize = 17;
          buttonStyle.richText = true;
          buttonStyle.wordWrap = true;
        }

        if (iconStyle == null)
        {
          iconStyle = new GUIStyle(GUI.skin.label);
          iconStyle.alignment = TextAnchor.MiddleCenter;
        }

        if (scrollStyle == null)
        {
          scrollStyle = new GUIStyle(GUI.skin.box);
          scrollStyle.richText = true;
          scrollStyle.wordWrap = true;
        }

        GUILayout.BeginVertical("box");
        {
          const float space = 5.0f;

          if (headerTexture != null)
          {
            Rect headerRect = new Rect(8.0f, 8.0f, 684.0f, 190.0f);
            GUI.DrawTexture(headerRect, headerTexture, ScaleMode.ScaleAndCrop, false);

            GUILayout.Space(200.0f);
          }

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.Space(space);

            GUILayout.TextArea(description, descriptionStyle, GUILayout.ExpandWidth(true), GUILayout.Height(60.0f));

            GUILayout.Space(space);
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.BeginVertical();
            {
              if (GUILayout.Button(" Online <b>documentation</b>.", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(VintageBase.Documentation);

              if (GUILayout.Button(" Check out <b>our assets</b>.", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(publisherURL);

              if (GUILayout.Button(" <b>Rate</b> 'Vintage', thanks!", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(storeURL);
            }
            GUILayout.EndVertical();

            scroll = GUILayout.BeginScrollView(scroll, scrollStyle, GUILayout.Width(this.minSize.x * 0.5f));
            {
              GUILayout.Label(changelog);
            }
            GUILayout.EndScrollView();
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.TextArea("<size=12>Any question or suggestion? Do not hesitate to send us an email to '<b>hello@nephasto.com</b>'.</size>", descriptionStyle, GUILayout.ExpandHeight(true));

            GUILayout.BeginHorizontal(GUILayout.Width(this.minSize.x * 0.5f));
            {
              if (GUILayout.Button(forumTexture, iconStyle) == true)
                Application.OpenURL(forumURL);

              if (GUILayout.Button(twitterTexture, iconStyle) == true)
                Application.OpenURL(twitterURL);

              if (GUILayout.Button(facebookTexture, iconStyle) == true)
                Application.OpenURL(facebookURL);
            }
            GUILayout.EndHorizontal();
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);
        }
        GUILayout.EndVertical();
      }
    }

    /// <summary>
    /// About window post-processor.
    /// </summary>
    public sealed class VintageAboutProcessor : AssetPostprocessor
    {
      static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
      {
        string version = "1.0";

        for (int i = 0; i < importedAssets.Length; ++i)
        {
          if (importedAssets[i].Contains("Vintage/Changelog.txt") == true && importedAssets[i].EndsWith(".meta") == false)
          {
            string assetPath = VintageAbout.GetAssetPath();
            if (string.IsNullOrEmpty(assetPath) == false)
            {
              TextAsset changeLogAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + "Changelog.txt");
              if (changeLogAsset != null && string.IsNullOrEmpty(changeLogAsset.text) == false)
              {
                version = changeLogAsset.text.Split(new char[] { '\n', '\r' })[0];

                if (EditorPrefs.GetString(VintageAbout.assetID, "0.0") != version)
                  VintageAbout.Open();

                break;
              }
            }
          }
        }
      }
    }
  }
}
