using NUnit.Framework;

namespace MK.Tests
{
    public class Test
    {
        private Service.Service _service;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _service = new Service.Service();
        }

        [TestFixtureTearDown]
        public void FixtureTeadDown()
        {
            _service.Dispose();
        }

        [Test]
        public void get_questions()
        {
            var qs = _service.GetQuestions(5);

            Assert.That(qs.Count, Is.EqualTo(5));
        }
    }
}
