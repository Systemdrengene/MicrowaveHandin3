using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, PowerTube, Output
    S: Light, Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration5
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
            _light = Substitute.For<ILight>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _sut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _sut;
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonWhileReady_DisplayShowsDefaultPowerLevel()
        {
            _powerButton.Press();

            _output.Received(1)
                .OutputLine(Arg.Is<string>(str => 
                    str.Contains("50")));
        }

        [Test]
        public void PowerButtonPress_PressPowerButton3Times_DisplayShows150Power()
        {
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();

            _output.Received(1)
                .OutputLine(Arg.Is<string>(str =>
                    str.Contains("150")));
        }

        [Test]
        public void PowerButtonPress_PressPowerButtonTooManyTimes_DisplayShows700Power()
        {
            for(int i = 0; i < 20; i++)
                _powerButton.Press();

            _output.Received(1)
                .OutputLine(Arg.Is<string>(str =>
                    str.Contains("700")));
        }
    }
}