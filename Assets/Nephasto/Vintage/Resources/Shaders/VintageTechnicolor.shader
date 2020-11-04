///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Vintage Technicolor.
/// </summary>
Shader "Hidden/Vintage/VintageTechnicolor"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "VintageCG.cginc"
  
  sampler2D _CurvesTex;
  sampler2D _EdgeBurnTex;
  
  inline float3 Vintage(float3 pixel, float2 uv)
  {
    const float3 redFilter = float3(1.0, 0.0, 0.0);
    const float3 bluegreenFilter = float3(0.0, 1.0, 0.7);
    const float3 cyanFilter = float3(0.0, 1.0, 0.5);
    const float3 magentaFilter = float3(1.0, 0.0, 0.25);

    float3 redRecord = pixel * redFilter;
	  float3 bluegreenRecord = pixel * bluegreenFilter;
    
	  float bluegreenNegative = (bluegreenRecord.g + bluegreenRecord.b) * 0.5;
    
#if TECHNICOLOR_ONE
    float3 redOutput = redRecord.r * redFilter;
	  float3 bluegreenOutput = bluegreenNegative * bluegreenFilter;

    return redOutput + bluegreenOutput;
#elif TECHNICOLOR_TWO
    float3 redOutput = redRecord.r + cyanFilter;
	  float3 bluegreenOutput = bluegreenNegative + magentaFilter;

    return redOutput * bluegreenOutput;
#else
    float redMatte = pixel.r - ((pixel.g + pixel.b) * 0.5);
    float greenMatte = pixel.g - ((pixel.r + pixel.b) * 0.5);
    float blueMatte = pixel.b - ((pixel.r + pixel.g) * 0.5);

    redMatte = 1.0 - redMatte;
    greenMatte = 1.0 - greenMatte;
    blueMatte  = 1.0 - blueMatte;

    float red = greenMatte * blueMatte * pixel.r;
    float green = redMatte * blueMatte * pixel.g;
    float blue = redMatte * greenMatte * pixel.b;

    return float3(red, green, blue);
#endif
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
      
      #pragma multi_compile ___ TECHNICOLOR_ONE TECHNICOLOR_TWO TECHNICOLOR_THREE
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
