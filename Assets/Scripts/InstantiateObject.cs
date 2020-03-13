using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject _sampleObject;
    public int _num;
    public float _radius;
    public float _maxScale;

    public AudioPeerMode _AudioPeerMode;

    GameObject[] _samples;

    private void Awake()
    {
        _samples = new GameObject[_num];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _num; i++)
        {

            GameObject cube = Instantiate(_sampleObject);
            cube.transform.parent = this.transform;
            cube.name = "SampleCube" + i;


            this.transform.eulerAngles = new Vector3(0, -360.0f * i / _num, 0);
            cube.transform.position = Vector3.forward * _radius;

            // float angle = (-360.0f * i / _num) * Mathf.Deg2Rad;
            // cube.transform.localPosition = new Vector3(Mathf.Sin(angle) * _radius, 0, Mathf.Cos(angle) * _radius);
            // cube.transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg, 0);

            _samples[i] = cube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _num; i++)
        {
            if (_samples != null)
            {
                if (_AudioPeerMode == AudioPeerMode.SAMPLE)
                {
                    _samples[i].transform.localScale = new Vector3(1, (AudioPeer._samples[i] * _maxScale) + 2, 1);
                }
                else if (_AudioPeerMode == AudioPeerMode.FREQUENCY_BAND)
                {
                    _samples[i].transform.localScale = new Vector3(1, (AudioPeer._freqBand[i] * _maxScale) + 2, 1);
                }
                else if (_AudioPeerMode == AudioPeerMode.BAND_BUFFER)
                {
                    _samples[i].transform.localScale = new Vector3(1, (AudioPeer._bandBuffer[i] * _maxScale) + 2, 1);
                }
            }
        }
    }
}
