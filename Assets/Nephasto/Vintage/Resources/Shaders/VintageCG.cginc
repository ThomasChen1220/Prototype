///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef VINTAGE_CGINC
#define VINTAGE_CGINC

// Common.
float _Strength;
float4 _RandomValue;

// Color control.
#ifdef COLOR_CONTROLS
  float _Brightness;
  float _Contrast;
  float _Gamma;
  float _Hue;
  float _Saturation;
#endif

// Film.
#ifdef FILM_ENABLED
  float _Vignette;
  float _FilmGrainStrength;
  float _FilmBlinkStrenght;
  float _FilmBlinkSpeed;
  int _FilmBlotches;
  float _FilmBlotchSize;
  float _FilmScratches;
  int _FilmLines;
  float _FilmLinesStrength;
#endif

// CRT.
#ifdef CRT_ENABLED
  float _ScanLine;
  float _SlowScan;
  float _ScanDistort;
  float _CRTVignette;
  float _CRTVignetteAperture;
  float _ColorShift;
  float _ReflexionShine;
  float _ReflexionAmbient;
#endif

UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
float4 _MainTex_ST;
float4 _MainTex_TexelSize;
float4 _MaskTex_TexelSize;

sampler2D _NoiseTex;

#if defined(MODE_LAYER) || defined(MODE_DISTANCE)
  UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
#endif

#if MODE_DISTANCE
  sampler2D _DistanceTex;
#endif

#ifdef MODE_LAYER
  float _DepthThreshold;

  sampler2D _RTT;
#endif

inline float3 ToLinear(float3 pixel)
{
  return pixel * (pixel * (pixel * 0.305306011 + 0.682171111) + 0.012522878);
}

inline float3 ToSRGB(float3 pixel)
{
  return pixel = max(1.055 * pow(pixel, 0.416666667) - 0.055, 0.0);
}

inline float3 SampleMainTexture(float2 uv)
{
  float3 pixel = 
#if defined(UNITY_SINGLE_PASS_STEREO)
  tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST)).rgb;
#else
  UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, uv).rgb;
#endif

#if UNITY_COLORSPACE_GAMMA
    pixel.rgb = GammaToLinearSpace(pixel.rgb);
#endif

  return pixel;
}

inline float3 SampleMainTextureLod(float2 uv)
{
  float3 pixel = 
#if defined(UNITY_SINGLE_PASS_STEREO)
    tex2Dlod(_MainTex, float4(UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST), 0.0, 0.0)).rgb;
#else
    tex2Dlod(_MainTex, float4(uv, 0.0, 0.0)).rgb;
#endif

#if UNITY_COLORSPACE_GAMMA
    pixel.rgb = GammaToLinearSpace(pixel.rgb);
#endif

  return pixel;
}

inline float3 SampleTexture(sampler2D tex, float2 uv)
{
  float3 pixel = UNITY_SAMPLE_SCREENSPACE_TEXTURE(tex, uv).rgb;

#if UNITY_COLORSPACE_GAMMA
    pixel.rgb = GammaToLinearSpace(pixel.rgb);
#endif
  
  return pixel;
}

inline float mod(float x, float y)
{
  return x - y * floor(x / y);
}

inline float2 mod(float2 a, float2 b)
{
  return a - floor(a / b) * b;
}

inline float Rand(float c)
{
  return frac(sin(dot(float2(c, 1.0 - c), float2(12.9898, 78.233))) * 43758.5453);
}

inline float Rand(float2 c)
{
  return frac(sin(dot(c, float2(12.9898, 78.233))) * 43758.5453);
}

