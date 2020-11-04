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
    /// The Lomofi efect gives your game a dreamy, blurry effect and saturated colors.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Lomofi")]
    public sealed class VintageLomofi : VintageBase
    {
      private Texture2D levelsTex;

      private static readonly int variableLevelsTex = Shader.PropertyToID("_LevelsTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "The Lomofi efect gives your game a dreamy, blurry effect and saturated colors.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        levelsTex = LoadTextureFromResources("Textures/lomoMap");
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