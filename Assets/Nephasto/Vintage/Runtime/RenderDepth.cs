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
    /// Internal use only. Used in the Layer and Distance modes.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public sealed class RenderDepth : MonoBehaviour
    {
      [HideInInspector]
      public int layer = -1;

      [HideInInspector]
      public RenderTexture renderTexture;

      private Shader shader;

      private Material material;

      private Camera depthCamera;

      private Camera parentCamera;

      private Material Material
      {
        get
        {
          if (material == null)
          {
            material = new Material(shader);
            if (material != null)
              material.hideFlags = HideFlags.HideAndDontSave;
            else
            {
              Debug.LogWarning("[Nephasto.Vintage] 'RenderDepth' material null. Please contact with 'hello@nephasto.com' and send the log file.");

              this.enabled = false;
            }
          }

          return material;
        }
      }

      private void Start()
      {
        depthCamera = GetComponent<Camera>();
        depthCamera.cullingMask = layer;
        depthCamera.depthTextureMode = DepthTextureMode.Depth;
        depthCamera.clearFlags = CameraClearFlags.Color;
        depthCamera.backgroundColor = Color.black;

        shader = Resources.Load<Shader>("Shaders/RenderDepth");
        if (shader != null)
        {
          if (shader.isSupported == true)
          {
            parentCamera = transform.parent.GetComponent<Camera>();

            CreateRenderTexture();
          }
          else
          {
            Debug.LogWarning("[Nephasto.Vintage] 'RenderDepth' shader not supported. Please contact with 'hello@nephasto.com' and send the log file.");

            this.enabled = false;
          }
        }
        else
        {
          Debug.LogWarning($"[Nephasto.Vintage] Shader 'Nephasto/Vintage/Resources/Shaders/RenderDepth.shader' not found. '{this.GetType().Name}' disabled.");

          this.enabled = false;
        }
      }

      /// <summary>
      /// Destroy the material.
      /// </summary>
      private void OnDisable()
      {
        if (material != null)
          DestroyImmediate(material);

        if (depthCamera != null)
          depthCamera.targetTexture = null;
      }

      private void Update()
      {
        if (MustCreateRenderTexture() == true)
          CreateRenderTexture();
      }

      private void LateUpdate()
      {
        if (depthCamera != null && parentCamera != null && renderTexture != null)
        {
          depthCamera.CopyFrom(parentCamera);

          depthCamera.cullingMask = layer;
          depthCamera.depthTextureMode = DepthTextureMode.Depth;
          depthCamera.clearFlags = CameraClearFlags.Color;
          depthCamera.backgroundColor = Color.black;
          depthCamera.targetTexture = renderTexture;
        }
      }

      private void OnRenderImage(RenderTexture source, RenderTexture destination)
      {
        if (Material != null)
          Graphics.Blit(source, destination, material);
      }

      private bool MustCreateRenderTexture()
      {
        if (renderTexture == null)
          return true;

        return renderTexture.IsCreated() == false || (Screen.width != renderTexture.width) || (Screen.height != renderTexture.height);
      }

      private void CreateRenderTexture()
      {
        if (Screen.width > 0 && Screen.height > 0)
        {
          renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
          if (renderTexture != null)
          {
            renderTexture.isPowerOfTwo = false;
            renderTexture.antiAliasing = 1;
            renderTexture.name = "RenderTexture from Vintage";
            if (renderTexture.Create() == true)
              depthCamera.targetTexture = renderTexture;
            else
            {
              Debug.LogError($"[Nephasto.Vintage] Hardware not support Render-To-Texture, '{this.GetType()}' disabled.");

              this.enabled = false;
            }
          }
          else
          {
            Debug.LogError($"[Nephasto.Vintage] RenderTexture null, hardware may not support Render-To-Texture, '{this.GetType()}' disabled.");

            this.enabled = false;
          }
        }
        else
        {
          Debug.LogError($"[Nephasto.Vintage] Wrong screen resolution '{Screen.width}x{Screen.height}', '{this.GetType().ToString()}' disabled.");

          this.enabled = false;
        }
      }
    }
  }
}