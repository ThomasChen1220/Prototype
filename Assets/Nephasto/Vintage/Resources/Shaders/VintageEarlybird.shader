///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Earlybird.
/// </summary>
Shader "Hidden/Vintage/VintageEarlybird"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _CurvesTex;
  sampler2D _OverlayTex;
  sampler2D _BlowoutTex;
  sampler2D _LevelsTex;
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    // Curves.
    float2 lookup;
    lookup.y = 0.5;
    lookup.x = pixel.r;
    final.r = tex2D(_CurvesTex, lookup).r;
    lookup.x = pixel.g;
    final.g = tex2D(_CurvesTex, lookup).g;
    lookup.x = pixel.b;
    final.b = tex2D(_CurvesTex, lookup).b;

    float avg = (final.r + final.g + final.b) / 3.0;

	  // Overlay.
    float3 result;
    lookup.x = avg;
    result.r = tex2D(_OverlayTex, lookup).r;
    lookup.x = avg;
    result.g = tex2D(_OverlayTex, lookup).g;
    lookup.x = avg;
    result.b = tex2D(_OverlayTex, lookup).b;

    final = lerp(final, result, 0.5);

    // Blowout.
    float3 sampled;
    lookup.x = final.r;
    sampled.r = tex2D(_BlowoutTex, lookup).r;
    lookup.x = final.g;
    sampled.g = tex2D(_BlowoutTex, lookup).g;
    lookup.x = final.b;
    sampled.b = tex2D(_BlowoutTex, lookup).b;

    final = sampled;

    // Levels.
    lookup.x = final.r;
    final.r = tex2D(_LevelsTex, lookup).r;
    lookup.x = final.g;
    final.g = tex2D(_LevelsTex, lookup).g;
    lookup.x = final.b;
    final.b = tex2D(_LevelsTex, lookup).b;

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
