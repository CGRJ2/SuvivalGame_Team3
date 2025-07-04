using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObservableProperty<T>
{
    [SerializeField] private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (object.Equals(_value, value)) return;
            _value = value;
            Notify();
        }
    }
    private UnityEvent<T> _onValueChanged = new();

    public ObservableProperty(T value = default)
    {
        _value = value;
    }

    public void Subscribe(UnityAction<T> action)
    {
        _onValueChanged.AddListener(action);
    }

    public void Unsubscribe(UnityAction<T> action)
    {
        _onValueChanged.RemoveListener(action);
    }

    public void UnsbscribeAll()
    {
        _onValueChanged.RemoveAllListeners();
    }

    private void Notify()
    {
        _onValueChanged?.Invoke(Value);
    }

}
