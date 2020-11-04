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
    /// Simulates the classic Sinclair ZX Spectrum from the 80s.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage ZX Spectrum")]
    public sealed class VintageZXSpectrum : VintageBase
    {
      /// <summary>
      /// Simulation modes.
      /// </summary>
      public enum Modes
      {
        /// <summary>
        /// Full simulation.
        /// </summary>
        Full,
        
        /// <summary>
        /// Only colors and dither (faster but less realistic).
        /// </summary>
        OnlyColors,
      }

      /// <summary>
      /// Simulation mode. Default Full.
      /// </summary>
      public Modes Mode
      {
        get { return mode; }
        set { if (value != mode) { mode = value; needUpdateValues = true; } }
      }
      
      /// <summary>
      /// Resolution [1 - 10]. Default 4.
      /// </summary>
      public int PixelSize
      {
        get { return pixelSize; }
        set { if (value != pixelSize) { pixelSize = value > 0 ? value : 1; needUpdateValues = true; } }
      }

      /// <summary>
      /// Dither amount [0.0 - 1.0]. Default 1.
      /// </summary>
      public float Dither
      {
        get { return dither; }
        set { if (value.Equals(dither) == false) { dither = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Color brightness full level [0.0 - 1.0]. Default 1.
      /// </summary>
      public float BrightnessFull
      {
        get { return brightnessFull; }
        set { if (value.Equals(brightnessFull) == true) { brightnessFull = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Color brightness half level [0.0 - 1.0]. Default 0.8.
      /// </summary>
      public float BrightnessHalf
      {
        get { return brightnessHalf; }
        set { if (value.Equals(brightnessHalf) == false) { brightnessHalf = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Adjust input gamma [0.0 - ...]. Default 1.
      /// </summary>
      public float AdjustGamma
      {
        get { return adjustGamma; }
        set { if (value.Equals(adjustGamma) == false) { adjustGamma = Mathf.Max(value, 0.0f); needUpdateValues = true; } }
      }

      [SerializeField]
      private Modes mode = Modes.Full;
      
      [SerializeField]
      private int pixelSize = 2;

      [SerializeField]
      private float dither = 1.0f;

      [SerializeField]
      private float brightnessFull = 1.0f;

      [SerializeField]
      private float brightnessHalf = 0.5f;

      [SerializeField]
      private float adjustGamma = 1.0f;

      private const string variablePixelSize = "_PixelSize";
      private const string variableDither = "_Dither";
      private const string variableBrightnessFull = "_BrightnessFull";
      private const string variableBrightnessHalf = "_BrightnessHalf";
      private const string variableAdjustGamma = "_AdjustGamma";
      
      private const string keywordFull = "FULL";
      
      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Simulates the classic Sinclair ZX Spectrum from the 80s.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        pixelSize = 2;
        dither = 1.0f;
        brightnessFull = 1.0f;
        brightnessHalf = 0.5f;
        adjustGamma = 1.0f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        if (mode == Modes.Full)
          material.EnableKeyword(keywordFull);

        material.SetFloat(variablePixelSize, pixelSize);
        material.SetFloat(variableDither, dither);
        material.SetFloat(variableBrightnessFull, brightnessFull);
        material.SetFloat(variableBrightnessHalf, brightnessHalf);
        material.SetFloat(variableAdjustGamma, adjustGamma);
      }
    }
  }
}