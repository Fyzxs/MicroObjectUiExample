namespace ExtractedCore {
    public abstract class UserName : ToSystem<string>
    {
        public abstract bool Matches(string compareTo);
    }
}