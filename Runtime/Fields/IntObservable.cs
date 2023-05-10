using System;

namespace VaporObservables
{
    [Serializable]
    public class IntObservable : ObservableField
    {
        public static implicit operator int(IntObservable f) => f.Value;

        public int Value { get; protected set; }
        public bool HasFlag(int flagToCheck) => (Value & flagToCheck) != 0;
        public event Action<IntObservable, int> ValueChanged; // Value and Delta


        public IntObservable(int fieldID, bool saveValue, int value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Int32;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(int value)
        {
            if (Value != value)
            {
                int oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value - oldValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool InternalModify(int value, ObservableModifyType type) => type switch
        {
            ObservableModifyType.Set => InternalSet(value),
            ObservableModifyType.Add => InternalSet(Value + value),
            ObservableModifyType.Multiplier => InternalSet(Value * value),
            ObservableModifyType.PercentAdd => InternalSet(Value + Value * value),
            _ => false,
        };

        public bool Set(int value)
        {
            return InternalSet(value);
        }

        public bool Modify(int value, ObservableModifyType type)
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
            return new IntObservable(FieldID, SaveValue, Value);
        }
    }
}
