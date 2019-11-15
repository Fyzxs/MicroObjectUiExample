using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WinformsUI
{
    public partial class Form1 : Form, IMainForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            IMainForm mainForm = this;
            //If valid
            if (txtUserName.Text == "Quinn")
            {
                //  UserName/password return User

                //  hide Controls
                mainForm.LogInControlsVisibility().Hide();

                //  show controls
                mainForm.WelcomeLabelVisibility().Show();

                //  set values
                mainForm.WelcomeLabelWriter().Write("Welcome Quinn");
            }
            else
            {
                //  Show error
                mainForm.ErrorLabelVisibility().Show();

                //Clear password
                mainForm.PasswordTextBoxWriter().Write(string.Empty);
            }
        }

        public IVisibility UserNameLabelVisibility() => new ControlVisibility(lblUserName);
        public IVisibility UserNameTextBoxVisibility() => new ControlVisibility(txtUserName);
        public IVisibility PasswordLabelVisibility() => new ControlVisibility(txtUserName);
        public IVisibility PasswordTextBoxVisibility() => new ControlVisibility(txtPassword);
        public IWriteText PasswordTextBoxWriter() => new ControlWriteText(txtPassword);
        public IVisibility ErrorLabelVisibility() => new ControlVisibility(lblError);
        public IVisibility LogInButtonVisibility() => new ControlVisibility(btnLogIn);
        public IVisibility WelcomeLabelVisibility() => new ControlVisibility(lblWelcome);
        public IWriteText WelcomeLabelWriter() => new ControlWriteText(lblWelcome);
        public IVisibility LogInControlsVisibility() => new LogInControlsVisibility(this);
    }

    internal sealed class ControlWriteText : IWriteText
    {
        private readonly Control _control;

        public ControlWriteText(Control control) => _control = control;

        public void Write(string text) => _control.Text = text;
    }

    public interface IWriteText {
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

    internal interface IMainForm {
        IVisibility UserNameLabelVisibility();
        IVisibility UserNameTextBoxVisibility();
        IVisibility PasswordLabelVisibility();
        IVisibility PasswordTextBoxVisibility();
        IVisibility ErrorLabelVisibility();
        IVisibility LogInButtonVisibility();
        IVisibility WelcomeLabelVisibility();
        IWriteText WelcomeLabelWriter();
        IVisibility LogInControlsVisibility();
        IWriteText PasswordTextBoxWriter();
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
}
