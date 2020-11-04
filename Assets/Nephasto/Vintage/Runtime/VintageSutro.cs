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
    /// Sutro gives you Sepia-like tones, with an emphasis on purple and brown.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Sutro")]
    public sealed class VintageSutro : VintageBase
    {
      private Texture2D curvesTex;
      private Texture2D edgeTex;

      private static readonly int variableCurvesTex = Shader.PropertyToID("_CurvesTex");
      private static readonly int variableEdgeBurnTex = Shader.PropertyToID("_EdgeBurnTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Sutro gives you Sepia-like tones, with an emphasis on purple and brown.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        curvesTex = LoadTextureFromResources("Textures/sutroCurves");
        edgeTex = LoadTextureFromResources("Textures/sutroEdgeBurn");
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableCurvesTex, curvesTex);
        material.SetTexture(variableEdgeBurnTex, edgeTex);
      }
    }
  }
}