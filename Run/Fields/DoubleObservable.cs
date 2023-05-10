using System;

namespace VaporObservables
{
    [Serializable]
    public class DoubleObservable : ObservableField
    {
        public static implicit operator double(DoubleObservable f) => f.Value;

        public double Value { get; protected set; }
        public event Action<DoubleObservable, double> ValueChanged;

        public DoubleObservable(int fieldID, bool saveValue, double value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Double;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(double value)
        {
            if (Value != value)
            {
                double oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value - oldValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool InternalModify(double value, ObservableModifyType type) => type switch
        {
            ObservableModifyType.Set => InternalSet(value),
            ObservableModifyType.Add => InternalSet(Value + value),
            ObservableModifyType.Multiplier => InternalSet(Value * value),
            ObservableModifyType.PercentAdd => InternalSet(Value + Value * value),
            _ => false,
        };

        public bool Set(double value)
        {
            return InternalSet(value);
        }

        public bool Modify(double value, ObservableModifyType type)
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
            return new DoubleObservable(FieldID, SaveValue, Value);
        }
    }
}
