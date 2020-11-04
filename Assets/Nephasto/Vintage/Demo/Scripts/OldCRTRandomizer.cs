///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Nephasto <hello@nephasto.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

using Nephasto.VintageAsset;

/// <summary>
/// Randomize the values of VintageOldCRT.
/// </summary>
[RequireComponent(typeof(VintageOldCRT))]
public sealed class OldCRTRandomizer : MonoBehaviour
{
  [SerializeField, Range(0.2f, 5.0f)]
  private float waitTimeMax = 3.0f;

  [SerializeField, Range(0.0f, 1.0f)]
  private float noiseTimeMax = 0.5f;

  [SerializeField, Range(0.0f, 1.0f)]
  private float noisePowerMax = 0.4f;

  [SerializeField, Range(0.0f, 5.0f)]
  private float offsetMax = 5.0f;

  [SerializeField, Range(0.0f, 5.0f)]
  private float baseOffsetMax = 0.5f;

  [SerializeField, Range(0.0f, 30.0f)]
  private float noiseSinWidthMax = 10.0f;

  private VintageOldCRT oldCRT;

  private float wait = 0.0f;
  private float waitTotal = 0.0f;

  private float baseNoisePower = 0.0f;
  private float noisePower = 0.0f;
  private float noisyTime = 0.0f;
  private Vector2 offset = Vector2.zero;
  private Vector2 baseOffset = Vector2.zero;

  private void OnEnable()
  {
    oldCRT = this.gameObject.GetComponent<VintageOldCRT>();

    baseNoisePower = Mathf.Clamp01(Random.Range(-0.01f, 0.01f));

    wait = waitTotal = Random.Range(0.2f, waitTimeMax);

    this.enabled = oldCRT != null;
  }

  private void Update()
  {
    float t = wait / waitTotal;
    float nt = Mathf.Clamp01(t / noisyTime);
    float np = baseNoisePower + noisePower * (1.0f - nt);

    oldCRT.NoiseX = np * 0.5f;
    oldCRT.NoiseRGB = np * 0.7f;
    oldCRT.NoiseSinScale = np * 1.0f;
    oldCRT.NoiseSinOffset += Time.deltaTime * 2.0f;
    oldCRT.Offset = baseOffset + offset * (np + baseNoisePower * t * 5.0f);

    if (wait <= 0.0f)
    {
      wait = waitTotal = Random.Range(waitTimeMax * 0.5f, waitTimeMax);

      noisyTime = Random.Range(0.0f, noiseTimeMax);
      noisePower = Random.Range(0.0f, noisePowerMax);
      offset = Vector2.right * Random.Range(-offsetMax, offsetMax) + Vector2.up * Random.Range(-offsetMax, offsetMax);
      baseOffset = (Vector2.right * Random.Range(-baseOffsetMax, baseOffsetMax) + Vector2.up * Random.Range(-baseOffsetMax, baseOffsetMax)) * 0.05f;

      oldCRT.NoiseSinWidth = Random.Range(0.0f, noiseSinWidthMax);
    }
    else
      wait -= Time.deltaTime;
  }
}
