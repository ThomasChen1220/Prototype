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
    /// Simulates an old CRT monitor.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Old CRT")]
    public sealed class VintageOldCRT : VintageBase
    {
      /// <summary>
      /// Screen coordinate offset. Default (0, 0).
      /// </summary>
      public Vector2 Offset
      {
        get { return offset; }
        set { if (value.Equals(offset) == false) { offset = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Screen curvature [0.0 - 1.0]. Default 0.2.
      /// </summary>
      public float Barrel
      {
        get { return barrel; }
        set { if (value.Equals(barrel) == false) { barrel = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Angular distortion scale [0.0 - 1.0]. Default 0.
      /// </summary>
      public float NoiseSinScale
      {
        get { return noiseSinScale; }
        set { if (value.Equals(noiseSinScale) == false) { noiseSinScale = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Angular distortion width [0.0 - 30.0]. Default 0.
      /// </summary>
      public float NoiseSinWidth
      {
        get { return noiseSinWidth; }
        set { if (value.Equals(noiseSinWidth) == false) { noiseSinWidth = Mathf.Clamp(value, 0.0f, 30.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Angular distortion offset [-10 - 10]. Default 0.
      /// </summary>
      public float NoiseSinOffset
      {
        get { return noiseSinOffset; }
        set { if (value.Equals(noiseSinOffset) == false) { noiseSinOffset = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Redraw bar size [0.0 - 2.0]. Default 1.5.
      /// </summary>
      public float ScanlineTail
      {
        get { return scanlineTail; }
        set { if (value.Equals(scanlineTail) == false) { scanlineTail = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Redraw bar speed [-10.0 - 10.0]. Default 10.
      /// </summary>
      public float ScanlineTailSpeed
      {
        get { return scanlineTailSpeed; }
        set { if (value.Equals(scanlineTailSpeed) == false) { scanlineTailSpeed = Mathf.Clamp(value, -10.0f, 10.0f); needUpdateValues = true; } }
      }

      /// <summary>
      /// Horizontal noise of the signal [0.0 - 1.0]. Default 0.
      /// </summary>
      public float NoiseX
      {
        get { return noiseX; }
        set { if (value.Equals(noiseX) == false) { noiseX = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Signal noise [0.0 - 1.0]. Default 0.
      /// </summary>
      public float NoiseRGB
      {
        get { return noiseRGB; }
        set { if (value.Equals(noiseRGB) == false) { noiseRGB = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      private static readonly int variableOffset = Shader.PropertyToID("_Offset");
      private static readonly int variableBarrel = Shader.PropertyToID("_Barrel");
      private static readonly int variableScanlineTail = Shader.PropertyToID("_ScanLineTail");
      private static readonly int variableScanlineSpeed = Shader.PropertyToID("_ScanLineSpeed");
      private static readonly int variableNoiseX = Shader.PropertyToID("_NoiseX");
      private static readonly int variableNoiseRGB = Shader.PropertyToID("_RGBNoise");
      private static readonly int variableNoiseSinScale = Shader.PropertyToID("_SinNoiseScale");
      private static readonly int variableNoiseSinWidth = Shader.PropertyToID("_SinNoiseWidth");
      private static readonly int variableNoiseSinOffset = Shader.PropertyToID("_SinNoiseOffset");

      [SerializeField]
      private Vector2 offset;

      [SerializeField]
      private float barrel = 0.2f;

      [SerializeField]
      private float scanlineTail = 1.5f;

      [SerializeField]
      private float scanlineTailSpeed = 10.0f;

      [SerializeField]
      private float noiseX = 0.0f;

      [SerializeField]
      private float noiseRGB = 0.0f;

      [SerializeField]
      private float noiseSinScale = 0.0f;

      [SerializeField]
      private float noiseSinWidth = 0.0f;

      [SerializeField]
      private float noiseSinOffset = 0.0f;

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Simulates an old CRT monitor.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        offset = Vector2.zero;

        barrel = 0.2f;

        scanlineTail = 1.5f;
        scanlineTailSpeed = 10.0f;

        noiseX = 0.0f;
        noiseRGB = 0.0f;
        noiseSinScale = 0.0f;
        noiseSinWidth = 0.0f;
        noiseSinOffset = 0.0f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetVector(variableOffset, offset);
        material.SetFloat(variableBarrel, barrel);

        material.SetFloat(variableScanlineTail, scanlineTail);
        material.SetFloat(variableScanlineSpeed, scanlineTailSpeed);

        material.SetFloat(variableNoiseX, noiseX);
        material.SetFloat(variableNoiseRGB, noiseRGB);
        material.SetFloat(variableNoiseSinScale, noiseSinScale);
        material.SetFloat(variableNoiseSinWidth, noiseSinWidth);
        material.SetFloat(variableNoiseSinOffset, noiseSinOffset);
      }
    }
  }
}