using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

/*
    T: UI
    X: CookCtrl, Door, Button, Display, Light, Timer, PowerTube, Output
    S: 
*/

namespace Microwave.Test.Integration
{
    public class IT7_FullIntegration
    {
        private UserInterface userI;

        private CookController CookCtrl;
        private IDisplay display;
        private IDoor door;
        private IPowerTube PT;
        private IOutput output;

        private IButton pwrBtn, timBtn, strtCnlBtn;
        private ILight light;
        private ITimer timer;

        [SetUp]
        public void Setup()
        {
            // Integrated
            pwrBtn = new Button();
            timBtn = new Button();
            strtCnlBtn = new Button();
            output = new Output();
            light = new Light(output);
            timer = new Timer();
            door = new Door();
            display = new Display(output);
            PT = new PowerTube(output);
            CookCtrl = new CookController(timer, display, PT);
            // Test
            userI = new UserInterface(pwrBtn, timBtn, strtCnlBtn, door, display, light, CookCtrl);

            CookCtrl.UI = userI;

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}