namespace ExtractedCore {
    internal sealed class ShowAuthenticatedControls_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public ShowAuthenticatedControls_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            mainForm.WelcomeLabelVisibility().Show();
            _nextAction.Act(mainForm);
        }
    }
}