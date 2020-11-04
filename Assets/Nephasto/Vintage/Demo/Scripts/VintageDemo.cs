///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//#define DEMO_REEL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using Nephasto.VintageAsset;

/// <summary>
/// Vintage demo.
/// </summary>
public sealed class VintageDemo : MonoBehaviour
{
  [SerializeField]
  private bool guiShow = true;

  [SerializeField]
  private float slideEffectTime = 3.0f;

  [SerializeField]
  private float changeEffectTime = 0.5f;

  [SerializeField]
  private Transform transformsContainer = null;
  
  [SerializeField]
  private AudioClip musicClip = null;
  
  private bool menuOpen = false;

  private const float guiMargen = 10.0f;
  private const float guiWidth = 250.0f;

  private float updateInterval = 0.5f;
  private float accum = 0.0f;
  private int frames = 0;
  private float timeleft;
  private float fps = 0.0f;

  private GUIStyle menuStyle;
  private GUIStyle submenuStyle;
  private GUIStyle boxStyle;
  private GUIStyle descriptionStyle;

  private Vector2 scrollPosition = Vector2.zero;

  Camera selectedCamera = null;
  
  private float effectTime = 0.0f;

  private readonly List<VintageBase> vintageEffects = new List<VintageBase>();

  private int effectSelected = -1;
  private bool effectCompare = false;
  private bool effectSelectedOldFilm = false;
  private bool effectSelectedCRT = false;

  private AudioSource audioSource;
  private const float audioVolume = 0.2f;
  
  private IEnumerator changeEffectCoroutine;  

  private readonly List<Transform> transforms = new List<Transform>();
  private int tranformIndex = 0;
  
  private void OnEnable()
  {
    Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
    
    Shader.DisableKeyword("DEMO");

    for (int i = 0; i < cameras.Length && selectedCamera == null; ++i)
    {
      if (cameras[i].enabled == true)
        selectedCamera = cameras[i];
    }

    if (selectedCamera != null)
    {
      VintageBase[] effects = selectedCamera.gameObject.GetComponents<VintageBase>();
      if (effects.Length > 0)
      {
        for (int i = 0; i < effects.Length; ++i)
        {
          if (effects[i].IsSupported() == true)
            vintageEffects.Add(effects[i]);
          else
            effects[i].enabled = false;
        }
      }
      else
      {
        Type[] types = Assembly.GetAssembly(typeof(VintageBase)).GetTypes();
        for (int i = 0; i < types.Length; ++i)
        {
          if (types[i].IsClass == true && types[i].IsAbstract == false && (types[i].IsSubclassOf(typeof(VintageBase)) == true || types[i].IsSubclassOf(typeof(VintageLutBase)) == true))
          {
            VintageBase vintageEffect = selectedCamera.gameObject.AddComponent(types[i]) as VintageBase;
            if (vintageEffect.IsSupported() == true)
            {
              vintageEffects.Add(vintageEffect);

              if (vintageEffect.GetType() == typeof(VintageOldCRT))
                selectedCamera.gameObject.AddComponent<OldCRTRandomizer>();
            }
            else
              Destroy(vintageEffect);
          }
        }
      }

      Shader.WarmupAllShaders();

      for (int i = 0; i < vintageEffects.Count; ++i)
        vintageEffects[i].enabled = false;

      if (transformsContainer != null)
      {
        Transform[] points = transformsContainer.gameObject.GetComponentsInChildren<Transform>();

        if (points.Length > 1)
        {
          for (int i = 1; i < points.Length; ++i)
            transforms.Add(points[i]);

          selectedCamera.gameObject.transform.position = transforms[0].localPosition;
          selectedCamera.gameObject.transform.rotation = transforms[0].localRotation;
        }
      }
      
      ChangeEffect(0);

      if (musicClip != null)
      {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.volume = audioVolume;
        audioSource.Play();
      }
    }
    else
      Debug.LogWarning("No camera found.");

    this.enabled = vintageEffects.Count > 0;
  }