/// Noise by Ian McEwan, Ashima Arts.
float3 mod289(float3 x)  { return x - floor(x * (1.0 / 289.0)) * 289.0; }
float2 mod289(float2 x)  { return x - floor(x * (1.0 / 289.0)) * 289.0; }
float3 permute(float3 x) { return mod289(((x * 34.0) + 1.0) * x); }
float snoise(float2 v)
{
  const float4 C = float4(0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439);
  
  float2 i  = floor(v + dot(v, C.yy) );
  float2 x0 = v -   i + dot(i, C.xx);
  
  float2 i1;
  i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
  
  float4 x12 = x0.xyxy + C.xxzz;
  x12.xy -= i1;
  
  i = mod289(i);
  float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));
  
  float3 m = max(0.5 - float3(dot(x0,x0), dot(x12.xy,x12.xy), dot(x12.zw,x12.zw)), 0.0);
  m = m*m;
  m = m*m;
  
  float3 x = 2.0 * frac(p * C.www) - 1.0;
  float3 h = abs(x) - 0.5;
  float3 ox = floor(x + 0.5);
  float3 a0 = x - ox;
  
  m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
  
  float3 g;
  g.x  = a0.x  * x0.x  + h.x  * x0.y;
  g.yz = a0.yz * x12.xz + h.yz * x12.yw;

  return 130.0 * dot(m, g);
}

// RGB -> HSV http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl.
inline float3 RGB2HSV(float3 c)
{
  const float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
  const float Epsilon = 1.0e-10;

  float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
  float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

  float d = q.x - min(q.w, q.y);

  return float3(abs(q.z + (q.w - q.y) / (6.0 * d + Epsilon)), d / (q.x + Epsilon), q.x);
}

// HSV -> RGB http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl.
inline float3 HSV2RGB(float3 c)
{
  const float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
  float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);

  return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

// HUE -> RGB.
inline float3 HUE2RGB(float h)
{
  float r = abs(h * 6.0 - 3.0) - 1.0;
  float g = 2.0 - abs(h * 6.0 - 2.0);
  float b = 2.0 - abs(h * 6.0 - 4.0);

  return saturate(float3(r, g, b));
}

#ifdef COLOR_CONTROLS

// Color adjust.
inline float3 ColorAdjust(float3 pixel)
{
  // Brightness.
  pixel += _Brightness;

  // Contrast.
  pixel = (pixel - 0.5) * ((1.015 * (_Contrast + 1.0)) / (1.015 - _Contrast)) + 0.5;

  // Hue & saturation.
  float3 hsv = RGB2HSV(pixel);

  hsv.x += _Hue;
  hsv.y *= _Saturation;

  pixel = HSV2RGB(hsv);

  // Gamma.
  pixel = pow(pixel, _Gamma);

  return pixel;
}
#endif

#ifdef FILM_ENABLED

inline float Fade(float t)
{
  return t * t * t * ((t * ((t * 6.0) - 15.0)) + 10.0);
}

inline float PixelBlotch(float seed, float2 uv)
{
  float x = Rand(seed);
  float y = Rand(seed + 1.0);
  float s = 0.01 * Rand(seed + 2.0);
	
  float2 p = float2(x, y) - uv;
  p.x *= _ScreenParams.x / _ScreenParams.y;
	
  float a = atan2(p.y, p.x);
  float ss = s * s * (sin(6.2831 * a * x) * 0.1 + _FilmBlotchSize);

  float v = 0.2;
  
  if (dot(p, p) > ss)
   v = pow(dot(p, p) - ss, 0.0625);

  return lerp(0.3 + 0.2 * (1.0 - (s / 0.02)), 1.0, v);
}

inline float PixelLine(float seed, float2 uv)
{
  float a = Rand(seed + 1.0);
  float b = 0.01 * Rand(seed);
  float c = Rand(seed + 2.0) - 0.5;
  float mu = Rand(seed + 3.0);

  float l = 1.0;
  if (mu > 0.2)
    l = pow(abs(a * uv.x + b * uv.y + c ), _FilmLinesStrength);
  else
    l = 2.0 - pow(abs(a * uv.x + b * uv.y + c), _FilmLinesStrength);

  return lerp(0.5, 1.0, l);
}

inline float3 Overlay(float3 src, float3 dst)
{
  return float3((dst.x <= 0.5) ? (2.0 * src.x * dst.x) : (1.0 - 2.0 * (1.0 - dst.x) * (1.0 - src.x)),
                (dst.y <= 0.5) ? (2.0 * src.y * dst.y) : (1.0 - 2.0 * (1.0 - dst.y) * (1.0 - src.y)),
                (dst.z <= 0.5) ? (2.0 * src.z * dst.z) : (1.0 - 2.0 * (1.0 - dst.z) * (1.0 - src.z)));
}

