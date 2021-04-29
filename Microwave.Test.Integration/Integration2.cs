using System;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

/*
    T: CookCtrl
    X: Display, Timer, PowerTube, Output
    S:
*/
namespace Microwave.Test.Integration
{
    public class Integration2
    {
        private IOutput _output;

        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IUserInterface _stubbedUI;

        private CookController _sut;

        [SetUp]
        public void Setup()
        {
	        _output = Substitute.For<IOutput>();
            _stubbedUI = Substitute.For<IUserInterface>();

            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _sut = new CookController(_timer, _display, _powerTube, _stubbedUI);
		}

        [Test]
        public void StartCooking_4SecondsInputWaitSecond_OneSecondLessRemaining()
        {


            _sut.StartCooking(20,4000);
            Thread.Sleep(1300);
            _sut.Stop();
            Assert.That(_timer.TimeRemaining, Is.EqualTo(3000));
        }

        [Test]
        public void StartCookingTwoSeconds_WaitTwoSeconds_TimerRemainingIsZero()
        {
            _sut.StartCooking(20,2000);
            Thread.Sleep(2300);
            Assert.That(()=> _timer.TimeRemaining == 0);
        }

        [Test]
        public void StartCookingOneSeconds_WaitForStopping_EventsAreSend()
        {
            _sut.StartCooking(20,1000);

	        Thread.Sleep(1400); //Vent til timer slut 100 ms over

            _output.Received(1).OutputLine(Arg.Is<string>(str =>str.Contains("PowerTube turned off")));
        }

        [Test]
        public void StartCookingTwoSeconds_DisplayShowTime()
        {
            _sut.StartCooking(20,2000);
            
            Thread.Sleep(2300);
            
            //Test om Powertube slukkes efter tid gået, og UI clear display
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 00:00")));

        }

    }
}