  private void Update()
  {
    timeleft -= Time.deltaTime;
    accum += Time.timeScale / Time.deltaTime;
    frames++;

    if (timeleft <= 0.0f)
    {
      fps = accum / frames;
      timeleft = updateInterval;
      accum = 0.0f;
      frames = 0;
    }

    if (slideEffectTime > 0.0f && vintageEffects.Count > 0)
    {
      effectTime += Time.deltaTime;
      if (effectTime >= (slideEffectTime + changeEffectTime))
      {
        ChangeEffect(effectSelected < (vintageEffects.Count - 1) ? effectSelected + 1 : 0);

        effectTime = 0.0f;
      }
    }

    if (Input.GetKeyUp(KeyCode.Tab) == true)
      guiShow = !guiShow;

    if (Input.GetKeyUp(KeyCode.KeypadPlus) == true || Input.GetKeyUp(KeyCode.KeypadMinus) == true ||
        Input.GetKeyUp(KeyCode.PageUp) == true || Input.GetKeyUp(KeyCode.PageDown) == true ||
        Input.GetKeyUp(KeyCode.LeftArrow) == true || Input.GetKeyUp(KeyCode.RightArrow) == true ||
        Input.GetKeyUp(KeyCode.UpArrow) == true || Input.GetKeyUp(KeyCode.DownArrow) == true)
    {
      int selection = 0;

      slideEffectTime = 0.0f;
      changeEffectTime = 0.5f;

      for (int i = 0; i < vintageEffects.Count; ++i)
      {
        if (vintageEffects[i].enabled == true)
        {
          selection = i;

          break;
        }
      }

      if (Input.GetKeyUp(KeyCode.KeypadPlus) == true || Input.GetKeyUp(KeyCode.PageUp) == true ||
          Input.GetKeyUp(KeyCode.RightArrow) == true || Input.GetKeyUp(KeyCode.UpArrow) == true)
      {
        selection = selection < vintageEffects.Count - 1 ? selection + 1 : 0;

        ChangeEffect(selection);
      }

      if (Input.GetKeyUp(KeyCode.KeypadMinus) == true || Input.GetKeyUp(KeyCode.PageDown) == true ||
          Input.GetKeyUp(KeyCode.LeftArrow) == true || Input.GetKeyUp(KeyCode.DownArrow) == true)
      {
        selection = selection > 0 ? selection - 1 : vintageEffects.Count - 1;

        ChangeEffect(selection);
      }
    }

#if !UNITY_WEBGL
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
#endif
  }

