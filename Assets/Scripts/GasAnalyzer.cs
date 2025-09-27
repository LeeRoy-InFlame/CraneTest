using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GasAnalyzer : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _panel;            // фон дисплея
    [SerializeField] private TextMeshProUGUI _text;   // текст показаний
    [SerializeField] private Image _progress;         // индикатор удержания кнопки (например, круг/бар)

    [Header("Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;   // скорость "загорания"
    [SerializeField] private float _holdTime = 3f;         // сколько держать кнопку
    [SerializeField] private Transform _probe;

    [SerializeField] private AudioClip _onSound;
    [SerializeField] private AudioClip _offSound;

    private bool _isOn = false;
    private Coroutine _fadeRoutine;
    private Coroutine _holdRoutine;
    private GameObject _dangerZone;

    void Start()
    {
        _dangerZone = GameObject.FindGameObjectWithTag("DangerZone");
        SetPanelAlpha(0f);
        _text.text = "";
        if (_progress != null) _progress.fillAmount = 0f;
    }

    void Update()
    {
        if (_isOn && _dangerZone != null && _probe != null)
        {
            float distance = Vector3.Distance(_probe.position, _dangerZone.transform.position);
            _text.text = distance.ToString("F2") + " m";
        }
    }

    public void OnButtonPress(bool isPressed)
    {
        if (isPressed)
        {
            if (_holdRoutine == null)
                _holdRoutine = StartCoroutine(HoldToToggle());
        }
        else
        {
            if (_holdRoutine != null)
            {
                StopCoroutine(_holdRoutine);
                _holdRoutine = null;
            }
            if (_progress != null) _progress.fillAmount = 0f;
        }
    }

    private IEnumerator HoldToToggle()
    {
        float holdProgress = 0f;

        while (holdProgress < _holdTime)
        {
            holdProgress += Time.deltaTime;
            if (_progress != null)
                _progress.fillAmount = holdProgress / _holdTime;

            yield return null;
        }

        TogglePower(); // тут переключение (и запуск fade)
        if (_progress != null) _progress.fillAmount = 0f;

        _holdRoutine = null; // сброс, можно снова нажимать
    }

    private void TogglePower()
    {
        _isOn = !_isOn;

        // воспроизводим звук
        if (TryGetComponent<AudioSource>(out var audio))
        {
            if (_isOn && _onSound != null)
                audio.PlayOneShot(_onSound);
            else if (!_isOn && _offSound != null)
                audio.PlayOneShot(_offSound);
        }

        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeScreen(_isOn));
    }

    private IEnumerator FadeScreen(bool turnOn)
    {
        float start = _panel.color.a;
        float target = turnOn ? 1f : 0f;
        float t = 0f;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, target, t / _fadeDuration);
            SetPanelAlpha(alpha);
            yield return null;
        }

        SetPanelAlpha(target);

        if (!turnOn) _text.text = "";
    }

    private void SetPanelAlpha(float a)
    {
        Color c = _panel.color;
        c.a = a;
        _panel.color = c;

        Color tc = _text.color;
        tc.a = a;
        _text.color = tc;
    }
}


