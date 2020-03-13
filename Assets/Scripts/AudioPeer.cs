using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioPeerMode
{
    SAMPLE,
    FREQUENCY_BAND,
    BAND_BUFFER
}

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;

    static public float[] _samples = new float[512];
    static public float[] _freqBand = new float[8];
    static public float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        /** 
        22050 / 512 = 43 hertz per sample
        
        we divided sound in 7 ranges as part 1
        20-60 hertz
        60-250
        250-500
        500-2000
        2000-4000
        4000-6000
        6000-20000

        we optimize into 8 frequency bands
        index: 0 -> sample: 2 -> total: 86 hertz -> ranges: 0-86
        index: 1 -> sample: 4 -> total: 172 hertz -> ranges: 87-258
        index: 2 -> sample: 8 -> total: 344 hertz -> ranges: 259-602
        index: 3 -> sample: 16 -> total: 688 hertz -> ranges: 603-1290
        index: 4 -> sample: 32 -> total: 1376 hertz -> ranges: 1291-2666
        index: 5 -> sample: 64 -> total: 2752 hertz -> ranges: 2667-5418
        index: 6 -> sample: 128 -> total: 5504 hertz -> ranges: 5419-10922
        index: 7 -> sample: 256 -> total: 11008 hertz -> ranges: 10923-21930
        ========================================================================
                            510 samples
        */
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);
            if (i == 7)
            {
                sampleCount += 2;
            }
            // Debug.Log("sample count" + sampleCount);

            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }

            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }
}
