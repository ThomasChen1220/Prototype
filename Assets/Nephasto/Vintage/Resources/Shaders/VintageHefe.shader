///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Hefe.
/// </summary>
Shader "Hidden/Vintage/VintageHefe"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _EdgeBurnTex;
  sampler2D _LevelsTex;
  sampler2D _GradientTex;
  sampler2D _SoftLightTex;
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    final *= SampleTexture(_EdgeBurnTex, uv).rgb;

    final = PixelLevels(_LevelsTex, final);

    float3 gradSample = SampleTexture(_GradientTex, float2(Luminance(final), 0.5)).rgb;

    final *= float3(SampleTexture(_SoftLightTex, float2(final.r, 0.16666 - gradSample.r)).r,
                   SampleTexture(_SoftLightTex, float2(final.g, 0.5 - gradSample.g)).g,
                   SampleTexture(_SoftLightTex, float2(final.b, 0.83333 - gradSample.b)).b);

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
