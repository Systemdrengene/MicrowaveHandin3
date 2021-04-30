using System;
using System.Runtime.InteropServices;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Exceptions;

/*
    T: CookCtrl
    X: Display, PowerTube, Output
    S: Timer
*/

namespace Microwave.Test.Integration
{
    public class Integration1
    {
        private IOutput _output;

        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _stubbedTimer;
        private IUserInterface _stubbedUI;

        private CookController _sut;

        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _stubbedUI = Substitute.For<IUserInterface>();
            _stubbedTimer = Substitute.For<ITimer>();

            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _sut = new CookController(_stubbedTimer, _display, _powerTube, _stubbedUI);
        }

        // Power skal være 50 - 700
        [TestCase(50, 1)]
        [TestCase(51, 1)]
        [TestCase(400, 1)]
        [TestCase(699, 1)]
        [TestCase(700, 1)]
        public void StartCooking_SetPowerAndTime_PowerTubeIsOn(int power, int time)
        {
            // Arrange

            // Act
            _sut.StartCooking(power, time);
            // Assert 
            //output.Received(1).OutputLine($"PowerTube works with {power} %");

            _output.Received(1).OutputLine(Arg.Is<string>(str => 
                str.Contains(power.ToString())
            ));
        }

        // Power out of range (Exception)
        [TestCase(1000, 1)]
        [TestCase(701, 1)]
        [TestCase(49, 1)]
        [TestCase(-5, 1)]
        public void StartCooking_SetPowerAndTime_PowerExceptionOOR(int power, int time)
        {
            // Arrange

            // Act
            
            // Assert 
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.StartCooking(power, time));
        }

        // Test Display
        [Test]
        public void Display_ThreeSecondsRemaining_OutputWrite()
        {
            _sut.StartCooking(50, 115);
            _stubbedTimer.TimeRemaining.Returns(115);
            _stubbedTimer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine($"Display shows: 01:55");
        }
        [Test]
        public void Stop_StopTheCookingWhileOn_CookingIsStopped()
        {
            // Act
            _sut.StartCooking(50, 50);
            _sut.Stop();
            // Assert
            _output.Received(1).OutputLine("PowerTube turned off");
        }
        [Test]
        public void Stop_StopTheCookingWhileOff_CookingIsStopped()
        {
            _sut.Stop();

            _output.Received(0).OutputLine("PowerTube turned off");
        }

        [Test]
        public void OnTimerExpired_eh_eh()
        {

        }
    }
}