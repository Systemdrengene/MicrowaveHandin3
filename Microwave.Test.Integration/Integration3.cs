using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
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
        private IButton _powerbutton = new Button();
		private IButton _timebutton = new Button();
		private IButton _startcancelbutton = new Button();
		private IDoor _door = new Door();
		private IDisplay _display;
		private ILight _light;
		ICookController _cooker;

		[SetUp]
        public void Setup()
		{
			_powerbutton = new Button();
			_timebutton = new Button();
			_startcancelbutton = new Button();
			_door = new Door();
			_display = new Display();
			_light = new Light();
			_cooker = new CookController();


            public UserInterface(
                IButton powerButton,
                IButton timeButton,
                IButton startCancelButton,
                IDoor door,
                IDisplay display,
                ILight light,
                ICookController cooker)

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}