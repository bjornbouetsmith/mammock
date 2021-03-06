﻿using Xunit;
using Mammock.Exceptions;

namespace Mammock.Tests
{

    public class PropertySetterFixture
    {
        [Fact]
        public void Setter_Expectation_With_Custom_Ignore_Arguments()
        {
            MockRepository mocks = new MockRepository();

            IBar bar = mocks.StrictMock<IBar>();

            using (mocks.Record())
            {
                Expect.Call(bar.Foo).SetPropertyAndIgnoreArgument();
            }

            using (mocks.Playback())
            {
                bar.Foo = 2;
            }

            mocks.VerifyAll();
        }

        [Fact]
        public void Setter_Expectation_Not_Fullfilled()
        {
            MockRepository mocks = new MockRepository();

            IBar bar = mocks.StrictMock<IBar>();

            using (mocks.Record())
            {
                Expect.Call(bar.Foo).SetPropertyAndIgnoreArgument();
            }

            ExpectationViolationException ex = Assert.Throws<ExpectationViolationException>(() =>
            {
                using (mocks.Playback())
                {
                }
            });

            Assert.Equal("IBar.set_Foo(any); Expected #1, Actual #0.", ex.Message);
        }

        [Fact]
        public void Setter_Expectation_With_Correct_Argument()
        {
            MockRepository mocks = new MockRepository();

            IBar bar = mocks.StrictMock<IBar>();

            using (mocks.Record())
            {
                Expect.Call(bar.Foo).SetPropertyWithArgument(1);
            }

            using (mocks.Playback())
            {
                bar.Foo = 1;
            }

            mocks.VerifyAll();
        }

        [Fact]
        public void Setter_Expectation_With_Wrong_Argument()
        {
            MockRepository mocks = new MockRepository();

            IBar bar = mocks.StrictMock<IBar>();

            using (mocks.Record())
            {
                Expect.Call(bar.Foo).SetPropertyWithArgument(1);
            }

            mocks.Playback();
            string expectedMessage = "IBar.set_Foo(0); Expected #0, Actual #1.\r\nIBar.set_Foo(1); Expected #1, Actual #0.";
            ExpectationViolationException ex = Assert.Throws<ExpectationViolationException>(() => { bar.Foo = 0; });
            Assert.Equal(expectedMessage, ex.Message);
        }
    }

    public interface IBar
    {
        int Foo { get; set; }
    }
}