// Film.
inline float3 PixelFilm(float3 pixel, float2 uv)
{
  // Grain.
  float noise = snoise(uv * float2(1024.0 + _RandomValue.x * 512.0, 1024.0 + _RandomValue.y * 512.0)) * 0.5;
  pixel += noise * _FilmGrainStrength;

  float t = float(int(_Time.y * 15.0));

  // Blotches.
  for (int i = 0; i < _FilmBlotches; ++i)
    pixel *= PixelBlotch(t + 10.0 * float(i), uv);
  
  // Lines.
  for (int j = 0; j < _FilmLines; ++j)
    pixel *= PixelLine(t + 10.0 * float(j), uv);

  // Scratches.
  if (_FilmScratches > 0.0 && _RandomValue.z < _FilmScratches)
  {
    float dist = 1.0 / _FilmScratches;
    float d = distance(uv, float2(_RandomValue.y * dist, _RandomValue.x * dist));
    if (d < 0.4)
    {
      float xPeriod = 8.0;
      float yPeriod = 1.0;
      float pi = 3.141592;
      float phase = _Time.y;
      float turbulence = snoise(uv * 2.5);
      float vScratch = 0.5 + (sin(((uv.x * xPeriod + uv.y * yPeriod + turbulence)) * pi + phase) * 0.5);
      vScratch = clamp((vScratch * 10000.0) + 0.35, 0.0, 1.0);
      
      pixel *= vScratch;
    }
  }

  // Blink.
  pixel *= (1.0 - _FilmBlinkStrenght) + _FilmBlinkStrenght * sin(_FilmBlinkSpeed * _Time.y);

  return pixel;
}

// Natural Vignette (https://en.wikipedia.org/wiki/Vignetting#Natural_vignetting).
inline float3 NaturalVignette(float3 pixel, float2 uv)
{
  float2 coord = (uv - 0.5) * (_ScreenParams.x / _ScreenParams.y) * 2.0;

  float rf = sqrt(dot(coord, coord)) * _Vignette;
  float rf2_1 = rf * rf + 1.0;
  float e = 1.0 / (rf2_1 * rf2_1);

  return pixel * e;
}
#endif

// CRT.
#ifdef CRT_ENABLED

inline float2 CRT(float2 uv)
{
  uv = (uv - 0.5) * 2.0;

  uv *= 0.5;	

  uv.x *= 1.0 + pow((abs(uv.y) * 0.5), 2.0);
  uv.y *= 1.0 + pow((abs(uv.x) * 0.5), 2.0);

  uv  = (uv / 1.0) + 0.5;

  return uv;
}

inline float2 ScanDistort(float2 uv)
{
  const float speed = 2.0;
  const float frecuency = 0.85;
  
  float scan1 = clamp(cos(uv.y * speed + _Time.y * frecuency), 0.0, 1.0);
  float scan2 = clamp(cos(uv.y * speed + _Time.y * frecuency + 4.0) * 10.0, 0.0, 1.0);

  float amount = scan1 * scan2 * uv.x;

  uv.x -= lerp(0.1 * _RandomValue.x * amount, amount, 0.9) * _ScanDistort;

  return uv;
}

inline float3 ColorShift(float2 uv)
{
  uv = ScanDistort(uv);

  float3 rand = tex2D(_NoiseTex, float2(_Time.y * 0.01, _Time.y * 0.02)).rgb * 2.0 * _ColorShift;

  float3 pixel = float3(0.0, 0.0, 0.0);
  pixel.r = tex2D(_MainTex, CRT(float2(uv.x, uv.y + 0.025 * rand.r * sin(uv.y * _ScreenParams.y * 0.01 + _Time.y)))).r;
  pixel.g = tex2D(_MainTex, CRT(float2(uv.x, uv.y + 0.01 * rand.g * sin(uv.y * _ScreenParams.y * 0.01 + _Time.y)))).g;
  pixel.b = tex2D(_MainTex, CRT(float2(uv.x, uv.y + 0.024 * rand.b * sin(uv.y * _ScreenParams.y * 0.01 + _Time.y)))).b;

  return pixel;
}

