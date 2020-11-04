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
    /// Inkwell adds high contrast and also makes black and white.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Inkwell")]
    public sealed class VintageInkwell : VintageBase
    {
      private Texture2D levelsTex;

      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Inkwell adds high contrast and also makes black and white.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        levelsTex = LoadTextureFromResources("Textures/inkwellMap");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableLevelsTex, levelsTex);
      }
    }
  }
}