///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage ZX Spectrum.
/// </summary>
Shader "Hidden/Vintage/VintageZXSpectrum"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  uniform float _PixelSize;
  uniform float _Dither;
  uniform float _BrightnessFull;
  uniform float _BrightnessHalf;
  uniform float _BrightnessGamma;
  uniform float _AdjustGamma;

  inline float4 SMap(float3 pixel)
  {
    pixel = pow(pixel, (float3)_AdjustGamma);
  
    return ((pixel.r > _BrightnessHalf) || (pixel.g > _BrightnessHalf) || (pixel.b > _BrightnessHalf)) ? float4(pixel, 1.0) : float4(min((pixel / _BrightnessHalf), (float3)1.0), 0.0);
  }

  inline float4 BMap(float3 pixel)
  {
    pixel = pow(pixel, (float3)_AdjustGamma);

    return ((pixel.r > _BrightnessHalf) || (pixel.g > _BrightnessHalf) || (pixel.b > _BrightnessHalf)) ? float4(floor(pixel + (float3)0.5), 1.0) : float4(min(floor((pixel / _BrightnessHalf) + (float3)0.5), (float3)1.0), 0.0); 
  }

  inline float3 FMap(float4 pixel)
  {
    return pixel.a >= 0.5 ? pixel.rgb * (float3)_BrightnessFull : pixel.rgb * (float3)_BrightnessFull;
  }
  
  inline float3 OnlyColors(float2 uv)
  {
    float2 pv = floor((_ScreenParams.xy * uv) / _PixelSize);
    float2 sv = floor(_ScreenParams.xy / _PixelSize);

    float4 cs = SMap(SampleMainTexture(pv / sv));
    
    return mod(pv.x + pv.y, 2.0) == 1.0 ? float3(FMap(float4(floor(cs.rgb + (float3)(0.5 + (_Dither * 0.3))), cs.a))) :
                                          float3(FMap(float4(floor(cs.rgb + (float3)(0.5 - (_Dither * 0.3))), cs.a)));
  }
  
  inline float3 Full(float2 uv)
  {
    float2 pv = floor((_ScreenParams.xy * uv) / _PixelSize);
    float2 bv = floor(pv / _PixelSize) * _PixelSize;
    float2 sv = floor(_ScreenParams.xy / _PixelSize);
    
    float4 min_cs = (float4)1.0;
    float4 max_cs = (float4)0.0;
    float bright = 0.0;

    for (int py = 1; py < 8; ++py)
    {
      for (int px = 0; px < 8; ++px)
      {
        float4 cs = BMap((SampleMainTextureLod((bv + float2(px, py)) / sv)));
        bright += cs.a;
        min_cs = min(min_cs, cs);
        max_cs = max(max_cs, cs);
      }
    }

    bright = bright >= 24.0 ? 1.0 : 0.0;
    
    if (all(max_cs.rgb == min_cs.rgb))
      min_cs.rgb = (float3)0.0;

    if (all(max_cs.rgb == (float3)0.0))
    {
      bright = 0.0;
      max_cs.rgb = float3(0.0, 0.0, 1.0);
      min_cs.rgb = (float3)0.0;
    }

    float3 c1 = FMap(float4(max_cs.rgb, bright));
    float3 c2 = FMap(float4(min_cs.rgb, bright));
    float3 cs = SampleMainTexture(pv / sv);
    
    float3 d = (cs + cs) - (c1 + c2);
    float dd = d.r + d.g + d.b;

    return mod(pv.x + pv.y, 2.0) == 1.0 ? float3(dd >= -(_Dither * 0.5) ? c1.r : c2.r, dd >= -(_Dither * 0.5) ? c1.g : c2.g, dd >= -(_Dither * 0.5) ? c1.b : c2.b) :
                                          float3(dd >=  (_Dither * 0.5) ? c1.r : c2.r, dd >=  (_Dither * 0.5) ? c1.g : c2.g, dd >=  (_Dither * 0.5) ? c1.b : c2.b);
  } 

  inline half3 Blend(half3 s, half3 d)
  {
    return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
  }

  inline float3 Vintage(float3 pixel, float2 uv)
  {
    return
#if FULL
    Blend(pixel, Full(uv));
#else
    Blend(pixel, OnlyColors(uv));
#endif
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
      
      #pragma multi_compile ___ FULL
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
