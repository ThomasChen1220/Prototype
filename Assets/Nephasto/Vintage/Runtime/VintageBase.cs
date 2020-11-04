///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Base effect.
    /// </summary>
    [HelpURL(Documentation)]
    public abstract class VintageBase : MonoBehaviour
    {
      /// <summary>
      /// Strength of the effect [0, 1]. Default 1.
      /// </summary>
      public float Strength
      {
        get { return strength; }
        set { if (value.Equals(strength) == false) { strength = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Screen or Layer mode. Default Screen.
      /// </summary>
      public EffectModes EffectMode
      {
        get { return effectMode; }
        set
        {
          if (value != effectMode)
          {
            effectMode = value;

            if (effectMode == EffectModes.Screen)
            {
              this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;

              DestroyDepthCamera();
            }
            else
            {
              this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

              CreateDepthCamera();
            }

            needUpdateValues = true;
          }
        }
      }

      /// <summary>
      /// The layer to which the effect affects. Used in EffectMode.Layer. Default 'Everything'.
      /// </summary>
      public LayerMask Layer
      {
        get { return layer; }
        set
        {
          if (value != layer)
          {
            layer = value;

            if (renderDepth != null)
              renderDepth.layer = layer;

            needUpdateValues = true;
          }
        }
      }

      /// <summary>
      /// Accuracy of depth texture [0.0, 0.01]. Used in EffectMode.Layer. Default 0.004.
      /// </summary>
      public float DepthThreshold
      {
        get { return depthThreshold; }
        set { if (value.Equals(depthThreshold) == false) { depthThreshold = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Effect strength curve. Used in EffectMode.Distance.
      /// </summary>
      public AnimationCurve DistanceCurve
      {
        get { return distanceCurve; }
        set
        {
          if (value != distanceCurve)
          {
            distanceCurve = value;

            UpdateDistanceCurveTexture();

            needUpdateValues = true;
          }
        }
      }

      /// <summary>
      /// Enable color controls (Brightness, Contrast, Gamma, Hue and Saturation).
      /// </summary>
      public bool EnableColorControls
      {
        get { return enableColorControls; }
        set { if (value != enableColorControls) { enableColorControls = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Brightness [-1.0, 1.0]. Default 0.
      /// </summary>
      public float Brightness
      {
        get { return brightness; }
        set { if (value.Equals(brightness) == false) { brightness = Mathf.Clamp(value, -1.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Contrast [-1.0, 1.0]. Default 0.
      /// </summary>
      public float Contrast
      {
        get { return contrast; }
        set { if (value.Equals(contrast) == false) { contrast = Mathf.Clamp(value, -1.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Gamma [0.1, 10.0]. Default 1.
      /// </summary>
      public float Gamma
      {
        get { return gamma; }
        set { if (value.Equals(gamma) == false) { gamma = Mathf.Clamp(value, 0.1f, 10.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// The color wheel [0.0, 1.0]. Default 0.
      /// </summary>
      public float Hue
      {
        get { return hue; }
        set { if (value.Equals(hue) == false) { hue = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Intensity of a colors [0.0, 2.0]. Default 1.
      /// </summary>
      public float Saturation
      {
        get { return saturation; }
        set { if (value.Equals(saturation) == false) { saturation = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Enable old film effect.
      /// </summary>
      public bool EnableFilm
      {
        get { return enableFilm; }
        set { if (value != enableFilm) { enableFilm = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Natural vignette [0, 2]. Default 0.1.
      /// </summary>
      public float FilmVignette
      {
        get { return filmVignette; }
        set { if (value.Equals(filmVignette) == false) { filmVignette = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Grain [0.0 - 1.0]. Default 0.2.
      /// </summary>
      public float FilmGrainStrength
      {
        get { return filmGrainStrength; }
        set { if (value.Equals(filmGrainStrength) == false) { filmGrainStrength = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Blink strength [0.0 - 1.0]. Default 0.
      /// </summary>
      public float FilmBlinkStrenght
      {
        get { return filmBlinkStrenght; }
        set { if (value.Equals(filmBlinkStrenght) == false) { filmBlinkStrenght = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Blink speed. Default 50.
      /// </summary>
      public float FilmBlinkSpeed
      {
        get { return filmBlinkSpeed; }
        set { if (value.Equals(filmBlinkSpeed) == false) { filmBlinkSpeed = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Film scratches. [0.0 - 1.0]. Default 0.5.
      /// </summary>
      public float FilmScratches
      {
        get { return filmScratches; }
        set { if (value.Equals(filmScratches) == false) { filmScratches = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Blotches [0 - 6]. Default 3.
      /// </summary>
      public int FilmBlotches
      {
        get { return filmBlotches; }
        set { if (value != filmBlotches) { filmBlotches = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Blotch size [0.0 - 10.0]. Default 1.0.
      /// </summary>
      public float FilmBlotchSize
      {
        get { return filmBlotchSize; }
        set { if (value.Equals(filmBlotchSize) == false) { filmBlotchSize = Mathf.Clamp(value, 0.0f, 10.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Lines [0 - 8]. Default 4.
      /// </summary>
      public int FilmLines
      {
        get { return filmLines; }
        set { if (value != filmLines) { filmLines = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Lines strength [0.0 - 1.0]. Default 0.25.
      /// </summary>
      public float FilmLinesStrength
      {
        get { return filmLinesStrength; }
        set { if (value.Equals(filmLinesStrength) == false) { filmLinesStrength = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Enable CRT effect.
      /// </summary>
      public bool EnableCRT
      {
        get { return enableCRT; }
        set { if (value != enableCRT) { enableCRT = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Tiny scanlines [0.0 - 2.0]. Default 1.0.
      /// </summary>
      public float CRTScanLine
      {
        get { return crtScanLine; }
        set { if (value.Equals(crtScanLine) == false) { crtScanLine = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Slow moving scanlines [0.0 - 1.0]. Default 0.01.
      /// </summary>
      public float CRTSlowScan
      {
        get { return crtSlowScan; }
        set { if (value.Equals(crtSlowScan) == false) { crtSlowScan = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Scanline distortion [0.0 - 1.0]. Default 0.3.
      /// </summary>
      public float CRTScanDistort
      {
        get { return crtScanDistort; }
        set { if (value.Equals(crtScanDistort) == false) { crtScanDistort = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// CRT vignette [0.0 - 1.0]. Default 1.0.
      /// </summary>
      public float CRTVignette
      {
        get { return crtVignette; }
        set { if (value.Equals(crtVignette) == false) { crtVignette = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// CRT vignette aperture [0.0 - 1.0]. Default 1.0.
      /// </summary>
      public float CRTVignetteAperture
      {
        get { return crtVignetteAperture; }
        set { if (value.Equals(crtVignetteAperture) == false) { crtVignetteAperture = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// CRT color shift [0.0 - 1.0]. Default 0.25.
      /// </summary>
      public float CRTColorShift
      {
        get { return crtColorShift; }
        set { if (value.Equals(crtColorShift) == false) { crtColorShift = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// CRT top reflexion of the screen.
      /// </summary>
      public float CRTReflexionShine
      {
        get { return crtReflexionShine; }
        set { if (value.Equals(crtReflexionShine) == false) { crtReflexionShine = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// CRT ambient reflexion of the screen.
      /// </summary>
      public float CRTReflexionAmbient
      {
        get { return crtReflexionAmbient; }
        set { if (value.Equals(crtReflexionAmbient) == false) { crtReflexionAmbient = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      [SerializeField]
      private float strength = 1.0f;

      [SerializeField]
      private EffectModes effectMode = EffectModes.Screen;

      [SerializeField]
      private bool enableColorControls = false;

      [SerializeField]
      private float brightness = 0.0f;

      [SerializeField]
      private float contrast = 0.0f;

      [SerializeField]
      private float gamma = 1.0f;

      [SerializeField]
      private float hue = 0.0f;

      [SerializeField]
      private float saturation = 1.0f;

      [SerializeField]
      private bool enableFilm = false;

      [SerializeField]
      private float filmVignette = 0.1f;

      [SerializeField]
      private float filmGrainStrength = 0.2f;

      [SerializeField]
      private float filmBlinkStrenght = 0.0f;

      [SerializeField]
      private float filmBlinkSpeed = 50.0f;

      [SerializeField]
      private float filmScratches = 0.5f;

      [SerializeField]
      private int filmBlotches = 3;

      [SerializeField]
      private float filmBlotchSize = 1.0f;

      [SerializeField]
      private int filmLines = 4;

      [SerializeField]
      private float filmLinesStrength = 0.25f;

      [SerializeField]
      private bool enableCRT = false;

      [SerializeField]
      private float crtScanLine = 1.0f;

      [SerializeField]
      private float crtSlowScan = 0.01f;

      [SerializeField]
      private float crtScanDistort = 0.3f;
      
      [SerializeField]
      private float crtVignette = 1.0f;

      [SerializeField]
      private float crtVignetteAperture = 1.0f;

      [SerializeField]
      private float crtColorShift = 0.25f;

      [SerializeField]
      private float crtReflexionShine = 0.5f;

      [SerializeField]
      private float crtReflexionAmbient = 0.25f;

      [SerializeField]
      private LayerMask layer = -1;

      [SerializeField]
      private AnimationCurve distanceCurve = new AnimationCurve(new Keyframe(1.0f, 0.0f, 0.0f, 0.0f), new Keyframe(0.0f, 1.0f, 0.0f, 0.0f));

      [SerializeField]
      private RenderDepth renderDepth;

      [SerializeField]
      private float depthThreshold = 0.004f;

      public const string Documentation = "https://www.nephasto.com/store/vintage.html";

      protected Material material;

      private Shader shader;

      private Texture2D noiseTex;

      private Texture2D distanceTexture;

      private Camera effectCamera;

      private static readonly int variableStrength = Shader.PropertyToID("_Strength");
      private static readonly int variableRandom = Shader.PropertyToID("_RandomValue");
      private static readonly int variableNoiseTex = Shader.PropertyToID("_NoiseTex");
      private static readonly int variableVignette = Shader.PropertyToID("_Vignette");
      private static readonly int variableBrightness = Shader.PropertyToID("_Brightness");
      private static readonly int variableContrast = Shader.PropertyToID("_Contrast");
      private static readonly int variableGamma = Shader.PropertyToID("_Gamma");
      private static readonly int variableHue = Shader.PropertyToID("_Hue");
      private static readonly int variableSaturation = Shader.PropertyToID("_Saturation");
      private static readonly int variableFilmGrainStrength = Shader.PropertyToID("_FilmGrainStrength");
      private static readonly int variableFilmBlinkStrenght = Shader.PropertyToID("_FilmBlinkStrenght");
      private static readonly int variableFilmBlinkSpeed = Shader.PropertyToID("_FilmBlinkSpeed");
      private static readonly int variableFilmBlotches = Shader.PropertyToID("_FilmBlotches");
      private static readonly int variableFilmBlotchSize = Shader.PropertyToID("_FilmBlotchSize");
      private static readonly int variableFilmScratches = Shader.PropertyToID("_FilmScratches");
      private static readonly int variableFilmLines = Shader.PropertyToID("_FilmLines");
      private static readonly int variableFilmLinesStrength = Shader.PropertyToID("_FilmLinesStrength");
      private static readonly int variableRenderToTexture = Shader.PropertyToID("_RTT");
      private static readonly int variableDepthThreshold = Shader.PropertyToID("_DepthThreshold");
      private static readonly int variableScanLine = Shader.PropertyToID("_ScanLine");
      private static readonly int variableSlowScan = Shader.PropertyToID("_SlowScan");
      private static readonly int variableScanDistort = Shader.PropertyToID("_ScanDistort");
      private static readonly int variableCRTVignette = Shader.PropertyToID("_CRTVignette");
      private static readonly int variableCRTVignetteAperture = Shader.PropertyToID("_CRTVignetteAperture");
      private static readonly int variableCRTColorShift = Shader.PropertyToID("_ColorShift");
      private static readonly int variableCRTReflexionShine = Shader.PropertyToID("_ReflexionShine");
      private static readonly int variableCRTReflexionAmbient = Shader.PropertyToID("_ReflexionAmbient");
      private static readonly int variableDistanceTexture = Shader.PropertyToID("_DistanceTex");

      private const string keywordModeScreen = "MODE_SCREEN";
      private const string keywordModeLayer = "MODE_LAYER";
      private const string keywordModeDistance = "MODE_DISTANCE";

      private const string keywordColorControls = "COLOR_CONTROLS";
      private const string keywordFilm = "FILM_ENABLED";
      private const string keywordCRT = "CRT_ENABLED";

      private const string keywordLinear = "LINEAR";

      protected bool needUpdateValues = true;

      /// <summary>
      /// Effect supported by the current hardware?
      /// </summary>
      public bool IsSupported()
      {
        bool supported = false;

        string shaderPath = ShaderPath();

        Shader test = Resources.Load<Shader>(shaderPath);
        if (test != null)
        {
          supported = test.isSupported == true && CheckHardwareRequirements() == true;

          Resources.UnloadAsset(test);
        }

        if (supported == true && (effectMode == EffectModes.Layer || effectMode == EffectModes.Distance))
          supported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);

        return supported;
      }

      /// <summary>
      /// Reset to default values.
      /// </summary>
      public virtual void ResetDefaultValues()
      {
        strength = 1.0f;

        brightness = 0.0f;
        contrast = 0.0f;
        gamma = 1.0f;
        hue = 0.0f;
        saturation = 1.0f;

        filmVignette = 0.1f;
        filmGrainStrength = 0.2f;
        filmBlinkStrenght = 0.0f;
        filmBlinkSpeed = 50.0f;
        filmScratches = 0.5f;
        filmBlotches = 3;
        filmBlotchSize = 1.0f;
        filmLines = 4;
        filmLinesStrength = 0.25f;

        crtScanLine = 1.0f;
        crtSlowScan = 0.01f;
        crtScanDistort = 0.3f;
        crtVignette = 1.0f;
        crtVignetteAperture = 1.0f;
        crtColorShift = 0.25f;
        crtReflexionShine = 0.5f;
        crtReflexionAmbient = 0.25f;

        depthThreshold = 0.004f;

        distanceCurve = new AnimationCurve(new Keyframe(1.0f, 0.0f, 0.0f, 0.0f), new Keyframe(0.0f, 1.0f, 0.0f, 0.0f));

        layer = LayerMask.NameToLayer("Everything");

        needUpdateValues = true;
      }

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "No description.";

      protected virtual string ShaderPath() => $"Shaders/{this.GetType().Name}";

      private void CreateDepthCamera()
      {
        if (renderDepth == null)
        {
          GameObject go = new GameObject("VintageDepthCamera", typeof(Camera));
          go.hideFlags = HideFlags.HideAndDontSave;
          go.transform.parent = this.transform;
          go.transform.localPosition = Vector3.zero;
          go.transform.localRotation = Quaternion.identity;
          go.transform.localScale = Vector3.one;

          renderDepth = go.AddComponent<RenderDepth>();
          renderDepth.layer = layer;
        }
      }

      private void DestroyDepthCamera()
      {
        if (renderDepth != null)
        {
          GameObject obj = renderDepth.gameObject;
          renderDepth = null;

          DestroyImmediate(obj);
        }
      }

      /// <summary>
      /// Custom hardware requirements.
      /// </summary>
      protected virtual bool CheckHardwareRequirements()
      {
        if ((effectMode == EffectModes.Layer || effectMode == EffectModes.Distance) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) == false)
        {
          Debug.LogWarning("[Nephasto.Vintage] Depth textures aren't supported on this device.");

          return false;
        }

        return true;
      }

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected virtual void LoadCustomResources() { }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected virtual void UpdateCustomValues() { }

      /// <summary>
      /// Load texture from "Resources/Textures". Internal use.
      /// </summary>
      protected Texture2D LoadTextureFromResources(string texturePathFromResources)
      {
        Texture2D texture = Resources.Load<Texture2D>(texturePathFromResources);
        if (texture != null)
          texture.wrapMode = TextureWrapMode.Clamp;
        else
        {
          Debug.LogWarning($"[Nephasto.Vintage] Texture '{texturePathFromResources}' not found in 'Nephasto/Vintage/Resources/Textures' folder. Please contact with 'hello@nephasto.com' and send the log file.");

          this.enabled = false;
        }

        return texture;
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      private void UpdateValues()
      {
        if (material != null)
        {
          material.shaderKeywords = null;

          if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            material.EnableKeyword(keywordLinear);

          material.SetFloat(variableStrength, strength);
          material.SetTexture(variableNoiseTex, noiseTex);
          material.SetVector(variableRandom, new Vector4(Random.value, Random.value, Random.value, Random.value));

          switch (effectMode)
          {
            case EffectModes.Screen:
              material.EnableKeyword(keywordModeScreen);
              break;
            case EffectModes.Layer:
              material.EnableKeyword(keywordModeLayer);

              if (renderDepth != null)
                material.SetTexture(variableRenderToTexture, renderDepth.renderTexture);

              material.SetFloat(variableDepthThreshold, depthThreshold);
              break;
            case EffectModes.Distance:
              material.EnableKeyword(keywordModeDistance);

              if (distanceTexture == null)
                UpdateDistanceCurveTexture();

              material.SetTexture(variableDistanceTexture, distanceTexture);
              break;
          }

          if (enableColorControls == true && strength > 0.0f)
          {
            material.EnableKeyword(keywordColorControls);

            material.SetFloat(variableBrightness, brightness);
            material.SetFloat(variableContrast, contrast);
            material.SetFloat(variableGamma, 1.0f / gamma);
            material.SetFloat(variableHue, hue);
            material.SetFloat(variableSaturation, saturation);
          }

          if (enableFilm == true && strength > 0.0f)
          {
            material.EnableKeyword(keywordFilm);

            material.SetFloat(variableVignette, filmVignette);
            material.SetFloat(variableFilmGrainStrength, filmGrainStrength);
            material.SetFloat(variableFilmBlinkStrenght, filmBlinkStrenght * 0.1f);
            material.SetFloat(variableFilmBlinkSpeed, filmBlinkSpeed);
            material.SetFloat(variableFilmScratches, filmScratches);
            material.SetInt(variableFilmBlotches, filmBlotches);
            material.SetFloat(variableFilmBlotchSize, filmBlotchSize);
            material.SetInt(variableFilmLines, filmLines);
            material.SetFloat(variableFilmLinesStrength, filmLinesStrength / 8.0f);
          }

          if (enableCRT == true && strength > 0.0f)
          {
            material.EnableKeyword(keywordCRT);

            material.SetFloat(variableScanLine, crtScanLine);
            material.SetFloat(variableSlowScan, crtSlowScan);
            material.SetFloat(variableScanDistort, crtScanDistort * 0.1f);
            material.SetFloat(variableCRTVignette, crtVignette);
            material.SetFloat(variableCRTVignetteAperture, crtVignetteAperture);
            material.SetFloat(variableCRTColorShift, crtColorShift);
            material.SetFloat(variableCRTReflexionShine, crtReflexionShine);
            material.SetFloat(variableCRTReflexionAmbient, crtReflexionAmbient);
          }

          UpdateCustomValues();
#if !UNITY_EDITOR
          needUpdateValues = false;
#endif
        }
      }

      private void UpdateDistanceCurveTexture()
      {
        if (distanceTexture == null)
        {
          distanceTexture = new Texture2D(1024, 2)
          {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            anisoLevel = 1
          };
        }

        float step = 1.0f / (float)distanceTexture.width;
        for (int i = 0; i < distanceTexture.width; ++i)
        {
          Color color = Color.white * Mathf.Clamp01(distanceCurve.Evaluate(i * step));
          
          distanceTexture.SetPixel(i, 0, color);
          distanceTexture.SetPixel(i, 1, color);
        }

        distanceTexture.Apply();
      }

      /// <summary>
      /// Called on the frame when a script is enabled just before any of the Update methods is called the first time.
      /// </summary>
      private void Start()
      {
        string shaderPath = ShaderPath();

        shader = Resources.Load<Shader>(shaderPath);
        if (shader != null)
        {
          if (shader.isSupported == true && CheckHardwareRequirements() == true)
          {
            string materialName = this.GetType().Name;

            material = new Material(shader);
            if (material != null)
            {
              material.name = materialName;
              material.hideFlags = HideFlags.HideAndDontSave;

              noiseTex = LoadTextureFromResources("Textures/Noise256");

              LoadCustomResources();
            }
            else
            {
              Debug.LogError($"[Nephasto.Vintage] '{materialName}' material null. Please contact with 'hello@nephasto.com' and send the log file.");

              this.enabled = false;
            }
          }
          else
          {
            Debug.LogWarning($"[Nephasto.Vintage] '{shaderPath}' shader not supported. Please contact with 'hello@nephasto.com' and send the log file.");

            this.enabled = false;
          }
        }
        else
        {
          Debug.LogWarning($"[Nephasto.Vintage] Shader 'Nephasto/Vintage/Resources/{shaderPath}.shader' not found. '{this.GetType().Name}' disabled.");

          this.enabled = false;
        }
      }

      /// <summary>
      /// Called when the object becomes enabled and active.
      /// </summary>
      private void OnEnable()
      {
        if (effectMode != EffectModes.Screen && renderDepth == null)
          CreateDepthCamera();

        effectCamera = this.GetComponent<Camera>();
        effectCamera.depthTextureMode = (effectMode == EffectModes.Screen ? DepthTextureMode.None : DepthTextureMode.Depth);

        needUpdateValues = true;
      }

      /// <summary>
      /// When the MonoBehaviour will be destroyed.
      /// </summary>
      protected virtual void OnDestroy()
      {
        if (material != null)
#if UNITY_EDITOR
          DestroyImmediate(material);
#else
				  Destroy(material);
#endif
      }

      /// <summary>
      /// Called after all rendering is complete to render image.
      /// </summary>
      private void OnRenderImage(RenderTexture source, RenderTexture destination)
      {
        if (material != null && strength > 0.0f)
        {
          if (needUpdateValues == true)
            UpdateValues();
          
          Graphics.Blit(source, destination, material, 0);
        }
        else
          Graphics.Blit(source, destination);
      }
#if false
      private void OnGUI()
      {
        string label = $"{this.GetType().ToString().Replace("Nephasto.VintageAsset.Vintage", string.Empty)}: {this.ToString()}";
        Rect rect = new Rect(5, Screen.height - 50.0f, Screen.width - 5.0f, 150);

        GUI.color = Color.black;
        GUI.Label(rect, label, new GUIStyle(GUI.skin.label) { fontSize = 18, wordWrap = true, margin = new RectOffset(10, 10, 10, 10) });

        GUI.color = Color.white;
        rect.x -= 2.0f;
        rect.y -= 2.0f;
        GUI.Label(rect, label, new GUIStyle(GUI.skin.label) { fontSize = 18, wordWrap = true, margin = new RectOffset(10, 10, 10, 10) });
      }
#endif
    }
  }
}