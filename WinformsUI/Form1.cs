using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

    internal sealed class Authenticate_LogInAction: ILogInAction
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

    internal sealed class NoOp_LogInAction : ILogInAction
    {
        public void Act(IMainForm mainForm) { }
    }


    public interface ILogInAction
    {
        void Act(IMainForm mainForm);
    }

    public sealed class UserNameControl : UserName
    {
        private readonly Control _control;

        public UserNameControl(Control control) => _control = control;

        public override bool Matches(string compareTo) => SystemValue().Equals(compareTo, StringComparison.InvariantCulture);

        protected override string SystemValue() => _control.Text;
    }

    internal sealed class MainFormAuthnRequest : IAuthnRequest
    {
        private readonly IMainForm _mainForm;

        public MainFormAuthnRequest(IMainForm mainForm) => _mainForm = mainForm;

        public UserName UserName() => _mainForm.UserNameControl();

        public IPassword Password() => _mainForm.Password();
    }

    public interface IAuthnRequest
    {
        UserName UserName();
        IPassword Password();
    }

    public abstract class UserName : ToSystem<string>
    {
        public abstract bool Matches(string compareTo);
    }

    internal sealed class HardCodedAuthentication : IAuthentication
    {
        public bool IsAuthenticated(IAuthnRequest authnRequest)
        {
            return authnRequest.UserName().Matches("Quinn") &&
                   authnRequest.Password().Matches("IsAwesome");
        }

        public bool IsNotAuthenticated(IAuthnRequest authnRequest) => !IsAuthenticated(authnRequest);
    }

    internal sealed class PasswordControl : IPassword
    {
        private readonly Control _control;

        public PasswordControl(Control control) => _control = control;

        public bool Matches(string compareTo) => _control.Text == compareTo;
        public void Clear() => _control.Text = string.Empty;
    }

    public interface IPassword
    {
        bool Matches(string compareTo);
        void Clear();
    }

    internal interface IAuthentication
    {
        bool IsAuthenticated(IAuthnRequest authnRequest);
        bool IsNotAuthenticated(IAuthnRequest authnRequest);
    }

    internal sealed class ControlWriteText : IWriteText
    {
        private readonly Control _control;

        public ControlWriteText(Control control) => _control = control;

        public void Write(string text) => _control.Text = text;
    }

    public interface IWriteText
    {
        void Write(string text);
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

    public interface IVisibility
    {
        void Show();
        void Hide();
        void ChangeTo(Visible visible);
    }

    public sealed class Visible
    {
        public static implicit operator bool(Visible visible) => visible._value;

        public static readonly Visible Show = new Visible(true);
        public static readonly Visible Hide = new Visible(false);

        private readonly bool _value;
        private Visible(bool value) => _value = value;
    }

    public abstract class ToSystem<T>
    {
        public T ToSystemValue() => SystemValue();
        protected abstract T SystemValue();
    }
}
