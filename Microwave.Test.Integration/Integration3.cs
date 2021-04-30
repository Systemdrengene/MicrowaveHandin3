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
        private IOutput output;
        private ITimer timer;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;
		private IDoor door;
		private IDisplay display;
		private ILight light;
		private ICookController cookerCtrl;
        private IPowerTube powerTube;

        private UserInterface sut;

		[SetUp]
        public void Setup()
		{
            output = Substitute.For<IOutput>();
            timer = Substitute.For<ITimer>();
            door = Substitute.For<IDoor>();
            light = Substitute.For<ILight>();

            powerButton = Substitute.For<Button>();
			timeButton = Substitute.For<Button>();
            startCancelButton = Substitute.For<Button>();
            display = new Display(output);
            powerTube = new PowerTube(output);
            cookerCtrl = new CookController(timer, display, powerTube);

            sut = new UserInterface
                (
                    powerButton,
                    timeButton,
                    startCancelButton,
                    door,
                    display,
                    light,
                    cookerCtrl
                );
        }

        [Test]
        public void OnStartCancelPressed_StatesIsSETTIME_PowerSetTo50TimerSetTo60()
        {
            sut.OnPowerPressed(this, EventArgs.Empty);
            sut.OnTimePressed(this, EventArgs.Empty);
            sut.OnStartCancelPressed(this, EventArgs.Empty);

            output.Received(1).OutputLine($"PowerTube works with 50");
            timer.Received(1).Start(60);
        }

        [Test]
        public void OnStartCancelPressed_StatesIsCOOKING_PowerTubeAndTimerStopped()
        {
            sut.OnPowerPressed(this, EventArgs.Empty);
            sut.OnTimePressed(this, EventArgs.Empty);
            sut.OnStartCancelPressed(this, EventArgs.Empty);
            sut.OnStartCancelPressed(this, EventArgs.Empty);

            output.Received(1).OutputLine($"PowerTube turned off");
            timer.Received(1).Stop();
        }

        [Test]
        public void OnDoorOpened_StatesIsCOOKING_PowerTubeAndTimerStopped()
        {
            sut.OnPowerPressed(this, EventArgs.Empty);
            sut.OnTimePressed(this, EventArgs.Empty);
            sut.OnStartCancelPressed(this, EventArgs.Empty);
            sut.OnDoorOpened(this, EventArgs.Empty);

            output.Received(1).OutputLine($"PowerTube turned off");
            timer.Received(1).Stop();
        }
    }
}