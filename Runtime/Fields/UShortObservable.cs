using System;

namespace VaporObservables
{
    public class UShortObservable : ObservableField
    {
        public static implicit operator ushort(UShortObservable f) => f.Value;

        public ushort Value { get; protected set; }
        public event Action<UShortObservable> ValueChanged;

        public UShortObservable(int fieldID, bool saveValue, ushort value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.UInt16;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(ushort value)
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

        public bool Set(ushort value)
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
            return new UShortObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}
