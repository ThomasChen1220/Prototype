///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Toaster.
/// </summary>
Shader "Hidden/Vintage/VintageToaster"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _MetalTex;
  sampler2D _SoftLightTex;
  sampler2D _CurvesTex;
  sampler2D _OverlayWarmTex;
  sampler2D _ColorShiftTex;
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    float2 lookup = float2(0.0, 0.0);

    float3 metal = tex2D(_MetalTex, uv).rgb;

    lookup = float2(metal.r, pixel.r);
    final.r = tex2D(_SoftLightTex, 1.0 - lookup).r;

    lookup = float2(metal.g, pixel.g);
    final.g = tex2D(_SoftLightTex, 1.0 - lookup).g;

    lookup = float2(metal.b, pixel.b);
    final.b = tex2D(_SoftLightTex, 1.0 - lookup).b;

    final = PixelLevels(_CurvesTex, final);
    
    float2 tc = ((2.0 * uv) - 1.0);
    lookup.x = dot(tc, tc);
    lookup.y = 1.0 - final.r;
    final.r = tex2D(_OverlayWarmTex, lookup).r;
    lookup.y = 1.0 - final.g;
    final.g = tex2D(_OverlayWarmTex, lookup).g;
    lookup.y = 1.0 - final.b;
    final.b = tex2D(_OverlayWarmTex, lookup).b;

    return PixelLevels(_ColorShiftTex, final);
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