inline float3 ReflexionShineAmbient(float2 uv, float3 pixel)
{
  // Shine.
  pixel += max(0.0, _ReflexionShine - distance(uv, float2(0.5, 1.0)));

  // Ambient.
  pixel += max(0.0, _ReflexionAmbient - 0.5 * distance(uv, float2(0.5, 0.5)));

  return pixel;
}

inline float3 PixelCRT(float3 pixel, float2 uv)
{
  float2 sdUV = ScanDistort(uv);
  float2 crtUV = CRT(sdUV);

  // Scan line.
  float scanline = sin(_ScreenParams.y * crtUV.y * _ScanLine - _Time.y * 10.0);

  // Slow scan.
  float slowscan = sin(_ScreenParams.y * crtUV.y * _SlowScan + _Time.y * 6.0);

  // Shine and ambient reflexion.
  pixel = ReflexionShineAmbient(uv, pixel);

  // Vignette.
	uv -= 0.5;
  float vignette = clamp(pow(cos(uv.x * 3.1415), 1.2 * _CRTVignette) * pow(cos(uv.y * 3.1415), 1.2 * _CRTVignette) * 50.0 * _CRTVignetteAperture, 0.0, 1.0);
  
  return lerp(pixel, lerp(scanline, slowscan, 0.5), 0.05) * vignette;
}
#endif

// Color levels.
inline float3 PixelLevels(sampler2D levels, float3 pixel)
{
  pixel.r = SampleTexture(levels, float2(pixel.r, 1.0 - 0.16666)).r;
  pixel.g = SampleTexture(levels, float2(pixel.g, 0.5)).g;
  pixel.b = SampleTexture(levels, float2(pixel.b, 1.0 - 0.83333)).b;

  return pixel;
}

// Texture overlay.
inline float3 PixelBlowoutOverlay(sampler2D blowout, sampler2D overlay, float2 uv, float3 pixel)
{
  float3 bo = SampleTexture(blowout, uv).rgb;

  float3 final;
  final.r = SampleTexture(overlay, float2(pixel.r, 1.0 - bo.r)).r;
  final.g = SampleTexture(overlay, float2(pixel.g, 1.0 - bo.g)).g;
  final.b = SampleTexture(overlay, float2(pixel.b, 1.0 - bo.b)).b;

  return final;
}

// Texture overlay.
inline float3 PixelBlowoutOverlayStrength(sampler2D blowout, sampler2D overlay, float2 uv, float3 pixel, float strength)
{
  float3 bo = SampleTexture(blowout, uv).rgb;

  float3 final;
  final.r = SampleTexture(overlay, float2(pixel.r, 1.0 - bo.r)).r;
  final.g = SampleTexture(overlay, float2(pixel.g, 1.0 - bo.g)).g;
  final.b = SampleTexture(overlay, float2(pixel.b, 1.0 - bo.b)).b;

  return lerp(pixel, final, strength);
}

// Do not use.
inline float3 PixelDemo(float3 pixel, float3 final, float2 uv)
{
  float separator = (sin(_Time.x * 15.0) * 0.25) + 0.7;
  const float separatorWidth = _MainTex_TexelSize.x * 4.0;

  if (uv.x > separator)
    final = pixel;
  else if (abs(uv.x - separator) < separatorWidth)
    final = pixel * float3(1.0, 0.0, 0.0);

  return final;
}

struct appdata_t
{
  float4 vertex : POSITION;
  half2 texcoord : TEXCOORD0;
  UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
  float4 vertex : SV_POSITION;
  half2 texcoord : TEXCOORD0;
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO    
};

v2f vert(appdata_t v)
{
  v2f o;
  UNITY_SETUP_INSTANCE_ID(v);
  UNITY_TRANSFER_INSTANCE_ID(v, o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  o.vertex = UnityObjectToClipPos(v.vertex);

#if UNITY_UV_STARTS_AT_TOP
  if (_MainTex_TexelSize.y < 0)
    o.texcoord.y = 1.0 - o.texcoord.y;
#endif

#if UNITY_HALF_TEXEL_OFFSET
  o.texcoord.xy += _MaskTex_TexelSize.xy * float2(-0.5, 0.5);
#endif
	
  o.texcoord = v.texcoord;

  return o;
}

#endif