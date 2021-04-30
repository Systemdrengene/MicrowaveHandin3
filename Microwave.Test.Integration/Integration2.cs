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
        private IOutput output;

        private IDisplay display;
        private IPowerTube powerTube;
        private ITimer timer;
        private IUserInterface stubbedUI;

        private CookController sut;

        [SetUp]
        public void Setup()
        {
	        output = Substitute.For<IOutput>();
            stubbedUI = Substitute.For<IUserInterface>();

            timer = new Timer();
            display = new Display(output);
            powerTube = new PowerTube(output);

            sut = new CookController(timer, display, powerTube, stubbedUI);
		}

        [Test]
        public void StartCooking_4SecondsInputWait1Second_OneSecondLessRemaining()
        {
            const int time = 4;
            sut.StartCooking(50,time);

            Thread.Sleep(1300);
            
            sut.Stop();

            Assert.That(timer.TimeRemaining, Is.EqualTo(time - 1));
        }

        [Test]
        public void StartCookingTwoSeconds_WaitTwoSeconds_TimerRemainingIsZero()
        {
            const int time = 2;
            sut.StartCooking(50, time);

            Thread.Sleep(2300);

            Assert.That(() => timer.TimeRemaining == 0);
        }

        [Test]
        public void StartCookingOneSeconds_WaitForStopping_EventsAreSend()
        {
            sut.StartCooking(50,1);

	        Thread.Sleep(1400); //Vent til timer slut 100 ms over

            output.Received(1)
                .OutputLine(Arg.Is<string>( str =>
                    str.Contains("PowerTube turned off")
                ));
        }

        [Test]
        public void StartCookingTwoSeconds_WaitForStopping_DisplayShowTime()
        {
            sut.StartCooking(50,2);
            
            Thread.Sleep(2300);

            output.Received(1)
                .OutputLine( Arg.Is<string>(str => 
                    str.Contains("Display shows: 00:00")
                ));
        }

    }
}