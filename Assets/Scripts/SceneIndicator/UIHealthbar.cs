using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthbar : UIIndicator
{
    [SerializeField] private Image _damageBar;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _decreaseSpeed = 2f;
    [SerializeField] private float _decreaseReducer = .1f;
    [SerializeField] private float _fadeOutTime = .2f;

    private SquadHealth _healthHandler;
    private CanvasGroup _canvasGroup;
    private Slider _slider;

    private int targetValue = 0;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (SceneIndicator.Squad == null) return;

        _healthHandler = SceneIndicator.Squad.SquadHealth;
        _slider.maxValue = _healthHandler.maxHealth;
        //_slider.value = _healthHandler.currentHealth;
        //_damageBar.fillAmount = _healthHandler.currentHealth;
        _healthHandler.eventOnChangeHealth.AddListener(SetTargetValue);
        SetTargetValue(_healthHandler.currentHealth);
        StartCoroutine(FadeIn());

    }

    public void UpdateBar(int health)
    {
        if (isActiveAndEnabled == false) return;

        StopAllCoroutines();

        if (health <= 0f)
        {
            StartCoroutine(FadeOut());
        }

        _slider.value = health;
        StartCoroutine(UpdateDamageBar(health));
    }

    private IEnumerator UpdateDamageBar(float health)
    {
        yield return new WaitForSeconds(_duration);

        float t = 0f;
        float startValue = _damageBar.fillAmount;
        float speed = _decreaseSpeed;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            speed = Mathf.Max(speed - _decreaseReducer * Time.deltaTime, .05f);
            _damageBar.fillAmount = Mathf.Lerp(startValue, health, t);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        float startValue = _canvasGroup.alpha;
        float targetValue = 1f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadeOutTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);
            
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        float startValue = _canvasGroup.alpha;
        float targetValue = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadeOutTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);

            yield return new WaitForEndOfFrame();
        }
    }

    private void SetTargetValue(int value)
    {
        targetValue = value;
        UpdateBar(targetValue);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
