///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Vintage base editor.
    /// </summary>
    [CustomEditor(typeof(VintageBase))]
    public abstract class VintageEditorBase : Inspector
    {
      private bool displayColorControls = false;
      private bool displayFilm = false;
      private bool displayCRT = false;

      /// <summary>
      /// Custom inspector.
      /// </summary>
      protected virtual void VintageInspector()
      {
      }

      /// <summary>
      /// OnInspectorGUI.
      /// </summary>
      protected override void InspectorGUI()
      {
        VintageBase baseTarget = this.target as VintageBase;
        
        ResetGUI(0, 0.0f, 125.0f);

        Undo.RecordObject(baseTarget, baseTarget.GetType().Name);

        BeginVertical();
        {
          /////////////////////////////////////////////////
          // Common.
          /////////////////////////////////////////////////
          
          Separator();

          baseTarget.Strength = SliderField("Strength", "The strength of the effect.\nFrom 0.0 (no effect) to 1.0 (full effect).", baseTarget.Strength, 0.0f, 1.0f, 1.0f);

          baseTarget.EffectMode = (EffectModes)EnumPopupField("Mode", "Screen, Layer or Depth mode. Default Screen.", baseTarget.EffectMode, EffectModes.Screen);

          if (baseTarget.EffectMode == EffectModes.Layer)
          {
            IndentLevel++;

            baseTarget.Layer = LayerMaskField("Layer mask", baseTarget.Layer, LayerMask.NameToLayer("Everything"));

            baseTarget.DepthThreshold = SliderField("Depth threshold", "Accuracy of depth texture.", baseTarget.DepthThreshold, 0.0f, 0.01f, 0.004f);

            IndentLevel--;            
          }
          else if (baseTarget.EffectMode == EffectModes.Distance)
            baseTarget.DistanceCurve = CurveField("    Curve", baseTarget.DistanceCurve);

          /////////////////////////////////////////////////
          // Effect GUI.
          /////////////////////////////////////////////////
          
          Separator();
          
          VintageInspector();

          /////////////////////////////////////////////////
          // Color controls.
          /////////////////////////////////////////////////

          Separator();

          baseTarget.EnableColorControls = ToogleFoldout("Color", baseTarget.EnableColorControls, ref displayColorControls);
          if (displayColorControls == true)
          {
            EnableGUI = baseTarget.EnableColorControls;

            EditorGUI.indentLevel++;

            baseTarget.Brightness = SliderField("Brightness", "Brightness [-1.0, 1.0]. Default 0.", baseTarget.Brightness, -1.0f, 1.0f, 0.0f);

            baseTarget.Contrast = SliderField("Contrast", "Contrast [-1.0, 1.0]. Default 0.", baseTarget.Contrast, -1.0f, 1.0f, 0.0f);

            baseTarget.Gamma = SliderField("Gamma", "Gamma [0.1, 10.0]. Default 1.", baseTarget.Gamma, 0.01f, 10.0f, 1.0f);

            if (baseTarget.GetType() != typeof(VintageInkwell))
            {
              baseTarget.Hue = SliderField("Hue", "The color wheel [0.0, 1.0]. Default 0.", baseTarget.Hue, 0.0f, 1.0f, 0.0f);

              baseTarget.Saturation = SliderField("Saturation", "Intensity of a colors [0.0, 2.0]. Default 1.", baseTarget.Saturation, 0.0f, 2.0f, 1.0f);
            }

            EditorGUI.indentLevel--;

            EnableGUI = true;
          }

          /////////////////////////////////////////////////
          // Film.
          /////////////////////////////////////////////////
          if (baseTarget.GetType() != typeof(VintageOldCRT))
          {
            Separator();

            baseTarget.EnableFilm = ToogleFoldout("Old film", baseTarget.EnableFilm, ref displayFilm);
            if (displayFilm == true)
            {
              EnableGUI = baseTarget.EnableFilm;

              IndentLevel++;

              baseTarget.FilmVignette = SliderField("Vignette", "Natural vignette.", baseTarget.FilmVignette, 0.0f, 2.0f, 0.1f);

              baseTarget.FilmGrainStrength = SliderField("Grain", "Film grain or granularity is noise texture due to the presence of small particles.\nFrom 0.0 (no grain) to 1.0 (full grain).", baseTarget.FilmGrainStrength, 0.0f, 1.0f, 0.2f);

              baseTarget.FilmScratches = SliderField("Scratches", baseTarget.FilmScratches, 0.0f, 1.0f, 0.5f);

              // Blink.
              Label("Blink");

              IndentLevel++;

              baseTarget.FilmBlinkStrenght = SliderField("Strenght", "Brightness variation.\nFrom 0.0 (no fluctuation) to 1.0 (full epilepsy).", baseTarget.FilmBlinkStrenght, 0.0f, 1.0f, 0.0f);

              baseTarget.FilmBlinkSpeed = SliderField("Speed", baseTarget.FilmBlinkSpeed, 0.0f, 250.0f, 50.0f);

              IndentLevel--;

              // Blotches.
              Label("Blotches");

              IndentLevel++;

              baseTarget.FilmBlotches = SliderField("Count", baseTarget.FilmBlotches, 0, 6, 3);

              baseTarget.FilmBlotchSize = SliderField("Size", baseTarget.FilmBlotchSize, 0.0f, 10.0f, 1.0f);

              IndentLevel--;

              // Lines.
              Label("Lines");

              IndentLevel++;

              baseTarget.FilmLines = SliderField("Count", baseTarget.FilmLines, 0, 8, 4);

              baseTarget.FilmLinesStrength = SliderField("Strength", baseTarget.FilmLinesStrength, 0.0f, 1.0f, 0.25f);

              IndentLevel--;

              IndentLevel--;

              EnableGUI = true;
            }
          }

          /////////////////////////////////////////////////
          // CRT.
          /////////////////////////////////////////////////
          if (baseTarget.GetType() != typeof(VintageOldCRT))
          {
            Separator();

            baseTarget.EnableCRT = ToogleFoldout("CRT", baseTarget.EnableCRT, ref displayCRT);
            if (displayCRT == true)
            {
              EnableGUI = baseTarget.EnableCRT;

              IndentLevel++;

              Label("Vignette");

              IndentLevel++;

              baseTarget.CRTVignette = SliderField("Strength", baseTarget.CRTVignette, 0.0f, 1.0f, 1.0f);

              baseTarget.CRTVignetteAperture = SliderField("Aperture", baseTarget.CRTVignetteAperture, 0.0f, 1.0f, 1.0f);

              IndentLevel--;

              Label("Scanlines");

              IndentLevel++;

              baseTarget.CRTScanLine = SliderField("Lines", "Tiny scanlines.", baseTarget.CRTScanLine, 0.0f, 2.0f, 1.0f);

              baseTarget.CRTSlowScan = SliderField("Slow moving", baseTarget.CRTSlowScan, 0.0f, 1.0f, 0.01f);

              baseTarget.CRTScanDistort = SliderField("Distort bar", "Scanline distortion.", baseTarget.CRTScanDistort, 0.0f, 1.0f, 0.3f);

              IndentLevel--;

              baseTarget.CRTColorShift = SliderField("Color shift", baseTarget.CRTColorShift, 0.0f, 1.0f, 0.25f);

              Label("Reflexion");

              IndentLevel++;

              baseTarget.CRTReflexionShine = SliderField("Shine", baseTarget.CRTReflexionShine, 0.0f, 1.0f, 0.5f);

              baseTarget.CRTReflexionAmbient = SliderField("Ambient", baseTarget.CRTReflexionAmbient, 0.0f, 1.0f, 0.25f);

              IndentLevel--;

              IndentLevel--;

              EnableGUI = true;
            }
          }

          /////////////////////////////////////////////////
          // Description.
          /////////////////////////////////////////////////

          Separator();

          EditorGUILayout.HelpBox(baseTarget.ToString(), MessageType.Info);

          /////////////////////////////////////////////////
          // Misc.
          /////////////////////////////////////////////////

          Separator();

          BeginHorizontal();
          {
            if (GUILayout.Button(new GUIContent("[doc]", "Online documentation"), GUI.skin.label) == true)
              Application.OpenURL(VintageBase.Documentation);

            FlexibleSpace();

            if (Button("Reset") == true)
              baseTarget.ResetDefaultValues();
          }
          EndHorizontal();
        }
        EndVertical();

        Separator();
      }
    }
  }
}