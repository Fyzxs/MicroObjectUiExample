namespace ExtractedCore {
    internal sealed class IsAuthenticated_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public IsAuthenticated_LogInAction() : this(
            new HideLogInControls_LogInAction(
                new ShowAuthenticatedControls_LogInAction(
                    new DisplayWelcome_LogInAction(
                        new NoOp_LogInAction())))) { }
        private IsAuthenticated_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm) => _nextAction.Act(mainForm);
    }
}