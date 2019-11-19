using System;
using System.Security.Cryptography.X509Certificates;
using ExtractedCore;

namespace ConsoleUi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConsoleLogIn consoleLogIn = new MainForm();
            consoleLogIn.LogIn();
        }
    }


    public sealed class Initialize_LogInAction:ILogInAction
    {
        private readonly ILogInAction _nextAction;

        public Initialize_LogInAction():this(new StartMessage_LogInAction(new StopMessage_LogInAction(new Authenticate_LogInAction()))){}

        private Initialize_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;

        public void Act(IMainForm mainForm) => _nextAction.Act(mainForm);
    }

    public sealed class StopMessage_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;

        public StopMessage_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;

        public void Act(IMainForm mainForm)
        {
            _nextAction.Act(mainForm);
            Console.WriteLine();
            Console.WriteLine("Thank you for logging in.");
        }
    }

    public sealed class StartMessage_LogInAction : ILogInAction
    {
        private readonly ILogInAction _nextAction;

        public StartMessage_LogInAction(ILogInAction nextAction) => _nextAction = nextAction;
        public void Act(IMainForm mainForm)
        {
            Console.WriteLine("Welcome to the UI example. Prepare to log in.");
            _nextAction.Act(mainForm);
        }
    }

    public sealed class NoOpVisibility : IVisibility
    {
        public void Show() { }
        public void Hide() { }
        public void ChangeTo(Visible visible){}
    }

    internal class MainForm : IMainForm, IConsoleLogIn
    {
        private IAuthnRequest _cacheAuthnRequest;
        private readonly ILogInAction _logInAction;

        public MainForm(): this(new Initialize_LogInAction()){}

        private MainForm(ILogInAction logInAction) => _logInAction = logInAction;

        public IVisibility UserNameLabelVisibility() => new NoOpVisibility();

        public IVisibility UserNameTextBoxVisibility() => new NoOpVisibility();

        public UserName UserNameControl()//Clearly not part of the general form?
        {
            throw new NotImplementedException();
        }

        public IVisibility PasswordLabelVisibility() => new NoOpVisibility();

        public IVisibility PasswordTextBoxVisibility() => new NoOpVisibility();

        public IPassword Password() => new ConsolePassword();

        public IVisibility ErrorLabelVisibility() => new ConsoleErrorLabel();

        public IVisibility LogInButtonVisibility() => new NoOpVisibility();

        public IVisibility WelcomeLabelVisibility() => new NoOpVisibility();

        public IWriteText WelcomeLabelWriter() => new ConsoleWelcomeMessage();

        public IVisibility LogInControlsVisibility() => new ConsoleLogInControl();

        public IAuthnRequest AuthnRequest() => _cacheAuthnRequest ??= new ConsoleAuthnRequest();

        public void LogIn() => _logInAction.Act(this);
    }

    internal interface IConsoleLogIn
    {
        void LogIn();
    }

    internal sealed class ConsoleWelcomeMessage : IWriteText
    {
        public void Write(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
        }
    }

    internal sealed class ConsoleLogInControl : IVisibility
    {
        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }

        public void ChangeTo(Visible visible)
        {
            
        }
    }

    internal sealed class ConsoleErrorLabel : IVisibility {
        public void Show() => Console.WriteLine("Invalid Username/Password.");

        public void Hide(){/*no-op*/}

        public void ChangeTo(Visible visible)
        {
            if (visible) Show();
        }
    }

    internal sealed class ConsoleAuthnRequest : IAuthnRequest {
        private UserName _cacheUsername;
        public UserName UserName() => _cacheUsername ??= new ConsoleUsername();

        public IPassword Password() => new ConsolePassword();
    }

    internal sealed class ConsoleUsername : UserName
    {
        private string _cache;

        protected override string SystemValue() => Cache();

        public override bool Matches(string compareTo) => Cache() == compareTo;

        private string Cache() => _cache ??= Retrieve();

        private string Retrieve()
        {
            Console.Write("Username: ");
            return Console.ReadLine();
        }
    }
    internal sealed class ConsolePassword : IPassword
    {
        private string _cache;

        public bool Matches(string compareTo) => Cache() == compareTo;

        public void Clear()//TODO: Clearly not part of "IPassword"
        {
            Console.WriteLine("IPassword shouldn't have a 'Clear'");
        }

        private string Cache() => _cache ??= Retrieve();

        private string Retrieve()
        {
            Console.Write("Password (not hidden): ");
            return Console.ReadLine();
        }
    }
}
