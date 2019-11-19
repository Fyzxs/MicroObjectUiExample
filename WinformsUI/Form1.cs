using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExtractedCore;

namespace WinformsUI
{
    public partial class Form1 : Form, IMainForm
    {
        private readonly ILogInAction _logInAction;

        public Form1():this(new Authenticate_LogInAction()){}

        private Form1(ILogInAction logInAction)
        {
            _logInAction = logInAction;

            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e) => _logInAction.Act(this);

        public IAuthnRequest AuthnRequest() => new MainFormAuthnRequest(this);
        public IVisibility UserNameLabelVisibility() => new ControlVisibility(lblUserName);
        public IVisibility UserNameTextBoxVisibility() => new ControlVisibility(txtUserName);
        public UserName UserNameControl() => new UserNameControl(txtUserName);
        public IVisibility PasswordLabelVisibility() => new ControlVisibility(lblPassword);
        public IVisibility PasswordTextBoxVisibility() => new ControlVisibility(txtPassword);
        public IPassword Password() => new PasswordControl(txtPassword);
        public IVisibility ErrorLabelVisibility() => new ControlVisibility(lblError);
        public IVisibility LogInButtonVisibility() => new ControlVisibility(btnLogIn);
        public IVisibility WelcomeLabelVisibility() => new ControlVisibility(lblWelcome);
        public IWriteText WelcomeLabelWriter() => new ControlWriteText(lblWelcome);
        public IVisibility LogInControlsVisibility() => new LogInControlsVisibility(this);
    }
    internal sealed class LogInControlsVisibility : MultipleControlsVisibility
    {
        private readonly IMainForm _mainForm;

        public LogInControlsVisibility(IMainForm mainForm) => _mainForm = mainForm;

        protected override IEnumerable<IVisibility> Controls()
        {
            return new List<IVisibility>
            {
                _mainForm.UserNameLabelVisibility(),
                _mainForm.UserNameTextBoxVisibility(),
                _mainForm.PasswordLabelVisibility(),
                _mainForm.PasswordTextBoxVisibility(),
                _mainForm.ErrorLabelVisibility(),
                _mainForm.LogInButtonVisibility()
            };
        }
    }
    public sealed class UserNameControl : UserName
    {
        private readonly Control _control;

        public UserNameControl(Control control) => _control = control;

        public override bool Matches(string compareTo) => SystemValue().Equals(compareTo, StringComparison.InvariantCulture);

        protected override string SystemValue() => _control.Text;
    }

    internal sealed class PasswordControl : IPassword
    {
        private readonly Control _control;

        public PasswordControl(Control control) => _control = control;

        public bool Matches(string compareTo) => _control.Text == compareTo;
        public void Clear() => _control.Text = string.Empty;
    }

    internal sealed class ControlWriteText : IWriteText
    {
        private readonly Control _control;

        public ControlWriteText(Control control) => _control = control;

        public void Write(string text) => _control.Text = text;
    }

    public sealed class ControlVisibility : IVisibility
    {
        private readonly Control _control;

        public ControlVisibility(Control control) => _control = control;

        public void Show() => _control.Show();

        public void Hide() => _control.Hide();

        public void ChangeTo(Visible visible) => _control.Visible = visible;
    }

    internal abstract class MultipleControlsVisibility : IVisibility
    {

        public void Show() => ForEach(c => c.Show());

        public void Hide() => ForEach(c => c.Hide());

        public void ChangeTo(Visible visible)
        {
            if (visible)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        protected abstract IEnumerable<IVisibility> Controls();

        private void ForEach(Action<IVisibility> action)
        {
            foreach (IVisibility control in Controls())
            {
                action(control);
            }
        }
    }
}
