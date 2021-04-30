using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, Light, PowerTube, Output
    S: Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration6
    {
        private IUserInterface _sut;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private IOutput _output;
        private ILight _light;
        private IDoor _door;

        private CookController _cookController;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;

        [SetUp]
        public void Setup()
        {
	        _output = Substitute.For<IOutput>();
	        _timer = Substitute.For<ITimer>();

	        _display = new Display(_output);
	        _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _door = new Door();
            _light = new Light(_output);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _sut = new UserInterface(_powerButton, _timeButton, 
	            _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _sut;
        }

        [Test]
        public void LightTurnOn_RecieveOnDoorOpenEvent_LightTurnsOn()
        {
            _sut.OnDoorOpened(this,EventArgs.Empty); 
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void LightTurnOff_RecieveOnDoorClosedEvent_Light()
        {
	        _sut.OnDoorOpened(this, EventArgs.Empty); // Open Door = myState er DOOROPEN
            _sut.OnDoorClosed(this, EventArgs.Empty); 
            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnPowerPressed_DefaultPower_DisplayShowsPower()
        {
            _sut.OnPowerPressed(this,EventArgs.Empty);
            _output.Received(1).OutputLine($"Display shows: 50 W");
        }

        [Test]
        public void OnTimePressed_DisplayShowsTime()
        {
	        _sut.OnPowerPressed(this, EventArgs.Empty); // State.SetPower
	        _sut.OnTimePressed(this, EventArgs.Empty); // State.SetTime (Default time 1 min)
	        _sut.OnTimePressed(this, EventArgs.Empty); // Show time with 1 min added (2 min total)

            _output.Received(1).OutputLine($"Display shows: 02:00");

        }

        [Test]
        public void OnStartCancelPressed_DisplayClear()
        {
	        _sut.OnPowerPressed(this, EventArgs.Empty); // State.SetPower
            _sut.OnStartCancelPressed(this, EventArgs.Empty);
            _output.Received(1).OutputLine($"Display cleared");
        }

    }
}