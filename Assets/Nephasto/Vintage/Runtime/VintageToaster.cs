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
    /// Gives your game a burnt, aged look. It also adds a slight texture plus vignetting.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Toaster")]
    public sealed class VintageToaster : VintageBase
    {
      private Texture2D metalTex;
      private Texture2D softLightTex;
      private Texture2D curvesTex;
      private Texture2D overlayWarmTex;
      private Texture2D colorShiftTex;

      private static readonly int variableMetalTex = Shader.PropertyToID("_MetalTex");
      private static readonly int variableSoftLightTex = Shader.PropertyToID("_SoftLightTex");
      private static readonly int variableCurvesTex = Shader.PropertyToID("_CurvesTex");
      private static readonly int variableOverlayWarmTex = Shader.PropertyToID("_OverlayWarmTex");
      private static readonly int variableColorShiftTex = Shader.PropertyToID("_ColorShiftTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Gives your game a burnt, aged look. It also adds a slight texture plus vignetting.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        metalTex = LoadTextureFromResources("Textures/toasterMetal");
        softLightTex = LoadTextureFromResources("Textures/toasterSoftLight");
        curvesTex = LoadTextureFromResources("Textures/toasterCurves");
        overlayWarmTex = LoadTextureFromResources("Textures/toasterOverlayMapWarm");
        colorShiftTex = LoadTextureFromResources("Textures/toasterColorShift");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableMetalTex, metalTex);
        material.SetTexture(variableSoftLightTex, softLightTex);
        material.SetTexture(variableCurvesTex, curvesTex);
        material.SetTexture(variableOverlayWarmTex, overlayWarmTex);
        material.SetTexture(variableColorShiftTex, colorShiftTex);
      }
    }
  }
}