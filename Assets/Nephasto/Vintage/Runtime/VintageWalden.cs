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
    /// Gives your game washed-out, bluish colors and adds a slight corner vignetting.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Walden")]
    public sealed class VintageWalden : VintageBase
    {
      private Texture2D levelsTex;

      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Gives your game washed-out, bluish colors and adds a slight corner vignetting.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        levelsTex = LoadTextureFromResources("Textures/waldenMap");
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