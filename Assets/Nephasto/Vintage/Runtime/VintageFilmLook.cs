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
    /// Vintage Film Look.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Nephasto/Vintage/Vintage Film Look")]
    public sealed class VintageFilmLook : VintageBase
    {
      /// <summary>
      /// Film Color Decision List (CDL) values.
      /// </summary>
      public class FilmCDL
      {
        // Slope of the transfer function without shifting the black level.
        public Vector3 slope = Vector3.one;
        
        // Raises or lowers overall brightness of a component.
        public Vector3 offset = Vector3.zero;
        
        // Changes the intermediate shape of the transfer function.
        public Vector3 power = Vector3.one;

        // Color saturation.
        public float saturation = 1.0f;

        // Color contrast.
        public float contrast = 1.0f;

        // Gamma.
        public float gamma = 1.0f;

        public bool filmContrast = false;
      }

      /// <summary>
      /// Manufacturers.
      /// </summary>
      public enum Manufacturers
      {
        Kodak_2383,
        Kodak_2393,
        Kodak_2395,
        
        Agfa_1978,
        Agfa_1905,
        Agfa_1935,

        Beer_1973,
        Beer_1933,
        Beer_2001,
        Beer_2006,

        Polaroid,
        
        Cuba_libre,
       
        Fuji_4711,

        ORWO_0815,
        
        Black_white,
        
        Spearmint,
        
        Tea_time,
        
        Purple_rain,
        
        Custom,
      }

      /// <summary>
      /// Manufacturer film.
      /// </summary>
      public Manufacturers Manufacturer
      {
        get { return manufacturer; }
        set { if (value != manufacturer) { manufacturer = value; needUpdateValues = true; } }
      }

      /// <summary>
      /// Custom CDL values. Manufacturer must be set to Manufacturers.Custom.
      /// </summary>
      public FilmCDL CustomCDL
      {
        get { return customCDL; }
        set { if (value.Equals(customCDL) == false) { customCDL = value; needUpdateValues = true; } }
      }
      
      [SerializeField]
      private Manufacturers manufacturer;

      [SerializeField]
      private FilmCDL customCDL = new FilmCDL();

      private readonly FilmCDL[] presets = new FilmCDL[]
      {
        // Kodak 2383.
        new FilmCDL() { slope = new Vector3(1.01f, 1.0f, 1.0f),                offset = Vector3.zero,                                   power = new Vector3(0.95f, 1.0f, 1.0f),               saturation = 1.2f,       contrast = 1.0f,  gamma = 1.0f,  filmContrast = true },
        
        // Kodak 2393.
        new FilmCDL() { slope = new Vector3(1.08f, 1.19f, 1.07f),              offset = new Vector3(0.04f, -0.06f, 0.02f),              power = new Vector3(1.07f, 1.11f, 1.20f),             saturation = 1.0f,       contrast = 1.0f,  gamma = 1.0f,  filmContrast = true },
        
        // Kodak 2395.
        new FilmCDL() { slope = new Vector3(0.98f, 1.0f, 1.03f),               offset = Vector3.zero,                                   power = new Vector3(0.84f, 0.97f, 1.10f),             saturation = 1.0f,       contrast = 1.0f,  gamma = 1.0f,  filmContrast = true },
        
        // Agfa 1978.
        new FilmCDL() { slope = new Vector3(1.12f, 1.42f, 1.19f),              offset = new Vector3(0.04f, -0.06f, 0.02f),              power = new Vector3(0.94f, 0.81f, 0.83f),             saturation = 0.7f,       contrast = 1.06f, gamma = 1.0f,  filmContrast = true },
        
        // Agfa 1905.
        new FilmCDL() { slope = Vector3.one,                                   offset = new Vector3(-0.05f, -0.04f, -0.03f),            power = Vector3.one,                                  saturation = 0.0f,       contrast = 1.33f, gamma = 0.6f,  filmContrast = false },
        
        // Agfa 1935.
        new FilmCDL() { slope = new Vector3(1.33f, 1.01f, 0.63f),              offset = Vector3.zero,                                   power = new Vector3(1.21f, 0.96f, 0.74f),             saturation = 0.6f,       contrast = 1.0f,  gamma = 0.83f, filmContrast = true },
        
        // Beer 1973.
        new FilmCDL() { slope = new Vector3(0.88f, 0.96f, 1.24f),              offset = Vector3.zero,                                   power = new Vector3(1.45f, 1.29f, 1.27f),             saturation = 1.0f,       contrast = 0.93f, gamma = 0.9f,  filmContrast = true },
        
        // Beer 1933.
        new FilmCDL() { slope = new Vector3(1.2f, 1.2f, 1.2f),                 offset = Vector3.zero,                                   power = new Vector3(1.3f, 1.3f, 1.3f),                saturation = 0.0f,       contrast = 0.8f,  gamma = 1.2f,  filmContrast = true },
        
        // Beer 2001.
        new FilmCDL() { slope = new Vector3(0.93f, 0.94f, 0.96f),              offset = Vector3.zero,                                   power = new Vector3(1.6f, 1.1f, 0.95f),               saturation = 0.4f,       contrast = 1.1f,  gamma = 0.7f,  filmContrast = true },
        
        // Beer 2006.
        new FilmCDL() { slope = new Vector3(1.616452f, 1.331932f, 0.842867f),  offset = new Vector3(-0.152205f, 0.079621f, 0.197558f),  power = new Vector3(1.650251f, 1.536614f, 1.553357f), saturation = 0.7f,       contrast = 1.0f,  gamma = 1.1f,  filmContrast = false },
        
        // Polaroid.
        new FilmCDL() { slope = new Vector3(0.65f, 1.0f, 0.8f),                offset = new Vector3(0.07f, 0.0f, 0.08f),                power = Vector3.one,                                  saturation = 1.4f,       contrast = 1.0f,  gamma = 1.0f,  filmContrast = true },
        
        // Cuba libre.
        new FilmCDL() { slope = new Vector3(1.19f, 1.1f, 0.77f),               offset = new Vector3(-0.04f, -0.08f, -0.07f),            power = new Vector3(0.8f, 0.8f, 0.8f),                saturation = 0.9f,       contrast = 1.0f,  gamma = 0.9f,  filmContrast = true },
        
        // Fuji 4711.
        new FilmCDL() { slope = new Vector3(1.1f, 1.0f, 0.8f),                 offset = Vector3.zero,                                   power = new Vector3(1.5f, 1.0f, 1.0f),                saturation = 0.6f,       contrast = 1.0f,  gamma = 0.9f,  filmContrast = true },
        
        // ORWO 0815.
        new FilmCDL() { slope = new Vector3(1.15f, 1.11f, 0.86f),              offset = new Vector3(0.0f, 0.01f, -0.02f),               power = new Vector3(1.41f, 1.0f, 0.74f),              saturation = 0.45f,      contrast = 0.98f, gamma = 0.86f, filmContrast = true },
        
        // Black and White.
        new FilmCDL() { slope = Vector3.one,                                   offset = Vector3.zero,                                   power = Vector3.one,                                  saturation = 0.0f,       contrast = 1.1f,  gamma = 0.7f,  filmContrast = true },
        
        // Spearmint.
        new FilmCDL() { slope = new Vector3(1.02f, 1.32f, 1.09f),              offset = new Vector3(0.04f, -0.06f, 0.02f),              power = new Vector3(0.70f, 0.44f, 0.51f),             saturation = 0.8f,       contrast = 1.0f,  gamma = 1.30f, filmContrast = true },
        
        // Tea time.
        new FilmCDL() { slope = new Vector3(1.297496f, 0.943091f, 0.501793f),  offset = new Vector3(-0.132450f, 0.036699f, 0.147457f),  power = new Vector3(1.180667f, 1.032265f, 1.215274f), saturation = 1.0f,       contrast = 1.0f,  gamma = 1.0f,  filmContrast = false },
        
        // Purple rain.
        new FilmCDL() { slope = new Vector3(1.671897f, 1.274243f, 0.994490f),  offset = new Vector3(-0.052148f, -0.026815f, 0.483182f), power = new Vector3(1.650251f, 1.536614f, 1.553357f), saturation = 0.282609f,  contrast = 1.0f,  gamma = 1.0f,  filmContrast = false },
      };

      private static readonly int variableSlope = Shader.PropertyToID("_Slope");
      private static readonly int variableOffset = Shader.PropertyToID("_Offset");
      private static readonly int variablePower = Shader.PropertyToID("_Power");
      private static readonly int variableSaturation = Shader.PropertyToID("_Saturation2");
      private static readonly int variableContrast = Shader.PropertyToID("_Contrast2");
      private static readonly int variableGamma = Shader.PropertyToID("_Gamma2");
      private static readonly int variableFilmContrast = Shader.PropertyToID("_FilmContrast");
      
      /// <summary>
      /// Effect description.
      /// </summary>
      public override string ToString() => "Imitates the look of different film manufacturers.";

      /// <summary>
      /// Set the default values of the shader.
      /// </summary>
      public override void ResetDefaultValues()
      {
        manufacturer = Manufacturers.Kodak_2383;

        base.ResetDefaultValues();
      }

      /// <summary>
      /// Set the values to shader.
      /// </summary>
      protected override void UpdateCustomValues()
      {
        if (Manufacturer == Manufacturers.Custom)
        {
          material.SetVector(variableSlope, customCDL.slope);
          material.SetVector(variableOffset, customCDL.offset);
          material.SetVector(variablePower, customCDL.power);
          material.SetFloat(variableSaturation, customCDL.saturation);
          material.SetFloat(variableContrast, customCDL.contrast);
          material.SetFloat(variableGamma, customCDL.gamma);
          material.SetInt(variableFilmContrast, customCDL.filmContrast ? 1 : 0);
        }
        else
        {
          material.SetVector(variableSlope, presets[(int)manufacturer].slope);
          material.SetVector(variableOffset, presets[(int)manufacturer].offset);
          material.SetVector(variablePower, presets[(int)manufacturer].power);
          material.SetFloat(variableSaturation, presets[(int)manufacturer].saturation);
          material.SetFloat(variableContrast, presets[(int)manufacturer].contrast);
          material.SetFloat(variableGamma, presets[(int)manufacturer].gamma);
          material.SetInt(variableFilmContrast, presets[(int)manufacturer].filmContrast ? 1 : 0);
        }
      }
    }
  }
}