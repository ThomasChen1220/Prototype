///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Film Look.
/// </summary>
Shader "Hidden/Vintage/VintageFilmLook"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  float3 _Slope;
  float3 _Offset;
  float3 _Power;
  float _Saturation2;
  float _Contrast2;
  float _Gamma2;
  int _FilmContrast;
  
  inline float3 Saturation(float3 pixel, float adjustment)
  {
    const float3 W = float3(0.2126, 0.7152, 0.0722);

    return lerp((float3)dot(pixel, W), pixel, adjustment);
  }

  inline float3 Contrast(float3 pixel, float4 contrast)
  {
    float3 c = contrast.rgb * (float3)contrast.a;

    return (1.0 - c.rgb) * 0.5 + c.rgb * pixel;
  }

  inline float3 FilmContrast(float3 contrast) 
  {
    return 1.0 / (1.0 + (exp(-(contrast - 0.5) * 7.0)));
  }
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
	// Gamma correction. 
	pixel = pow(pixel, (float3)_Gamma2);

	// CDL values.
	pixel = pow(clamp(((pixel * _Slope) + _Offset), 0.0, 1.0), _Power);

    // Saturation.
	pixel = Saturation(pixel, _Saturation2);

	// Contrast.
	pixel = Contrast(pixel, (float4)_Contrast2);

	// Film contrast.
	if (_FilmContrast == 1)
      pixel = FilmContrast(pixel);

    return saturate(pixel);
  }

  #include "VintageFragCG.cginc"
  ENDCG

  SubShader
  {
    Cull Off
    ZWrite Off
    ZTest Always

    // Pass 0: Effect.
    Pass
    {
      CGPROGRAM
      #pragma target 3.0
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma exclude_renderers d3d9 d3d11_9x ps3 flash
      
      #pragma multi_compile ___ MODE_SCREEN MODE_LAYER MODE_DISTANCE
      #pragma multi_compile ___ COLOR_CONTROLS
      #pragma multi_compile ___ FILM_ENABLED
      #pragma multi_compile ___ CRT_ENABLED
      #pragma multi_compile ___ DEMO
      
      #pragma vertex vert
      #pragma fragment vintageFrag
      ENDCG
    }
  }
  
  FallBack off
}
