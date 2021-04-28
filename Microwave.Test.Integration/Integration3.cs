using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

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

        private IButton _powerbutton = new Button();
		private IButton _timebutton = new Button();
		private IButton _startcancelbutton = new Button();
		private IDoor _door = new Door();
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

            _powerbutton = new Button();
			_timebutton = new Button();
			_startcancelbutton = new Button();
			_door = new Door();
			_display = new Display(_output);
			_light = new Light(_output);
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
        public void Test1()
        {
            Assert.Pass();
        }
    }
}