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
    /// Sierra makes the game appear softer by adding bluish tones while emphasizing darks and yellows.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Sierra")]
    public sealed class VintageSierra : VintageBase
    {
      /// <summary>
      /// Overlay strength [0.0 - 1.0]. Default 0.25.
      /// </summary>
      public float Overlay
      {
        get { return overlayStrength; }
        set { if (value.Equals(overlayStrength) == false) { overlayStrength = Mathf.Clamp01(value); needUpdateValues = true; } }
      }

      private Texture2D blowoutTex;
      private Texture2D overlayTex;
      private Texture2D levelsTex;

      private static readonly int variableBlowoutTex = Shader.PropertyToID("_BlowoutTex");
      private static readonly int variableOverlayTex = Shader.PropertyToID("_OverlayTex");
      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");
      private static readonly int variableOverlayStrength = Shader.PropertyToID("_OverlayStrength");

      [SerializeField]
      private float overlayStrength = 0.25f;

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Sierra makes the game appear softer by adding bluish tones while emphasizing darks and yellows.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        overlayStrength = 0.25f;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        blowoutTex = LoadTextureFromResources("Textures/blackboard1024");
        overlayTex = LoadTextureFromResources("Textures/overlayMap");
        levelsTex = LoadTextureFromResources("Textures/sierraMap");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableBlowoutTex, blowoutTex);
        material.SetTexture(variableLevelsTex, levelsTex);
        material.SetTexture(variableOverlayTex, overlayTex);

        material.SetFloat(variableOverlayStrength, overlayStrength);
      }
    }
  }
}