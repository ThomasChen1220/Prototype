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
    /// Lookup texture base.
    /// </summary>
    public abstract class VintageLutBase : VintageBase
    {
      protected Texture3D lutTex3D = null;
      protected Texture2D lutTex2D = null;

      protected bool supports3DTextures = false;

      private static readonly int variableParams = Shader.PropertyToID("_LutParams");
      private static readonly int variableLutTex = Shader.PropertyToID("_LutTex");

      private static readonly string keywordLut3D = "LUT_3D";

      protected override string ShaderPath() => "Shaders/VintageLut";

      /// <summary>
      /// Custom hardware requirements.
      /// </summary>
      protected override bool CheckHardwareRequirements()
      {
        supports3DTextures = SystemInfo.supports3DTextures;

        if (SystemInfo.supports3DTextures == false)
          Debug.LogWarning($"[Nephasto.Vintage] Hardware not support 3D textures. '{this.GetType().ToString()}' using 2D luts.");

        return base.CheckHardwareRequirements();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        if (supports3DTextures == true && lutTex3D != null)
        {
          material.EnableKeyword(keywordLut3D);

          int lutSize = lutTex3D.width;

          material.SetVector(variableParams, new Vector2((lutSize - 1) / (1.0f * lutSize), 1.0f / (2.0f * lutSize)));
          material.SetTexture(variableLutTex, lutTex3D);
        }
        else if (lutTex2D != null)
        {
          float lutSize = Mathf.Sqrt(lutTex2D.width * 1.0f);

          material.SetVector(variableParams, new Vector3(1.0f / (float)lutTex2D.width, 1.0f / (float)lutTex2D.height, lutSize - 1.0f));
          material.SetTexture(variableLutTex, lutTex2D);
        }
      }

      /// <summary>
      /// Creates a 3D texture. Internal use.
      /// </summary>
      protected Texture3D CreateTexture3DFromResources(string texturePathFromResources, int slices)
      {
        Texture3D texture3D = null;

        Texture2D texture2D = Resources.Load<Texture2D>(texturePathFromResources);
        if (texture2D != null)
        {
          int height = texture2D.height;
          int width = texture2D.width / slices;

          Color[] pixels2D = texture2D.GetPixels();
          Color[] pixels3D = new Color[pixels2D.Length];

          for (int z = 0; z < slices; ++z)
            for (int y = 0; y < height; ++y)
              for (int x = 0; x < width; ++x)
                pixels3D[x + (y * width) + (z * (width * height))] = pixels2D[x + (z * width) + (((width - y) - 1) * width * height)];

          texture3D = new Texture3D(width, height, slices, TextureFormat.ARGB32, false);
          texture3D.SetPixels(pixels3D);
          texture3D.Apply();
          texture3D.filterMode = FilterMode.Bilinear;
          texture3D.wrapMode = TextureWrapMode.Clamp;
          texture3D.anisoLevel = 1;
        }
        else
        {
          Debug.LogWarning($"[Nephasto.Vintage] Texture '{texturePathFromResources}' not found in 'Ibuprogames/Vintage/Resources/Textures' folder. Please contact with 'hello@nephasto.com' and send the log file.");

          this.enabled = false;
        }

        return texture3D;
      }

      protected override void OnDestroy()
      {
        if (lutTex3D != null)
#if UNITY_EDITOR
          DestroyImmediate(lutTex3D);
#else
				  Destroy(lutTex3D);
#endif

        if (lutTex2D != null)
          Resources.UnloadAsset(lutTex2D);

        base.OnDestroy();
      }
    }
  }
}