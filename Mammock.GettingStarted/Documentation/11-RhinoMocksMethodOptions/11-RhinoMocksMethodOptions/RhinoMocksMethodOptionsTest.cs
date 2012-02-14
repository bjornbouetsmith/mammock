﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace RhinoMocksMethodOptions
{
    /// <summary>
    /// The IMethodOptions allows you to set various options on a method call.
    /// </summary>
    /// <see cref="http://www.ayende.com/wiki/Rhino+Mocks+Method+Options+Interface.ashx"/>
    [TestFixture]
    public class RhinoMocksMethodOptionsTest
    {
        public MockRepository mocks { get; set; }
        public IProjectView view {get; set;}

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            view = mocks.StrictMock<IProjectView>();
        }

        /// <summary>
        /// The return value of a method, if it has one.
        /// </summary>
        [Test]
        public void setReturnValueOneMethod()
        {
            Expect.Call(this.view.Ask(null, null)).Return(null);

            mocks.ReplayAll();

            Assert.AreEqual(null, view.Ask(null, null));

            mocks.VerifyAll();
        }

        /// <summary>
        /// The return value of a method, if it has one with AAA syntax.
        /// </summary>
        [Test]
        public void setReturnValueOneMethod_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v => view.Ask(null, null)).Return(null);           

            //Act
            object obj = this.view.Ask(null, null);

            //Assert
            Assert.AreEqual(null, obj);
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// The exception the method will throw:
        /// </summary>
        [Test]
        [ExpectedException(typeof(Exception))]
        public void setThrowException()
        {
            Expect.Call(view.Ask(null, null)).Throw(new Exception("Demo"));

            mocks.ReplayAll();

            view.Ask(null, null);

            mocks.VerifyAll();
        }

        /// <summary>
        /// The exception the method will throw with AAA syntax
        /// </summary>
        [Test]
        [ExpectedException(typeof(Exception))]
        public void setThrowException_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v => v.Ask(null, null)).Throw(new Exception("Demo"));

            //Act
            view.Ask(null, null);

            //Assert
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// The number of times this method is expected to repeat (there are a number of convenience methods there):
        /// </summary>
        [Test]
        public void setReturnValueTwice()
        {
            Expect.Call(view.Ask(null, null)).Return(null).Repeat.Twice();

            mocks.ReplayAll();

            view.Ask(null, null);
            view.Ask(null, null);

            mocks.VerifyAll();
        }

        /// <summary>
        /// The number of times this method is expected to repeat 
        /// (there are a number of convenience methods there) with AAA syntax
        /// </summary>
        [Test]
        public void setReturnValueTwice_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v => v.Ask(null, null)).Return(null).Repeat.Twice();

            //Act
            this.view.Ask(null, null);
            this.view.Ask(null, null);

            //Assert
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// To ignore the method arguments:
        /// </summary>
        [Test]
        public void setReturnValueWhateverArguments()
        {
            Expect.Call(view.Ask(null, null)).Return(null).IgnoreArguments();

            mocks.ReplayAll();

            Assert.AreEqual(null, view.Ask("1", null));
         
            mocks.VerifyAll();
        }

        /// <summary>
        /// To ignore the method arguments with AAA
        /// </summary>
        [Test]
        public void setReturnValueWhateverArguments_AAA()
        {
            //Assert
            this.view.Replay();
            this.view.Expect(v => v.Ask(null, null)).Return(null).IgnoreArguments();

            //Act
            object obj = this.view.Ask("1", null);

            //Assert
            Assert.AreEqual(null, obj);
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// To set the constraints of the method:
        /// </summary>
        [Test]
        public void setReturnValueWithContraintsArguments()
        {
            Expect.Call(view.Ask(null, null)).Return(null)
                .Constraints(Rhino.Mocks.Constraints.Text.StartsWith("Some"), Rhino.Mocks.Constraints.Text.EndsWith("Text"));

            mocks.ReplayAll();

            Assert.AreEqual(null, view.Ask("Someone", "MyText"));

            mocks.VerifyAll();
        }

        /// <summary>
        /// To set the constraints of the method with AAA syntax
        /// </summary>
        [Test]
        public void setReturnValueWithContraintsArguments_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v =>v.Ask(null, null)).Return(null)
                .Constraints(Rhino.Mocks.Constraints.Text.StartsWith("Some"), Rhino.Mocks.Constraints.Text.EndsWith("Text"));

            //Act
            object obj = view.Ask("Someone", "MyText");
            
            //Assert
            Assert.AreEqual(null, obj);
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// To set the callback for this method:
        /// </summary>
        [Test]
        public void setReturnValueWithCallbackMethod()
        {
            //Expect.Call(view.Ask(null, null)).Return(null).
            //    Callback(new AskDelegate(VerifyAskArguments));

            Assert.Ignore("Not implemented");
        }

        /// <summary>
        /// To call the original method on the class:
        /// </summary>
        [Test]
        public void callOriginalMethod()
        {
            //Expect.Call(view.Ask(null,null)).
            //    CallOriginalMethod();

            Assert.Ignore("Not implemented");
        }

        /// <summary>
        /// To emulate simple property accessors on a property:
        /// </summary>
        [Test]
        public void emulateProperty()
        {
            Expect.Call(view.Name).PropertyBehavior();

            mocks.ReplayAll();

            view.Name = "Ayende";
            Assert.AreEqual("Ayende", view.Name);

            mocks.VerifyAll();
        }

        /// <summary>
        /// To emulate simple property accessors on a property:
        /// </summary>
        [Test]
        public void emulateProperty_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v => v.Name).PropertyBehavior();

            //Act
            view.Name = "Ayende";

            //Assert
            Assert.AreEqual("Ayende", view.Name);
            this.view.VerifyAllExpectations();
        }

        /// <summary>
        /// Used as Delegate to controlProgrammaticallyReturn
        /// </summary>
        /// <param name="first"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
        delegate string ViewAskDelegate(string first, string surname);

        /// <summary>
        /// To control programmatically what the method call will return or throw (see The Do() Handler):
        /// </summary>
        [Test]
        public void controlProgrammaticallyReturn()
        {
            Expect.Call(view.Ask(null, null)).
                Do((ViewAskDelegate)delegate(string s1, string s2) { return s1 + " " + s2; }).
                IgnoreArguments();

            mocks.ReplayAll();

            Assert.AreEqual("Ayende Rahien", view.Ask("Ayende", "Rahien"));

            mocks.VerifyAll();
        }

        /// <summary>
        /// To control programmatically what the method call
        /// will return or throw (see The Do() Handler)with AAA syntax
        /// </summary>
        [Test]
        public void controlProgrammaticallyReturn_AAA()
        {
            //Arrange
            this.view.Replay();
            this.view.Expect(v => v.Ask(null, null)).
                Do((ViewAskDelegate)delegate(string s1, string s2) { return s1 + " " + s2; }).
                IgnoreArguments();

            //Act
            string sentence = view.Ask("Ayende", "Rahien").ToString();

            //Assert
            Assert.AreEqual("Ayende Rahien", sentence);
            this.view.VerifyAllExpectations();
        }
    }
}
