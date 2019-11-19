namespace ExtractedCore {
    internal sealed class HardCodedAuthentication : IAuthentication
    {
        public bool IsAuthenticated(IAuthnRequest authnRequest)
        {
            return authnRequest.UserName().Matches("Quinn") &&
                   authnRequest.Password().Matches("IsAwesome");
        }

        public bool IsNotAuthenticated(IAuthnRequest authnRequest) => !IsAuthenticated(authnRequest);
    }
}