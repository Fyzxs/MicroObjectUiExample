namespace ExtractedCore {
    public interface IAuthnRequest
    {
        UserName UserName();
        IPassword Password();
    }
}