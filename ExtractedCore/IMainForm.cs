namespace ExtractedCore {
    public interface IMainForm
    {
        IVisibility UserNameLabelVisibility();
        IVisibility UserNameTextBoxVisibility();
        UserName UserNameControl();
        IVisibility PasswordLabelVisibility();
        IVisibility PasswordTextBoxVisibility();
        IPassword Password();
        IVisibility ErrorLabelVisibility();
        IVisibility LogInButtonVisibility();
        IVisibility WelcomeLabelVisibility();
        IWriteText WelcomeLabelWriter();
        IVisibility LogInControlsVisibility();
        IAuthnRequest AuthnRequest();
    }
}