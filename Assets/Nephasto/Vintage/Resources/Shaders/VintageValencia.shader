///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Valencia.
/// </summary>
Shader "Hidden/Vintage/VintageValencia"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _LevelsTex;
  sampler2D _GradientLevelsTex;
  
  const half4x4 saturateMatrix = half4x4(1.1402,  -0.0598,  -0.061,  0.0,
                                        -0.1174,   1.0826,  -0.1186, 0.0,
                                        -0.0228,  -0.0228,   1.1772, 0.0,
                                         0.0,      0.0,      0.0,    0.0);
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float3 final = pixel;

    final = PixelLevels(_LevelsTex, pixel);
    
    final = mul((half3x3)saturateMatrix, final);

    float avg = 1.0 - (final.r + final.g + final.b) * 0.33;

    final.r = tex2D(_GradientLevelsTex, float2(final.r, avg)).r;
    final.g = tex2D(_GradientLevelsTex, float2(final.g, avg)).g;
    final.b = tex2D(_GradientLevelsTex, float2(final.b, avg)).b;

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
