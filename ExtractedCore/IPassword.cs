namespace ExtractedCore {
    public interface IPassword
    {
        bool Matches(string compareTo);
        void Clear();
    }
}