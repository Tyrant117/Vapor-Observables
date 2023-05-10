using System;

namespace VaporObservables
{
    [Serializable]
    public class LongObservable : ObservableField
    {
        public static implicit operator long(LongObservable f) => f.Value;

        public long Value { get; protected set; }
        public event Action<LongObservable, long> ValueChanged;

        public LongObservable(int fieldID, bool saveValue, long value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Int64;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(long value)
        {
            if (Value != value)
            {
                var oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value - oldValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool InternalModify(long value, ObservableModifyType type)
        {
            return type switch
            {
                ObservableModifyType.Set => InternalSet(value),
                ObservableModifyType.Add => InternalSet(Value + value),
                ObservableModifyType.Multiplier => InternalSet(Value * value),
                ObservableModifyType.PercentAdd => InternalSet(Value + Value * value),
                _ => false,
            };
        }

        public bool Set(long value)
        {
            return InternalSet(value);
        }

        public bool Modify(long value, ObservableModifyType type)
        {
            return InternalModify(value, type);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, Value.ToString());
        }

        public override ObservableField Clone()
        {
            return new LongObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}