  private void OnGUI()
  {
    if (vintageEffects.Count == 0 || guiShow == false)
      return;

    if (menuStyle == null)
    {
      menuStyle = new GUIStyle(GUI.skin.textArea);
      menuStyle.richText = true;
      menuStyle.alignment = TextAnchor.MiddleCenter;
      menuStyle.fontSize = 24;
    }

    if (submenuStyle == null)
    {
      submenuStyle = new GUIStyle(GUI.skin.textArea);
      submenuStyle.richText = true;
      submenuStyle.alignment = TextAnchor.MiddleCenter;
      submenuStyle.fontSize = 18;
    }
    
    if (boxStyle == null)
    {
      boxStyle = new GUIStyle(GUI.skin.box);
      boxStyle.normal.background = MakeTex(2, 2, new Color(0.075f, 0.075f, 0.075f, 0.75f));
    }

    if (descriptionStyle == null)
    {
      descriptionStyle = new GUIStyle(GUI.skin.box);
      descriptionStyle.alignment = TextAnchor.MiddleCenter;
      descriptionStyle.normal.background = boxStyle.normal.background;
      descriptionStyle.wordWrap = true;
#if DEMO_REEL
      descriptionStyle.fontSize = 16;
#else
      descriptionStyle.fontSize = 32;
#endif
    }

    GUILayout.BeginVertical(GUILayout.Height(Screen.height));
    {
      GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(Screen.width));
      {
        GUILayout.Space(guiMargen);
#if !DEMO_REEL
        if (GUILayout.Button("MENU", menuStyle, GUILayout.Width(80.0f)) == true)
          menuOpen = !menuOpen;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("<<", menuStyle, GUILayout.Width(70.0f)) == true)
        {
          slideEffectTime = 0.0f;
          changeEffectTime = 0.5f;

          ChangeEffect(effectSelected > 0 ? effectSelected - 1 : vintageEffects.Count - 1);   
        }
#else
        GUILayout.FlexibleSpace();
#endif
        string vintageEffectName = "NONE";
        for (int i = 0; i < vintageEffects.Count && string.CompareOrdinal(vintageEffectName, "NONE") == 0; ++i)
          vintageEffectName = vintageEffects[i].enabled == true ? EffectName(vintageEffects[i].GetType().ToString()) : "NONE";

        if (string.IsNullOrEmpty(vintageEffectName) == false)
          GUILayout.Label(changeEffectCoroutine == null ? vintageEffectName : string.Empty, menuStyle, GUILayout.Width(200.0f));

#if !DEMO_REEL
        if (GUILayout.Button(">>", menuStyle, GUILayout.Width(70.0f)) == true)
        {
          slideEffectTime = 0.0f;
          changeEffectTime = 0.5f;

          ChangeEffect(effectSelected < vintageEffects.Count - 1 ? effectSelected + 1 : 0); 
        }
#endif
        GUILayout.FlexibleSpace();
#if !DEMO_REEL
        if (musicClip != null)
        {
          if (GUILayout.Button("MUTE", menuStyle, GUILayout.Width(80.0f)) == true)
            audioSource.volume = audioSource.volume < audioVolume ? audioVolume : 0.0f;
        }
        
        GUILayout.Space(guiMargen);

        if (fps < 24.0f)
          GUI.contentColor = Color.yellow;
        else if (fps < 15.0f)
          GUI.contentColor = Color.red;
        else
          GUI.contentColor = Color.green;

        GUILayout.Label(fps.ToString("000"), menuStyle, GUILayout.Width(60.0f));

        GUI.contentColor = Color.white;

        GUILayout.Space(guiMargen);
#endif
      }
      GUILayout.EndHorizontal();

#if !DEMO_REEL
      if (changeEffectCoroutine == null && effectSelected >= 0 && effectSelected < vintageEffects.Count)
      {
        GUILayout.BeginHorizontal();
        {
          GUILayout.FlexibleSpace();
          
          if (vintageEffects[effectSelected] is VintageCGA)
          {
            VintageCGA cga = vintageEffects[effectSelected] as VintageCGA;

            if (GUILayout.Button("Zero", submenuStyle) == true)
            {
              cga.Palette = VintageCGA.Palettes.Zero;
              slideEffectTime = 0.0f;
            }

            if (GUILayout.Button("One", submenuStyle) == true)
            {
              cga.Palette = VintageCGA.Palettes.One;
              slideEffectTime = 0.0f;
            }

            if (GUILayout.Button("Two", submenuStyle) == true)
            {
              cga.Palette = VintageCGA.Palettes.Two;
              slideEffectTime = 0.0f;
            }
          }
          else if (vintageEffects[effectSelected] is VintageTechnicolor)
          {
            VintageTechnicolor technicolor = vintageEffects[effectSelected] as VintageTechnicolor;

            if (GUILayout.Button("One", submenuStyle) == true)
            {
              technicolor.Process = VintageTechnicolor.Processes.One;
              slideEffectTime = 0.0f;
            }

            if (GUILayout.Button("Two", submenuStyle) == true)
            {
              technicolor.Process = VintageTechnicolor.Processes.Two;
              slideEffectTime = 0.0f;
            }

            if (GUILayout.Button("Three", submenuStyle) == true)
            {
              technicolor.Process = VintageTechnicolor.Processes.Three;
              slideEffectTime = 0.0f;
            }
          }
          else if (vintageEffects[effectSelected] is VintageFilmLook)
          {
            VintageFilmLook filmLook = vintageEffects[effectSelected] as VintageFilmLook;

            VintageFilmLook.Manufacturers[] manufacturerses = (VintageFilmLook.Manufacturers[])Enum.GetValues(typeof(VintageFilmLook.Manufacturers));
            for (int i = 0; i < manufacturerses.Length; ++i)
            {
              string manufacturer = manufacturerses[i].ToString();
              manufacturer = manufacturer.Replace("Ibuprogames.VintageAsset.VintageFilmLook.", string.Empty).Replace('_', ' ');

              if (GUILayout.Button(manufacturer, submenuStyle) == true)
              {
                filmLook.Manufacturer = manufacturerses[i];
                slideEffectTime = 0.0f;
              }
            }
          }
          else if (vintageEffects[effectSelected] is VintageZXSpectrum)
          {
            VintageZXSpectrum zxSpectrum = vintageEffects[effectSelected] as VintageZXSpectrum;

            if (GUILayout.Button("Full", submenuStyle) == true)
            {
              zxSpectrum.Mode = VintageZXSpectrum.Modes.Full;
              slideEffectTime = 0.0f;
            }

            if (GUILayout.Button("Color", submenuStyle) == true)
            {
              zxSpectrum.Mode = VintageZXSpectrum.Modes.OnlyColors;
              slideEffectTime = 0.0f;
            }
          }
          
          GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
      }
#endif
      if (menuOpen == true)
        MenuGUI();

      GUILayout.FlexibleSpace();

      if (changeEffectCoroutine == null)
      {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        {
          GUILayout.Space(guiMargen);

          if (effectSelected >= 0 && effectSelected < vintageEffects.Count)
            GUILayout.Label(changeEffectCoroutine == null ? vintageEffects[effectSelected].ToString() : string.Empty, descriptionStyle, GUILayout.ExpandWidth(true));

          GUILayout.Space(guiMargen);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Space(guiMargen);
      }
    }
    GUILayout.EndVertical();
  }

