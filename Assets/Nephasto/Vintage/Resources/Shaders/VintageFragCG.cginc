///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef VINTAGE_FRAG_CGINC
#define VINTAGE_FRAG_CGINC

inline float4 vintageFrag(v2f i) : SV_Target
{
  UNITY_SETUP_INSTANCE_ID(i);

  float3 pixel = SampleMainTexture(i.texcoord);
  float3 final = pixel;

#ifdef MODE_SCREEN
  final = Vintage(pixel, i.texcoord);
#elif MODE_LAYER
  float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.texcoord));

  float3 rtt = tex2D(_RTT, i.texcoord).rgb;

  float mask = rtt > float3(0.0, 0.0, 0.0) ? 0.0 : 1.0;

  if (mask - depth < _DepthThreshold)
    final = Vintage(pixel, i.texcoord);
#elif MODE_DISTANCE
  final = Vintage(pixel, i.texcoord);

  float depth = Linear01Depth(SAMPLE_RAW_DEPTH_TEXTURE(_CameraDepthTexture, i.texcoord));
  
  float curve = clamp(tex2D(_DistanceTex, float2(depth, 0.0)).x, 0.0, 1.0);

  final = lerp(pixel, final, curve);
#endif

#ifdef COLOR_CONTROLS
  final = ColorAdjust(final);
#endif

#ifdef FILM_ENABLED
  final = PixelFilm(final, i.texcoord);
#endif

#ifdef CRT_ENABLED
  final = PixelCRT(final, i.texcoord);
#endif

#ifdef FILM_ENABLED
  final = NaturalVignette(final, i.texcoord);
#endif

#ifdef VINTAGE_DEMO
  final = PixelDemo(tex2D(_MainTex, i.texcoord).rgb, final, i.texcoord);
#endif

  final = lerp(pixel, final, _Strength);

#if UNITY_COLORSPACE_GAMMA
  pixel = LinearToGammaSpace(pixel);
  final = LinearToGammaSpace(final);
#endif

#if DEMO
  final = PixelDemo(pixel, final, i.texcoord);
#endif

  return float4(final, 1.0);
}

#endif