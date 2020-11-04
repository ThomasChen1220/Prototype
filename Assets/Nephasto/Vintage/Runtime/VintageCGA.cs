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
    /// Simulates the CGA (Color Graphics Adapter) standard introduced by IBM in 1981.
    /// Use the high intensity palette: Black, ligth cyan, light magenta and white.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage CGA")]
    public sealed class VintageCGA : VintageBase
    {
      /// <summary>
      /// CGA palettes.
      /// </summary>
      public enum Palettes
      {
        /// <summary>
        /// Black, light green, light red and white.
        /// </summary>
        Zero,

        /// <summary>
        /// Black, light cyan, light magenta and white.
        /// </summary>
        One,

        /// <summary>
        /// Black, light cyan, light red and white.
        /// </summary>
        Two
      }

      /// <summary>
      /// CGA palettes.
      /// </summary>
      public Palettes Palette
      {
        get { return palette; }
        set { if (value != palette) { palette = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Pixel size [1 - 25]. Default 4.
      /// </summary>
      public int PixelSize
      {
        get { return pixelSize; }
        set { if (value != pixelSize) { pixelSize = (value < 1) ? 1 : ((value > 25) ? 25 : value); needUpdateValues = true; } }
      }

      /// <summary>
      /// Threshold of the palette [0.0 - 2.0]. Default 0.35.
      /// </summary>
      public float Threshold
      {
        get { return threshold; }
        set { if (value.Equals(threshold) == false) { threshold = Mathf.Clamp(value, 0.0f, 2.0f); needUpdateValues = true; } }
      }

      private static readonly int variableColor0 = Shader.PropertyToID("_Color0");
      private static readonly int variableColor1 = Shader.PropertyToID("_Color1");
      private static readonly int variableColor2 = Shader.PropertyToID("_Color2");
      private static readonly int variableColor3 = Shader.PropertyToID("_Color3");
      private static readonly int variablePixelSize = Shader.PropertyToID("_PixelSize");
      private static readonly int variableThreshold = Shader.PropertyToID("_Threshold");

      [SerializeField]
      private Palettes palette = Palettes.One;

      [SerializeField]
      private int pixelSize = 4;

      [SerializeField]
      private float threshold = 0.35f;

      private static readonly Color[] palette0 =
      {
        new Color(0.0f, 0.0f, 0.0f),    // Black.
        new Color(0.33f, 1.0f, 0.33f),  // Light green.
        new Color(1.0f, 0.33f, 0.33f),  // Light red.
        new Color(1.0f, 1.0f, 0.33f)    // Yellow.
      };

      private static readonly Color[] palette1 =
      {
        new Color(0.0f, 0.0f, 0.0f),    // Black.
        new Color(0.33f, 1.0f, 1.0f),   // Light cyan.
        new Color(1.0f, 0.33f, 1.0f),   // Light magenta.
        new Color(1.0f, 1.0f, 1.0f)     // White.
      };

      private static readonly Color[] palette2 =
      {
        new Color(0.0f, 0.0f, 0.0f),    // Black.
        new Color(0.33f, 1.0f, 1.0f),   // Light cyan.
        new Color(1.0f, 0.33f, 0.33f),  // Light red.
        new Color(1.0f, 1.0f, 1.0f)     // White.
      };

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Simulates the CGA (Color Graphics Adapter) standard introduced by IBM in 1981.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        palette = Palettes.One;
        pixelSize = 4;
        threshold = 0.35f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        switch (palette)
        {
          case Palettes.Zero:
            material.SetColor(variableColor0, palette0[0]);
            material.SetColor(variableColor1, palette0[1]);
            material.SetColor(variableColor2, palette0[2]);
            material.SetColor(variableColor3, palette0[3]);
            break;

          case Palettes.One:
            material.SetColor(variableColor0, palette1[0]);
            material.SetColor(variableColor1, palette1[1]);
            material.SetColor(variableColor2, palette1[2]);
            material.SetColor(variableColor3, palette1[3]);
            break;

          case Palettes.Two:
            material.SetColor(variableColor0, palette2[0]);
            material.SetColor(variableColor1, palette2[1]);
            material.SetColor(variableColor2, palette2[2]);
            material.SetColor(variableColor3, palette2[3]);
            break;
        }

        material.SetFloat(variablePixelSize, pixelSize * 1.0f);
        material.SetFloat(variableThreshold, threshold);
      }
    }
  }
}