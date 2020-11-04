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
    /// Vintage ZX Spectrum editor.
    /// </summary>
    [CustomEditor(typeof(VintageZXSpectrum))]
    public sealed class VintageZXSpectrumEditor : VintageEditorBase
    {
      /// <summary>
      /// Custom inspector.
      /// </summary>
      protected override void VintageInspector()
      {
        VintageZXSpectrum thisTarget = (VintageZXSpectrum)target;

        thisTarget.Mode = (VintageZXSpectrum.Modes)EnumPopupField("Mode", "Simulation mode. Default Full.", thisTarget.Mode, VintageZXSpectrum.Modes.Full);
        thisTarget.PixelSize = SliderField("Pixel size", "Resolution [1 - 10]. Default 2.", thisTarget.PixelSize, 1, 10, 2);
        thisTarget.Dither = SliderField("Dither", "Dither amount [0.0 - 1.0]. Default 1.", thisTarget.Dither, 0.0f, 1.0f, 1.0f);

        float half = thisTarget.BrightnessHalf;
        float full = thisTarget.BrightnessFull;
        MinMaxSliderField("Brightness levels", "Color brightness levels.", ref half, ref full, 0.0f, 1.0f, 0.5f, 1.0f);

        thisTarget.BrightnessHalf = half;
        thisTarget.BrightnessFull = full;
        
        thisTarget.AdjustGamma = SliderField("Adjust gamma", "Adjust input gamma [0.0 - ...]. Default 1.", thisTarget.AdjustGamma, 0.0f, 10.0f, 1.0f);
      }
    }
  }
}