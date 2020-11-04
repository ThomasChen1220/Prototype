///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage LUT.
/// </summary>
Shader "Hidden/Vintage/VintageLut"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
#ifdef LUT_3D
  sampler3D _LutTex;
#else
  sampler2D _LutTex;
#endif

  float3 _LutParams;

  float3 internal_tex3d(sampler2D tex, float3 uv)
  {
    uv.y = 1.0 - uv.y;
    uv.z *= _LutParams.z;

    float shift = floor(uv.z);
    uv.xy = uv.xy * _LutParams.z * _LutParams.xy + 0.5 * _LutParams.xy;
    uv.x += shift * _LutParams.y;
    uv.xyz = lerp(tex2D(tex, uv.xy).rgb, tex2D(tex, uv.xy + float2(_LutParams.y, 0.0)).rgb, uv.z - shift);

    return uv;
  }

  inline float3 Vintage(float3 pixel, float2 uv)
  {
#ifdef LUT_3D
    pixel = tex3D(_LutTex, (pixel * _LutParams.x) + _LutParams.y).rgb;
#else
    pixel = internal_tex3d(_LutTex, pixel);
#endif

    return pixel;
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
      
      #pragma multi_compile ___ LUT_3D
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
