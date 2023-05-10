using System;

namespace VaporObservables
{
    [Serializable]
    public class ULongObservable : ObservableField
    {
        public static implicit operator ulong(ULongObservable f) => f.Value;

        public ulong Value { get; protected set; }
        public event Action<ULongObservable> ValueChanged;

        public ULongObservable(int fieldID, bool saveValue, ulong value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.UInt64;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(ulong value)
        {
            if (Value != value)
            {
                Value = value;
                ValueChanged?.Invoke(this);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Set(ulong value)
        {
            return InternalSet(value);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, Value.ToString());
        }

        public override ObservableField Clone()
        {
            return new ULongObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}