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
    /// Use Earlybird to get a retro 'Polaroid' feel with soft faded colors and a hint of yellow.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Earlybird")]
    public sealed class VintageEarlybird : VintageBase
    {
      private Texture2D curvesTex;
      private Texture2D overlayTex;
      private Texture2D blowoutTex;
      private Texture2D levelsTex;

      private static readonly int variableBlowoutTex = Shader.PropertyToID("_BlowoutTex");
      private static readonly int variableOverlayTex = Shader.PropertyToID("_OverlayTex");
      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");
      private static readonly int variableCurvesTex = Shader.PropertyToID("_CurvesTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Use Earlybird to get a retro 'Polaroid' feel with soft faded colors and a hint of yellow.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        curvesTex = LoadTextureFromResources("Textures/earlyBirdCurves");
        overlayTex = LoadTextureFromResources("Textures/earlybirdOverlayMap");
        blowoutTex = LoadTextureFromResources("Textures/earlybirdBlowout");
        levelsTex = LoadTextureFromResources("Textures/earlybirdMap");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableCurvesTex, curvesTex);
        material.SetTexture(variableOverlayTex, overlayTex);
        material.SetTexture(variableBlowoutTex, blowoutTex);
        material.SetTexture(variableLevelsTex, levelsTex);
      }
    }
  }
}