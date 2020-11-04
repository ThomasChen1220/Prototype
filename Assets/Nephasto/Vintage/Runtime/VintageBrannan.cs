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
    /// This low-key effect brings out the grays and greens in your game.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Brannan")]
    public sealed class VintageBrannan : VintageBase
    {
      private Texture2D processTex;
      private Texture2D blowoutTex;
      private Texture2D contrastTex;
      private Texture2D lumaTex;
      private Texture2D screenTex;

      private static readonly int variableProcessTex = Shader.PropertyToID("_ProcessTex");
      private static readonly int variableBlowoutTex = Shader.PropertyToID("_BlowoutTex");
      private static readonly int variableContrastTex = Shader.PropertyToID("_ContrastTex");
      private static readonly int variableLumaTex = Shader.PropertyToID("_LumaTex");
      private static readonly int variableScreenTex = Shader.PropertyToID("_ScreenTex");

      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "This low-key effect brings out the grays and greens in your game.";

      /// <summary>
      /// Load custom resources.
      /// </summary>
      protected override void LoadCustomResources()
      {
        processTex = LoadTextureFromResources("Textures/brannanProcess");
        blowoutTex = LoadTextureFromResources("Textures/brannanBlowout");
        contrastTex = LoadTextureFromResources("Textures/brannanContrast");
        lumaTex = LoadTextureFromResources("Textures/brannanLuma");
        screenTex = LoadTextureFromResources("Textures/brannanScreen");        
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        material.SetTexture(variableProcessTex, processTex);
        material.SetTexture(variableBlowoutTex, blowoutTex);
        material.SetTexture(variableContrastTex, contrastTex);
        material.SetTexture(variableLumaTex, lumaTex);
        material.SetTexture(variableScreenTex, screenTex);        
      }
    }
  }
}