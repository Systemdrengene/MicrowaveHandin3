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
            Thread.Sleep(1000);
            _sut.Stop();
            Assert.That(_timer.TimeRemaining, Is.EqualTo(4000));
            
            //Assert.AreEqual(_timer.Enabled, true);
            //Assert.AreEqual(_timer.Timer, time);
        }

        [Test]
        public void StartCookingTwoSeconds_Stopping_EventsAreSend()
        {
            _sut.StartCooking(20,2);

            
        }

        [Test]
        public void StartCooking_StopTurnedOnTimer_TimerIsTurnedOf()
        {
            _sut.StartCooking(1,1);
            _sut.Stop();
            

        }

        [Test]
        public void StartCooking_StopNotTunredOnTimer_TimerIsTurnedOf()
        {

        }
    }
}