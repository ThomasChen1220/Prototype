///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Brannan.
/// </summary>
Shader "Hidden/Vintage/VintageBrannan"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _ProcessTex;
  sampler2D _BlowoutTex;
  sampler2D _ContrastTex;
  sampler2D _LumaTex;
  sampler2D _ScreenTex;

  float _OverlayStrength;
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    // Process.
    float2 lookup;
    lookup.y = 0.5;
    lookup.x = pixel.r;
    final.r = tex2D(_ProcessTex, lookup).r;
    lookup.x = pixel.g;
    final.g = tex2D(_ProcessTex, lookup).g;
    lookup.x = pixel.b;
    final.b = tex2D(_ProcessTex, lookup).b;
    
    // Blowout.
    float2 tc = (2.0 * uv) - 1.0;
    float d = dot(tc, tc);
    float3 sampled;
    lookup.x = final.r;
    sampled.r = tex2D(_BlowoutTex, lookup).r;
    lookup.x = final.g;
    sampled.g = tex2D(_BlowoutTex, lookup).g;
    lookup.x = final.b;
    sampled.b = tex2D(_BlowoutTex, lookup).b;
    float value = smoothstep(0.0, 1.0, d);
    final = lerp(sampled, final, value);

    // Constrast.
    lookup.x = final.r;
    final.r = tex2D(_ContrastTex, lookup).r;
    lookup.x = final.g;
    final.g = tex2D(_ContrastTex, lookup).g;
    lookup.x = final.b;
    final.b = tex2D(_ContrastTex, lookup).b;

    // Luma.
    lookup.x = Luminance(final);
    final = lerp(tex2D(_LumaTex, lookup).rgb, final, 0.5f);

    // Screen.
    lookup.x = final.r;
    final.r = tex2D(_ScreenTex, lookup).r;
    lookup.x = final.g;
    final.g = tex2D(_ScreenTex, lookup).g;
    lookup.x = final.b;
    final.b = tex2D(_ScreenTex, lookup).b;

    final = pow(final, 0.8);

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
