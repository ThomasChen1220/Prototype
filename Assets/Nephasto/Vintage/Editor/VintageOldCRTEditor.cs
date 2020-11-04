///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Vintage Old CRT editor.
    /// </summary>
    [CustomEditor(typeof(VintageOldCRT))]
    public sealed class VintageOldCRTEditor : VintageEditorBase
    {
      /// <summary>
      /// Custom inspector.
      /// </summary>
      protected override void VintageInspector()
      {
        VintageOldCRT thisTarget = (VintageOldCRT)target;

        Label("Distortions");
        {
          IndentLevel++;

          thisTarget.Offset = Vector2Field("Offset", "Screen coordinate offset. Default (0, 0).", thisTarget.Offset, Vector2.zero);
          thisTarget.Barrel = SliderField("Barrel", "Screen curvature [0.0 - 1.0]. Default 0.2.", thisTarget.Barrel, 0.0f, 1.0f, 0.2f);
          thisTarget.NoiseSinScale = SliderField("Sin scale", "Angular distortion scale [0.0 - 1.0]. Default 0.", thisTarget.NoiseSinScale, 0.0f, 1.0f, 0.0f);
          thisTarget.NoiseSinWidth = SliderField("Sin width", "Angular distortion width [0.0 - 30.0]. Default 0.", thisTarget.NoiseSinWidth, 0.0f, 30.0f, 0.0f);
          thisTarget.NoiseSinOffset = SliderField("Sin offset", "Angular distortion offset [-10 - 10]. Default 0.", thisTarget.NoiseSinOffset, -10.0f, 10.0f, 0.0f);

          IndentLevel--;
        }

        Separator();

        Label("Scanlines");
        {
          IndentLevel++;

          thisTarget.ScanlineTail = SliderField("Tail", "Redraw bar size [0.0 - 2.0]. Default 1.5.", thisTarget.ScanlineTail, 0.0f, 2.0f, 1.5f);
          thisTarget.ScanlineTailSpeed = SliderField("Speed", "Redraw bar speed [-10.0 - 10.0]. Default 10.", thisTarget.ScanlineTailSpeed, -10.0f, 10.0f, 10.0f);

          IndentLevel--;
        }

        Separator();

        Label("Noise");
        {
          IndentLevel++;

          thisTarget.NoiseX = SliderField("Horizontal", "Horizontal noise of the signal [0.0 - 1.0]. Default 0.", thisTarget.NoiseX, 0.0f, 1.0f, 0.0f);
          thisTarget.NoiseRGB = SliderField("RGB", "Signal noise [0.0 - 1.0]. Default 0.", thisTarget.NoiseRGB, 0.0f, 1.0f, 0.0f);

          IndentLevel--;
        }
      }
    }
  }
}