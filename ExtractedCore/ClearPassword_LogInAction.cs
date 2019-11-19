namespace ExtractedCore {
    internal sealed class ClearPassword_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public ClearPassword_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            mainForm.Password().Clear();
            _nextAction.Act(mainForm);
        }
    }
}