  private void MenuGUI()
  {
    GUILayout.BeginVertical(boxStyle, GUILayout.Width(guiWidth));
    {
      GUILayout.Space(guiMargen);

      // Common controls.
      if (effectSelected >= 0 && effectSelected < vintageEffects.Count)
      {
        GUILayout.BeginVertical(boxStyle);
        {
          effectSelectedOldFilm = GUILayout.Toggle(effectSelectedOldFilm, "  Old Film");

          effectSelectedCRT = GUILayout.Toggle(effectSelectedCRT, "  CRT");
          
          effectCompare = GUILayout.Toggle(effectCompare, "  Compare");

          if (changeEffectCoroutine == null)
          {
            VintageBase currentEffect = vintageEffects[effectSelected];
            currentEffect.EnableFilm = effectSelectedOldFilm;
            currentEffect.EnableCRT = effectSelectedCRT;

            if (effectCompare == true)
              Shader.EnableKeyword("DEMO");
            else
              Shader.DisableKeyword("DEMO");
          }
        }
        GUILayout.EndVertical();
      }

      // Effects.
      GUILayout.BeginVertical(boxStyle);
      {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, "box");
        {
          int effectChanged = -1;

          for (int i = 0; i < vintageEffects.Count; ++i)
          {
            VintageBase vintageEffect = vintageEffects[i];

            GUILayout.BeginHorizontal();
            {
              if (vintageEffect.enabled == true && i == effectSelected)
                GUILayout.BeginVertical("box");
              else
                GUILayout.BeginVertical();

              bool enableChanged = GUILayout.Toggle(vintageEffect.enabled == true && i == effectSelected, $"  {EffectName(vintageEffect.GetType().ToString())}");
              if (enableChanged != vintageEffect.enabled && changeEffectCoroutine == null && i != effectSelected)
              {
                slideEffectTime = 0.0f;
                changeEffectTime = 0.5f;

                effectChanged = i;
              }

              GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(guiMargen * 0.5f);
          }

          if (effectChanged != -1)
            ChangeEffect(effectChanged, false);
        }
        GUILayout.EndScrollView();
      }
      GUILayout.EndVertical();

      GUILayout.FlexibleSpace();

      // Options.
      GUILayout.BeginVertical(boxStyle);
      {
        GUILayout.Label("TAB - Hide/Show gui.");

        GUILayout.BeginHorizontal(boxStyle);
        {
          if (GUILayout.Button("Open Web") == true)
            Application.OpenURL(VintageBase.Documentation);

#if !UNITY_WEBGL
          if (GUILayout.Button("Quit") == true)
            Application.Quit();
#endif
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(guiMargen * 2.0f);

        GUILayout.Label("Art by Synth Studio.");
        GUILayout.Label("Music by NADA Free Music.");
      }
      GUILayout.EndVertical();
    }
    GUILayout.EndVertical();
  }
  
  private float ParametricBlend(float t)
  {
    float sqt = t * t;

    return sqt / (2.0f * (sqt - t) + 1.0f);
  }

  private IEnumerator ChangeEffectCoroutine(int effectIndex, bool changeCamera = true)
  {
    if (effectIndex >= 0 && effectIndex < vintageEffects.Count)
    {
      int oldSelected = effectSelected;
      effectSelected = effectIndex;

      vintageEffects[effectSelected].enabled = true;
      vintageEffects[effectSelected].Strength = 0.0f;
      vintageEffects[effectSelected].EnableFilm = effectSelectedOldFilm;
      vintageEffects[effectSelected].EnableCRT = effectSelectedCRT;

      if (oldSelected >= 0)
      {
        vintageEffects[oldSelected].enabled = true;
        vintageEffects[oldSelected].Strength = 1.0f;
        vintageEffects[oldSelected].EnableFilm = effectSelectedOldFilm;
        vintageEffects[oldSelected].EnableCRT = effectSelectedCRT;
      }

      if (changeCamera == true && transforms.Count > 0 && ++tranformIndex > transforms.Count - 1)
        tranformIndex = 0;

      Vector3 cameraPosition = selectedCamera.transform.position;
      Quaternion cameraRotation = selectedCamera.transform.rotation;

      float time = 0.0f;
      while (time < changeEffectTime)
      {
        float linear = time / changeEffectTime;
        float ease = ParametricBlend(linear);

        vintageEffects[effectSelected].Strength = ease;
        
        if (oldSelected >= 0)
          vintageEffects[oldSelected].Strength = 1.0f - ease;

        if (changeCamera == true && transforms.Count > 0)
        {
          selectedCamera.transform.position = Vector3.Lerp(cameraPosition, transforms[tranformIndex].localPosition, ease);
          selectedCamera.transform.rotation = Quaternion.Slerp(cameraRotation, transforms[tranformIndex].localRotation, ease);
        }
        
        time += Time.deltaTime;
          
        yield return null;
      }

      if (changeCamera == true && transforms.Count > 0)
      {
        selectedCamera.transform.position = transforms[tranformIndex].position;
        selectedCamera.transform.rotation = transforms[tranformIndex].rotation;
      }
      
      vintageEffects[effectSelected].Strength = 1.0f;
      vintageEffects[effectSelected].enabled = true;

      if (oldSelected >= 0)
      {
        vintageEffects[oldSelected].Strength = 0.0f;
        vintageEffects[oldSelected].enabled = false;
      }

      changeEffectCoroutine = null;
    }
  }
  
  private string EffectName(string effectname)
  {
    return effectname.Replace("Nephasto.VintageAsset.Vintage", string.Empty).ToUpper();
  }

  private void ChangeEffect(int effectIndex, bool changeCamera = true)
  {
    if (changeEffectCoroutine == null && effectIndex >= 0 && effectIndex < vintageEffects.Count)
      StartCoroutine(changeEffectCoroutine = ChangeEffectCoroutine(effectIndex, changeCamera));
  }

  private Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
  }
}
