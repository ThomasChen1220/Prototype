///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;

namespace Nephasto
{
  namespace VintageAsset
  {
    /// <summary>
    /// Vintage Film Look editor.
    /// </summary>
    [CustomEditor(typeof(VintageFilmLook))]
    public sealed class VintageFilmLookEditor : VintageEditorBase
    {
      /// <summary>
      /// Custom inspector.
      /// </summary>
      protected override void VintageInspector()
      {
        VintageFilmLook thisTarget = (VintageFilmLook)target;

        thisTarget.Manufacturer = (VintageFilmLook.Manufacturers)EnumPopupField("Film look", thisTarget.Manufacturer, VintageFilmLook.Manufacturers.Kodak_2383);

        if (thisTarget.Manufacturer == VintageFilmLook.Manufacturers.Custom)
        {
          IndentLevel++;

          thisTarget.CustomCDL.slope = Vector3Field("Slope", "Slope of the transfer function without shifting the black level.", thisTarget.CustomCDL.slope, Vector3.one);
          thisTarget.CustomCDL.offset = Vector3Field("Offset", "Raises or lowers overall brightness of a component.", thisTarget.CustomCDL.offset, Vector3.zero);
          thisTarget.CustomCDL.power = Vector3Field("Power", "Changes the intermediate shape of the transfer function.", thisTarget.CustomCDL.power, Vector3.one);

          thisTarget.CustomCDL.saturation = SliderField("Saturation", "Color saturation.", thisTarget.CustomCDL.saturation, 0.0f, 2.0f, 1.0f);
          thisTarget.CustomCDL.contrast = SliderField("Contrast", "Color contrast.", thisTarget.CustomCDL.contrast, 0.0f, 2.0f, 1.0f);
          thisTarget.CustomCDL.gamma = SliderField("Gamma", "Gamma.", thisTarget.CustomCDL.gamma, 0.0f, 2.0f, 1.0f);

          thisTarget.CustomCDL.filmContrast = Toggle("Film contrast", "Extra contrast.", thisTarget.CustomCDL.filmContrast, false);
          
          IndentLevel--;
        }
      }
    }
  }
}