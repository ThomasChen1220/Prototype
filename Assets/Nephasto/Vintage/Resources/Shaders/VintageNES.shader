///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage NES.
/// </summary>
Shader "Hidden/Vintage/VintageNES"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  float _Luminosity;
  float _Threshold;

  inline float3 FindClosest(float3 ref)
  {
    float3 old = 25500.0;
    
    #define TRY_COLOR(color) old = lerp(color, old, step(length(old - ref), length(color - ref)));

    TRY_COLOR(float3(000.0, 088.0, 000.0));
    TRY_COLOR(float3(080.0, 048.0, 000.0));
    TRY_COLOR(float3(000.0, 104.0, 000.0));
    TRY_COLOR(float3(000.0, 064.0, 088.0));
    TRY_COLOR(float3(000.0, 120.0, 000.0));
    TRY_COLOR(float3(136.0, 020.0, 000.0));
    TRY_COLOR(float3(000.0, 168.0, 000.0));
    TRY_COLOR(float3(168.0, 016.0, 000.0));
    TRY_COLOR(float3(168.0, 000.0, 032.0));
    TRY_COLOR(float3(000.0, 168.0, 068.0));
    TRY_COLOR(float3(000.0, 184.0, 000.0));
    TRY_COLOR(float3(000.0, 000.0, 188.0));
    TRY_COLOR(float3(000.0, 136.0, 136.0));
    TRY_COLOR(float3(148.0, 000.0, 132.0));
    TRY_COLOR(float3(068.0, 040.0, 188.0));
    TRY_COLOR(float3(120.0, 120.0, 120.0));
    TRY_COLOR(float3(172.0, 124.0, 000.0));
    TRY_COLOR(float3(124.0, 124.0, 124.0));
    TRY_COLOR(float3(228.0, 000.0, 088.0));
    TRY_COLOR(float3(228.0, 092.0, 016.0));
    TRY_COLOR(float3(088.0, 216.0, 084.0));
    TRY_COLOR(float3(000.0, 000.0, 252.0));
    TRY_COLOR(float3(248.0, 056.0, 000.0));
    TRY_COLOR(float3(000.0, 088.0, 248.0));
    TRY_COLOR(float3(000.0, 120.0, 248.0));
    TRY_COLOR(float3(104.0, 068.0, 252.0));
    TRY_COLOR(float3(248.0, 120.0, 088.0));
    TRY_COLOR(float3(216.0, 000.0, 204.0));
    TRY_COLOR(float3(088.0, 248.0, 152.0));
    TRY_COLOR(float3(248.0, 088.0, 152.0));
    TRY_COLOR(float3(104.0, 136.0, 252.0));
    TRY_COLOR(float3(252.0, 160.0, 068.0));
    TRY_COLOR(float3(248.0, 184.0, 000.0));
    TRY_COLOR(float3(184.0, 248.0, 024.0));
    TRY_COLOR(float3(152.0, 120.0, 248.0));
    TRY_COLOR(float3(000.0, 232.0, 216.0));
    TRY_COLOR(float3(060.0, 188.0, 252.0));
    TRY_COLOR(float3(188.0, 188.0, 188.0));
    TRY_COLOR(float3(216.0, 248.0, 120.0));
    TRY_COLOR(float3(248.0, 216.0, 120.0));
    TRY_COLOR(float3(248.0, 164.0, 192.0));
    TRY_COLOR(float3(000.0, 252.0, 252.0));
    TRY_COLOR(float3(184.0, 184.0, 248.0));
    TRY_COLOR(float3(184.0, 248.0, 184.0));
    TRY_COLOR(float3(240.0, 208.0, 176.0));
    TRY_COLOR(float3(248.0, 120.0, 248.0));
    TRY_COLOR(float3(252.0, 224.0, 168.0));
    TRY_COLOR(float3(184.0, 248.0, 216.0));
    TRY_COLOR(float3(216.0, 184.0, 248.0));
    TRY_COLOR(float3(164.0, 228.0, 252.0));
    TRY_COLOR(float3(248.0, 184.0, 248.0));
    TRY_COLOR(float3(248.0, 216.0, 248.0));
    TRY_COLOR(float3(248.0, 248.0, 248.0));
    TRY_COLOR(float3(252.0, 252.0, 252.0));

    #undef TRY_COLOR
    
    return old;
  }
  
  inline float DitherMatrix(float x, float y)
  {
    return lerp(lerp(lerp(lerp(lerp(lerp(0.0, 32.0, step(1.0, y)), lerp(8.0, 40.0, step(3.0, y)), step(2.0, y)), lerp(lerp(2.0, 34.0, step(5.0, y)), lerp(10.0, 42.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), lerp(lerp(lerp(48.0, 16.0, step(1.0, y)), lerp(56.0, 24.0, step(3.0, y)), step(2.0, y)), lerp(lerp(50.0, 18.0, step(5.0, y)), lerp(58.0, 26.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), step(1.0, x)), lerp(lerp(lerp(lerp(12.0, 44.0, step(1.0, y)), lerp(4.0, 36.0, step(3.0, y)), step(2.0, y)), lerp(lerp(14.0, 46.0, step(5.0, y)), lerp(6.0, 38.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), lerp(lerp(lerp(60.0, 28.0, step(1.0, y)), lerp(52.0, 20.0, step(3.0, y)), step(2.0, y)), lerp(lerp(62.0, 30.0, step(5.0, y)), lerp(54.0, 22.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), step(3.0, x)), step(2.0, x)), lerp(lerp(lerp(lerp(lerp(3.0, 35.0, step(1.0, y)), lerp(11.0, 43.0, step(3.0, y)), step(2.0, y)), lerp(lerp(1.0, 33.0, step(5.0, y)), lerp(9.0, 41.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), lerp(lerp(lerp(51.0, 19.0, step(1.0, y)), lerp(59.0, 27.0, step(3.0, y)), step(2.0, y)), lerp(lerp(49.0, 17.0, step(5.0, y)), lerp(57.0, 25.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), step(5.0, x)), lerp(lerp(lerp(lerp(15.0, 47.0, step(1.0, y)), lerp(7.0, 39.0, step(3.0, y)), step(2.0, y)), lerp(lerp(13.0, 45.0, step(5.0, y)), lerp(5.0, 37.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), lerp(lerp(lerp(63.0, 31.0, step(1.0, y)), lerp(55.0, 23.0, step(3.0, y)), step(2.0, y)), lerp(lerp(61.0, 29.0, step(5.0, y)), lerp(53.0, 21.0, step(7.0, y)), step(6.0, y)), step(4.0, y)), step(7.0, x)), step(6.0, x)), step(4.0, x));
  }

  inline float3 Dither(float3 color, float2 uv)
  {
    color = pow(color, _Threshold);
    color *= 255.0 * _Luminosity * 0.8;
    color += DitherMatrix(fmod(uv.x, 8.0), fmod(uv.y, 8.0));
    color = FindClosest(clamp(color, 0.0, 255.0));
		
    return color / 255.0;
  }

  inline float3 Vintage(float3 pixel, float2 uv)
  {
    return Dither(pixel, uv * _ScreenParams.xy);
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
