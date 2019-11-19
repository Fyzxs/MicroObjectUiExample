namespace ExtractedCore {
    internal sealed class NotAuthenticated_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public NotAuthenticated_LogInAction() : this(
            new ClearPassword_LogInAction(
                new ShowError_LogInAction(
                    new NoOp_LogInAction()))) { }
        private NotAuthenticated_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm) => _nextAction.Act(mainForm);
    }
}