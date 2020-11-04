///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Vintage Commodore 64 editor.
    /// </summary>
    [CustomEditor(typeof(VintageCommodore64))]
    public sealed class VintageCommodore64Editor : VintageEditorBase
    {
      /// <summary>
      /// Custom inspector.
      /// </summary>
      protected override void VintageInspector()
      {
        VintageCommodore64 thisTarget = (VintageCommodore64)target;

        thisTarget.PixelSize = SliderField("Pixel size", thisTarget.PixelSize, 1, 25, 2);

        thisTarget.DitherSaturation = SliderField("Dither saturation", thisTarget.DitherSaturation, -2.0f, 2.0f, 1.0f);

        thisTarget.DitherNoise = SliderField("Dither noise", thisTarget.DitherNoise, 0.0f, 1.0f, 1.0f);

        thisTarget.Threshold = SliderField("Palete threshold", thisTarget.Threshold, 0.0f, 2.0f, 1.0f);
      }
    }
  }
}