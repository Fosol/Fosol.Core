using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fosol.Core.Test.Initialization
{
    [TestClass]
    public class DefaultsContainerTest
    {
        #region Variables
        #endregion

        #region Methods
        [TestInitialize]
        public void DefaultsContainerInitialize()
        {
        }

        [TestMethod]
        public void DefaultsContainer_Configure()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Add<IPerson>(new Person()); });

            Assert.IsFalse(container.IsInitialized);
        }

        [TestMethod]
        public void DefaultsContainer_Initialize()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Initialize();

            Assert.IsTrue(container.IsInitialized);
        }

        [TestMethod]
        public void DefaultsContainer_InitializeAfterResolve()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();

            try
            {
                var value = container.Resolve<int>();
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(container.IsInitialized);
            }
        }

        [TestMethod]
        public void DefaultsContainer_ConfigureAdd()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Add<IPerson>(new Person()); });
            var p = container.Resolve<IPerson>();

            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void DefaultsContainer_ConfigureSet()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Set<IPerson>(new Person()); });
            var p = container.Resolve<IPerson>();

            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void DefaultsContainer_Resolve()
        {
            var container = new Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Set<IPerson>(new Person()); });
            var p = container.Resolve<Person>(typeof(IPerson));

            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void DefaultsContainer_Resolve_Fail()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Set<IPerson>(new Person()); });

            Person value = null;
            try
            {
                value = container.Resolve<Person>();
                Assert.Fail();
            }
            catch
            {
                Assert.IsNull(value);
            }
        }

        [TestMethod]
        public void DefaultsContainer_Resolve_Fail1()
        {
            var container = new Fosol.Core.Initialization.DefaultsContainer();
            container.Configure(c => { c.Set<IPerson>(new Person()); });

            HashCode value = null;
            try
            {
                value = container.Resolve<HashCode>();
                Assert.Fail();
            }
            catch
            {
                Assert.IsNull(value);
            }
        }
        #endregion

        #region Classes
        private class Person
            : IPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private interface IPerson
        {
            string FirstName { get; set; }
            string LastName { get; set; }
        }
        #endregion
    }
}
