using System;

namespace VaporObservables
{
    [Serializable]
    public class FloatObservable : ObservableField
    {
        public static implicit operator float(FloatObservable f) => f.Value;

        public float Value { get; protected set; }
        public event Action<FloatObservable, float> ValueChanged;

        public FloatObservable(int fieldID, bool saveValue, float value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Single;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(float value)
        {
            if (Value != value)
            {
                float oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value - oldValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool InternalModify(float value, ObservableModifyType type) => type switch
        {
            ObservableModifyType.Set => InternalSet(value),
            ObservableModifyType.Add => InternalSet(Value + value),
            ObservableModifyType.Multiplier => InternalSet(Value * value),
            ObservableModifyType.PercentAdd => InternalSet(Value + Value * value),
            _ => false,
        };

        public bool Set(float value)
        {
            return InternalSet(value);
        }

        public bool Modify(float value, ObservableModifyType type)
        {
            return InternalModify(value, type);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, Value.ToString());
        }
        #endregion

        public override string ToString()
        {
            return $"{FieldID} [{Value}]";
        }

        public override ObservableField Clone()
        {
            return new FloatObservable(FieldID, SaveValue, Value);
        }
    }
}
