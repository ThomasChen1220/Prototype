///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// .
/// </summary>
Shader "Hidden/Vintage/VintageOldCRT"
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"

  float _NoiseX;
  float _RGBNoise;
  float _Barrel;
  float2 _Offset;
  float _SinNoiseWidth;
  float _SinNoiseScale;
  float _SinNoiseOffset;
  float _ScanLineTail;
  float _ScanLineSpeed;

  inline float3 Vintage(float3 pixel, float2 uv)
  {
    float2 inUV = uv;
    uv = uv - 0.5;

    // Calc vignette.
    float vignet = length(uv);

    // Barrel.
    uv /= 1.0 - vignet * _Barrel;
    float2 texUV = uv + 0.5;

    if (max(abs(uv.y) - 0.5, abs(uv.x) - 0.5) > 0.0)
      return (float3)0.0;

    // Apply noise and offset.
    texUV.x += sin(texUV.y * _SinNoiseWidth + _SinNoiseOffset) * _SinNoiseScale;
    texUV += _Offset;
    texUV.x += (Rand(floor(texUV.y * 500) + _Time.y) - 0.5) * _NoiseX;
    texUV = mod(texUV, 1);

    // Shift RGB.
    pixel.r = tex2D(_MainTex, texUV).r;
    pixel.g = tex2D(_MainTex, texUV - float2(0.002, 0.0)).g;
    pixel.b = tex2D(_MainTex, texUV - float2(0.004, 0.0)).b;

    // RGB noise.
    if (Rand((Rand(floor(texUV.y * 500.0) + _Time.y) - 0.5) + _Time.y) < _RGBNoise)
    {
      pixel.r = Rand(uv + float2(123.0 + _Time.y, 0.0));
      pixel.g = Rand(uv + float2(123.0 + _Time.y, 1.0));
      pixel.b = Rand(uv + float2(123.0 + _Time.y, 2.0));
    }

    // Determine the RGB to draw for each pixel.
    float floorX = fmod(inUV.x * _ScreenParams.x / 3.0, 1.0);
    pixel.r *= floorX > 0.3333;
    pixel.g *= floorX < 0.3333 || floorX > 0.6666;
    pixel.b *= floorX < 0.6666;

    // Scanlines.
    float scanLineColor = sin(_Time.y * 10.0 + uv.y * 500.0) / 2.0 + 0.5;
    pixel *= 0.5 + clamp(scanLineColor + 0.5, 0.0, 1.0) * 0.5;

    // Tail trace.
    float tail = clamp((frac(uv.y + _Time.y * _ScanLineSpeed) - 1.0 + _ScanLineTail) / min(_ScanLineTail, 1.0), 0.0, 1.0);
    pixel *= tail;

    // Apply vignette.
    pixel *= 1.0 - vignet * 1.3;

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
      #pragma multi_compile ___ DEMO

      #pragma vertex vert
      #pragma fragment vintageFrag
      ENDCG
    }
  }
  
  FallBack off
}
