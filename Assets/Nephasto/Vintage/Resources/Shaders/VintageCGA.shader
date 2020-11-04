///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage CGA (high intensity palette).
/// </summary>
Shader "Hidden/Vintage/VintageCGA"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"

  float3 _Color0;
  float3 _Color1;
  float3 _Color2;
  float3 _Color3;

  float _PixelSize;
  float _Threshold;

  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float2 c = floor((uv * _ScreenParams.xy) / _PixelSize);
    float2 coord = c * _PixelSize;
    pixel = SampleMainTexture(coord / _ScreenParams.xy);

    pixel = pow(pixel, _Threshold);

    float distance0 = distance(pixel, _Color0);
    float distance1 = distance(pixel, _Color3);
    float distance2 = distance(pixel, _Color2);
    float distance3 = distance(pixel, _Color1);

    pixel = _Color2;

    float colorDistance = min(distance2, distance3);
    colorDistance = min(colorDistance, distance1);
    colorDistance = min(colorDistance, distance0);

    if (colorDistance == distance0)
      pixel = _Color0;
    else if (colorDistance == distance1)
      pixel = _Color3;
    else if (colorDistance == distance3)
      pixel = _Color1;

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
