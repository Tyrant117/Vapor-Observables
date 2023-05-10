using System;

namespace VaporObservables
{
    [Serializable]
    public class ShortObservable : ObservableField
    {
        public static implicit operator short(ShortObservable f) => f.Value;

        public short Value { get; protected set; }
        public event Action<ShortObservable, int> ValueChanged;

        public ShortObservable(int fieldID, bool saveValue, short value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Int16;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(short value)
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

        internal bool InternalModify(short value, ObservableModifyType type)
        {
            return type switch
            {
                ObservableModifyType.Set => InternalSet(value),
                ObservableModifyType.Add => InternalSet((short)(Value + value)),
                ObservableModifyType.Multiplier => InternalSet((short)(Value * value)),
                ObservableModifyType.PercentAdd => InternalSet((short)(Value + Value * value)),
                _ => false,
            };
        }

        public bool Set(short value)
        {
            return InternalSet(value);
        }

        public bool Modify(short value, ObservableModifyType type)
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
            return new ShortObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}