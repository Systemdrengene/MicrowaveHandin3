using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

/*
    T: UI
    X: CookCtrl, Display, PowerTube, Output
    S: Button, Door, Light, Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration3
    {
        private IOutput _output;
        private ITimer _timer;

        private IButton _powerbutton;
        private IButton _timebutton;
        private IButton _startcancelbutton;
		private IDoor _door;
		private IDisplay _display;
		private ILight _light;
		private ICookController _cooker;
        private IPowerTube _powertube;

        private UserInterface _sut;

		[SetUp]
        public void Setup()
		{
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _door = Substitute.For<IDoor>();
            _light = Substitute.For<ILight>();

            _powerbutton = Substitute.For<Button>();
			_timebutton = Substitute.For<Button>();
            _startcancelbutton = Substitute.For<Button>();
            _display = new Display(_output);
            _powertube = new PowerTube(_output);
            _cooker = new CookController(_timer, _display, _powertube);

            _sut = new UserInterface
                (
                    _powerbutton,
                    _timebutton,
                    _startcancelbutton,
                    _door,
                    _display,
                    _light,
                    _cooker
                );
        }

        [Test]
        public void OnStartCancelPressed_StatesIsSETTIME_PowerSetTo50TimerSetTo60()
        {
            _sut.OnPowerPressed(this, EventArgs.Empty);
            _sut.OnTimePressed(this, EventArgs.Empty);
            _sut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine($"PowerTube works with 50");
            _timer.Received(1).Start(60);
        }

        [Test]
        public void OnStartCancelPressed_StatesIsCOOKING_PowertubeAndTimerStopped()
        {
            _sut.OnPowerPressed(this, EventArgs.Empty);
            _sut.OnTimePressed(this, EventArgs.Empty);
            _sut.OnStartCancelPressed(this, EventArgs.Empty);
            _sut.OnStartCancelPressed(this, EventArgs.Empty);

            _output.Received(1).OutputLine($"PowerTube turned off");
            _timer.Received(1).Stop();
        }

        [Test]
        public void OnDoorOpened_StatesIsCOOKING_PowertubeAndTimerStopped()
        {
            _sut.OnPowerPressed(this, EventArgs.Empty);
            _sut.OnTimePressed(this, EventArgs.Empty);
            _sut.OnStartCancelPressed(this, EventArgs.Empty);
            _sut.OnDoorOpened(this, EventArgs.Empty);

            _output.Received(1).OutputLine($"PowerTube turned off");
            _timer.Received(1).Stop();
        }
    }
}