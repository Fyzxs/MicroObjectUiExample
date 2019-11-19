namespace ExtractedCore {
    internal sealed class DisplayWelcome_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public DisplayWelcome_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            mainForm.WelcomeLabelWriter().Write($"Welcome {mainForm.AuthnRequest().UserName().ToSystemValue()}");
            _nextAction.Act(mainForm);
        }
    }
}