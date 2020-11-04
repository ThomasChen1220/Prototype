///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Commodore 64.
/// </summary>
Shader "Hidden/Vintage/VintageCommodore64"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  float _PixelSize;
  float _DitherSaturation;
  float _DitherNoise;
  float _Threshold;

  inline float Hash(float2 p)
  {
    return frac(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x))));
  }

  inline float Compare(float3 a, float3 b)
  {
    a = max(0.0, a - min(a.r, min(a.g, a.b)) * 0.25 * _DitherSaturation);
    b = max(0.0, b - min(b.r, min(b.g, b.b)) * 0.25 * _DitherSaturation);

    a *= a * a;
    b *= b * b;

    float3 diff = (a - b);

    return dot(diff, diff);
  }

  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    float2 c = floor((uv * _ScreenParams.xy) / _PixelSize);
    float2 coord = c * _PixelSize;
    float3 src = SampleMainTexture(coord / _ScreenParams.xy).rgb;
    src = pow(src, _Threshold);

    float3 dst0 = 0.0;
    float3 dst1 = 0.0;
    float best0 = 1e3;
    float best1 = 1e3;

    #define TRY_COLOR(R, G, B) { const float3 tst = float3(R, G, B); float err = Compare(src, tst); if (err < best0) { best1 = best0; dst1 = dst0; best0 = err; dst0 = tst; } }

    TRY_COLOR(0.078431, 0.047059, 0.109804);
    TRY_COLOR(0.266667, 0.141176, 0.203922);
    TRY_COLOR(0.188235, 0.203922, 0.427451);
    TRY_COLOR(0.305882, 0.290196, 0.305882);
    TRY_COLOR(0.521569, 0.298039, 0.188235);
    TRY_COLOR(0.203922, 0.396078, 0.141176);
    TRY_COLOR(0.815686, 0.274510, 0.282353);
    TRY_COLOR(0.458824, 0.443137, 0.380392);
    TRY_COLOR(0.349020, 0.490196, 0.807843);
    TRY_COLOR(0.823529, 0.490196, 0.172549);
    TRY_COLOR(0.521569, 0.584314, 0.631373);
    TRY_COLOR(0.427451, 0.666667, 0.172549);
    TRY_COLOR(0.823529, 0.666667, 0.600000);
    TRY_COLOR(0.427451, 0.760784, 0.792157);
    TRY_COLOR(0.854902, 0.831373, 0.368627);
    TRY_COLOR(0.870588, 0.933333, 0.839216);

    #undef TRY_COLOR

    best0 = sqrt(best0);
    best1 = sqrt(best1);

    final = fmod(c.x + c.y, 2.0 * _DitherNoise) > (Hash(c * 2.0 * _DitherNoise + frac(sin(float2(floor(1.9 * _DitherNoise), floor(1.7 * _DitherNoise))))) * 0.75 * _DitherNoise) + (best1 / (best0 + best1)) ? dst1 : dst0;

    return final;
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
