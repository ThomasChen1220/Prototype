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
    /// Hefe slightly increases saturation and gives a warm fuzzy tone to your game.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Hefe")]
    public sealed class VintageHefe : VintageBase
    {
      private Texture2D edgeBurnTex;
      private Texture2D levelsTex;
      private Texture2D gradientTex;
      private Texture2D softLightTex;

      private static readonly int variableEdgeBurnTex = Shader.PropertyToID("_EdgeBurnTex");
      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");
      private static readonly int variableGradientTex = Shader.PropertyToID("_GradientTex");
      private static readonly int variableSoftLightTex = Shader.PropertyToID("_SoftLightTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Hefe slightly increases saturation and gives a warm fuzzy tone to your game.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        edgeBurnTex = LoadTextureFromResources("Textures/edgeBurn");
        levelsTex = LoadTextureFromResources("Textures/hefeMap");
        gradientTex = LoadTextureFromResources("Textures/hefeGradientMap");
        softLightTex = LoadTextureFromResources("Textures/hefeSoftLight");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
          material.SetTexture(variableEdgeBurnTex, edgeBurnTex);
          material.SetTexture(variableLevelsTex, levelsTex);
          material.SetTexture(variableGradientTex, gradientTex);
          material.SetTexture(variableSoftLightTex, softLightTex);
      }
    }
  }
}