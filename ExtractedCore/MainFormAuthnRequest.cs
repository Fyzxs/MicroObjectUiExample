namespace ExtractedCore {
    public sealed class MainFormAuthnRequest : IAuthnRequest
    {
        private readonly IMainForm _mainForm;

        public MainFormAuthnRequest(IMainForm mainForm) => _mainForm = mainForm;

        public UserName UserName() => _mainForm.UserNameControl();

        public IPassword Password() => _mainForm.Password();
    }
}