using System;

namespace VaporObservables
{
    [Serializable]
    public class StringObservable : ObservableField
    {
        public static implicit operator string(StringObservable f) => f.Value;

        public string Value { get; protected set; }
        /// <summary>
        /// Returns the new and old values of the changed string. New -> Old
        /// </summary>
        public event Action<StringObservable, string, string> ValueChanged;

        public StringObservable(int fieldID, bool saveValue, string value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.String;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(string value)
        {
            if (Value != value)
            {
                string oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value, oldValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Set(string value)
        {
            return InternalSet(value);
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
            return new StringObservable(FieldID, SaveValue, Value);
        }
    }
}
