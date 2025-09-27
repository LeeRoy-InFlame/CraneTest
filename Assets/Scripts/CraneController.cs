using UnityEngine;

public class CraneController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource _hookAudio;
    [SerializeField] private AudioSource _carriageAudio;
    [SerializeField] private AudioSource _beamAudio;

    [Header("Hook and transform")]
    [SerializeField] private Transform _hook;
    [SerializeField] private Transform _beam;
    [SerializeField] private Transform _carriage;
    [SerializeField] private Transform _tube;

    [Header("Limits (set empty GameObjects in scene)")]
    [SerializeField] private Transform _hookMin;
    [SerializeField] private Transform _hookMax;
    [SerializeField] private Transform _carriageMin;
    [SerializeField] private Transform _carriageMax;
    [SerializeField] private Transform _beamMin;
    [SerializeField] private Transform _beamMax;

    [Header("Speeds (units/sec)")]
    [SerializeField] private float _speedUpDown = 1f;
    [SerializeField] private float _speedEastWest = 1f;
    [SerializeField] private float _speedNorthSouth = 1f;

    Vector3 _hookSpeed = Vector3.zero;
    Vector3 _beamSpeed = Vector3.zero;
    Vector3 _carriageSpeed = Vector3.zero;

    Vector3 _hookVelocity = Vector3.zero;
    Vector3 _beamVelocity = Vector3.zero;
    Vector3 _carriageVelocity = Vector3.zero;

    [SerializeField] private float _smoothing = 10f;

    void OnEnable()
    {
        CraneSignals.OnMoveUpDown += OnMoveUpDown;
        CraneSignals.OnMoveEastWest += OnMoveEastWest;
        CraneSignals.OnMoveNorthSouth += OnMoveNorthSouth;
    }

    void OnDisable()
    {
        CraneSignals.OnMoveUpDown -= OnMoveUpDown;
        CraneSignals.OnMoveEastWest -= OnMoveEastWest;
        CraneSignals.OnMoveNorthSouth -= OnMoveNorthSouth;
    }

    void OnMoveUpDown(float v) => _hookVelocity.y = v * _speedUpDown;
    void OnMoveEastWest(float v) => _carriageVelocity.x = v * _speedEastWest;
    void OnMoveNorthSouth(float v) => _beamVelocity.z = v * _speedNorthSouth;

    void Update()
    {
        // плавное приближение
        _hookSpeed = Vector3.Lerp(_hookSpeed, _hookVelocity, Time.deltaTime * _smoothing);
        _beamSpeed = Vector3.Lerp(_beamSpeed, _beamVelocity, Time.deltaTime * _smoothing);
        _carriageSpeed = Vector3.Lerp(_carriageSpeed, _carriageVelocity, Time.deltaTime * _smoothing);

        // движение
        _hook.localPosition += _hookSpeed * Time.deltaTime;
        _beam.localPosition += _beamSpeed * Time.deltaTime;
        _carriage.localPosition += _carriageSpeed * Time.deltaTime;
        _tube.Rotate((Vector3.right * _hookSpeed.y * Time.deltaTime) * 100);

        // --- Ограничители ---
        ClampToLimits(_hook, _hookMin, _hookMax, Axis.Y);
        ClampToLimits(_carriage, _carriageMin, _carriageMax, Axis.X);
        ClampToLimits(_beam, _beamMin, _beamMax, Axis.Z);

        // --- ЗВУК ---
        HandleAudio(_hookAudio, _hookSpeed.y);
        HandleAudio(_carriageAudio, _carriageSpeed.x);
        HandleAudio(_beamAudio, _beamSpeed.z);
    }

    private void HandleAudio(AudioSource source, float velocity)
    {
        if (Mathf.Abs(velocity) > 0.01f)
        {
            if (!source.isPlaying)
                source.Play();

            source.pitch = 1f + Mathf.Abs(velocity) * 0.2f;
            source.volume = 0.6f + Mathf.Abs(velocity) * 0.4f;
        }
        else
        {
            if (source.isPlaying)
                source.Stop();
        }
    }

    private enum Axis { X, Y, Z }

    private void ClampToLimits(Transform target, Transform minT, Transform maxT, Axis axis)
    {
        if (minT == null || maxT == null) return;

        Vector3 pos = target.localPosition;

        switch (axis)
        {
            case Axis.X:
                pos.x = Mathf.Clamp(pos.x, minT.localPosition.x, maxT.localPosition.x);
                break;
            case Axis.Y:
                pos.y = Mathf.Clamp(pos.y, minT.localPosition.y, maxT.localPosition.y);
                break;
            case Axis.Z:
                pos.z = Mathf.Clamp(pos.z, minT.localPosition.z, maxT.localPosition.z);
                break;
        }

        target.localPosition = pos;
    }
}
