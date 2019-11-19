namespace ExtractedCore {
    internal interface IAuthentication
    {
        bool IsAuthenticated(IAuthnRequest authnRequest);
        bool IsNotAuthenticated(IAuthnRequest authnRequest);
    }
}