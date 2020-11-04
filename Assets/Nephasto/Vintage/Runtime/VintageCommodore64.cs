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
    /// Simulates the palette of the Commodore64 graphic chip (VIC-II).
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Commodore 64")]
    public sealed class VintageCommodore64 : VintageBase
    {
      /// <summary>
      /// Pixel size [1 - 25]. Default 4.
      /// </summary>
      public int PixelSize
      {
        get { return pixelSize; }
        set { if (value != pixelSize) { pixelSize = (value < 1) ? 1 : ((value > 25) ? 25 : value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Dither saturation [-2.0 - 2.0]. Default 1.0.
      /// </summary>
      public float DitherSaturation
      {
        get { return ditherSaturation; }
        set { if (value.Equals(ditherSaturation) == false) { ditherSaturation = Mathf.Clamp(value, -2.0f, 2.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Dither noise [0.0 - 1.0]. Default 1.0.
      /// </summary>
      public float DitherNoise
      {
        get { return ditherNoise; }
        set { if (value.Equals(ditherNoise) == false) { ditherNoise = Mathf.Clamp(value, 0.0f, 1.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Threshold of the palette [0.0 - 2.0]. Default 1.0.
      /// </summary>
      public float Threshold
      {
        get { return threshold; }
        set { if (value.Equals(threshold) == false) { threshold = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      private static readonly int variablePixelSize = Shader.PropertyToID("_PixelSize");
      private static readonly int variableSaturation = Shader.PropertyToID("_DitherSaturation");
      private static readonly int variableNoise = Shader.PropertyToID("_DitherNoise");
      private static readonly int variableThreshold = Shader.PropertyToID("_Threshold");

      [SerializeField]
      private int pixelSize = 4;

      [SerializeField]
      private float ditherSaturation = 1.0f;

      [SerializeField]
      private float ditherNoise = 1.0f;

      [SerializeField]
      private float threshold = 1.0f;

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Simulates the palette of the Commodore64 graphic chip (VIC-II).";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        pixelSize = 4;
        ditherSaturation = 1.0f;
        ditherNoise = 1.0f;
        threshold = 1.0f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetFloat(variablePixelSize, pixelSize * 1.0f);
        material.SetFloat(variableSaturation, ditherSaturation);
        material.SetFloat(variableNoise, ditherNoise);
        material.SetFloat(variableThreshold, threshold);
      }
    }
  }
}