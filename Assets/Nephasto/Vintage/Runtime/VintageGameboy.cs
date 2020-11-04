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
    /// The colors of the popular Nintendo portable console from 1988.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Gameboy")]
    public sealed class VintageGameboy : VintageBase
    {
      /// <summary>
      /// Luminosity [0.0 - 1.0]. Default 1.0.
      /// </summary>
      public float Luminosity
      {
        get { return luminosity; }
        set { if (value.Equals(luminosity) == false) { luminosity = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Threshold of the palette [0.0 - 2.0]. Default 1.0.
      /// </summary>
      public float Threshold
      {
        get { return threshold; }
        set { if (value.Equals(threshold) == false) { threshold = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      private static readonly int variableLuminosity = Shader.PropertyToID("_Luminosity");
      private static readonly int variableThreshold = Shader.PropertyToID("_Threshold");

      [SerializeField]
      private float luminosity = 1.0f;

      [SerializeField]
      private float threshold = 1.0f;

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "The colors of the popular Nintendo portable console from 1988.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        luminosity = 1.0f;
        threshold = 1.0f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetFloat(variableLuminosity, luminosity);
        material.SetFloat(variableThreshold, threshold);
      }
    }
  }
}