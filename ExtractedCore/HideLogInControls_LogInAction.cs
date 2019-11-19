namespace ExtractedCore {
    internal sealed class HideLogInControls_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;
        public HideLogInControls_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            mainForm.LogInControlsVisibility().Hide();
            _nextAction.Act(mainForm);
        }
    }
}