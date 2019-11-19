namespace ExtractedCore {
    internal sealed class ShowError_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public ShowError_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            mainForm.ErrorLabelVisibility().Show();
            _nextAction.Act(mainForm);
        }
    }
}