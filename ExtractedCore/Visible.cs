namespace ExtractedCore {
    public sealed class Visible
    {
        public static implicit operator bool(Visible visible) => visible._value;

        public static readonly Visible Show = new Visible(true);
        public static readonly Visible Hide = new Visible(false);

        private readonly bool _value;
        private Visible(bool value) => _value = value;
    }
}