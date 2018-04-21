using System;

namespace GamepadMapper.Configuration
{
    public interface IConfigDescriptor
    {
        string Key { get; }

        object GetValue();

        string FormatValue();

        bool isValidValue(string token);

        void SetValue(string token);

        void Increment();

        void Decrement();

        void Toggle();

        void Reset();
    }

    public class ConfigDescriptor : IConfigDescriptor
    {
        public ConfigDescriptor(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public Func<object> GetValue { get; set; }

        object IConfigDescriptor.GetValue() => GetValue?.Invoke();

        public Func<string> FormatValue { get; set; }

        string IConfigDescriptor.FormatValue() => FormatValue?.Invoke();

        public Func<string, bool> IsValidValue { get; set; }

        bool IConfigDescriptor.isValidValue(string token) => IsValidValue?.Invoke(token) ?? true;

        public Action<string> SetValue { get; set; }

        void IConfigDescriptor.SetValue(string token) => SetValue?.Invoke(token);

        public System.Action Increment { get; set; }

        void IConfigDescriptor.Increment() => Increment?.Invoke();

        public System.Action Decrement { get; set; }

        void IConfigDescriptor.Decrement() => Decrement?.Invoke();

        public System.Action Toggle { get; set; }

        void IConfigDescriptor.Toggle() => Toggle?.Invoke();

        public System.Action Reset { get; set; }

        void IConfigDescriptor.Reset() => Reset?.Invoke();
    }
}
