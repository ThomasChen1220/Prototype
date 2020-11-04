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
    /// Gives your game a slight faded, 1980’s touch by adding a light brown and gray tint.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Valencia")]
    public sealed class VintageValencia : VintageBase
    {
      private Texture2D levelsTex;
      private Texture2D gradTex;

      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");
      private static readonly int variableGradientLevelsTex = Shader.PropertyToID("_GradientLevelsTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Gives your game a slight faded, 1980’s touch by adding a light brown and gray tint.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        levelsTex = LoadTextureFromResources("Textures/valenciaMap");
        gradTex = LoadTextureFromResources("Textures/valenciaGradientMap");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableLevelsTex, levelsTex);
        material.SetTexture(variableGradientLevelsTex, gradTex);
      }
    }
  }
}