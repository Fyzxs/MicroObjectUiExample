namespace ExtractedCore {
    public sealed class Authenticate_LogInAction: ILogInAction
    {
        private readonly IAuthentication _authentication;
        private readonly ILogInAction _isAuthnAction;
        private readonly ILogInAction _notAuthnAction;

        public Authenticate_LogInAction():this(
            new HardCodedAuthentication(), new IsAuthenticated_LogInAction(), new NotAuthenticated_LogInAction()){}

        private Authenticate_LogInAction(IAuthentication authentication, ILogInAction isAuthnAction, ILogInAction notAuthnAction)
        {
            _authentication = authentication;
            _isAuthnAction = isAuthnAction;
            _notAuthnAction = notAuthnAction;
        }

        public void Act(IMainForm mainForm)
        {
            if (_authentication.IsNotAuthenticated(mainForm.AuthnRequest()))
            {
                _notAuthnAction.Act(mainForm);
                return;
            }

            _isAuthnAction.Act(mainForm);
        }
    }
}