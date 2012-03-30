namespace Aec.Cqrs
{
    public sealed class ImmutableAttribute
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public ImmutableAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Key, Value);
        }
    }
}