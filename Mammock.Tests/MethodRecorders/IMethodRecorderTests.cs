﻿#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion


using System;
using System.Reflection;
using Xunit;
using Mammock.Expectations;
using Mammock.Impl;
using Mammock.Interfaces;
using Mammock.Generated;
using Mammock.Tests.Expectations;

namespace Mammock.Tests.MethodRecorders
{
	public abstract class IMethodRecorderTests
	{
		private MockRepository mocks;
		protected IDemo demo;
		protected MethodInfo voidNoArgs;
		protected AnyArgsExpectation expectationOne,expectationTwo;
		protected IMethodRecorder recorder;
		protected MethodInfo voidThreeArgs;

		public IMethodRecorderTests()
		{
			mocks = new MockRepository();
			demo = this.mocks.StrictMock(typeof (IDemo)) as IDemo;
			voidNoArgs = typeof (IDemo).GetMethod("VoidNoArgs");
			voidThreeArgs = typeof (IDemo).GetMethod("VoidThreeStringArgs");
			expectationOne = new AnyArgsExpectation(new FakeInvocation(this.voidNoArgs), new Range(1, 1));
			expectationTwo = new AnyArgsExpectation(new FakeInvocation(voidThreeArgs), new Range(1, 1));
			recorder = CreateRecorder();
			ChildSetup();
		}

		[Fact]
		public void HasExpectationsStartsEmpty()
		{
			Assert.False(recorder.HasExpectations);
		}

		[Fact]
		public void HasExpectationsAfterAddingExpectation()
		{
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			Assert.True(recorder.HasExpectations);
		}

		[Fact]
		public void HasExpectationsAfterGettingRecordedExpectation()
		{
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			recorder.GetRecordedExpectation(new FakeInvocation(voidNoArgs),demo, voidNoArgs, new object[0]);
			Assert.False(recorder.HasExpectations);
		}

		[Fact]
		public void GetAllExpectationForProxyAndMethod()
		{
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			recorder.Record(this.demo, this.voidThreeArgs, expectationOne);
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);

			ExpectationsList expectations = recorder.GetAllExpectationsForProxyAndMethod(demo, voidNoArgs);
			Assert.Equal(2, expectations.Count);
			expectations = recorder.GetAllExpectationsForProxyAndMethod(demo, voidThreeArgs);
			Assert.Equal(1, expectations.Count);
		}

		[Fact]
		public void GetAllExpectationsForProxy()
		{
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			recorder.Record(this.demo, this.voidThreeArgs, expectationOne);
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			ExpectationsList expectations = recorder.GetAllExpectationsForProxy(demo);
			Assert.Equal(3, expectations.Count);

		}


		[Fact]
		public void ReplaceExpectation()
		{
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			AnyArgsExpectation newExpectation = new AnyArgsExpectation(new FakeInvocation(voidNoArgs), new Range(1, 1));
			recorder.ReplaceExpectation(demo, voidNoArgs, expectationOne, newExpectation);
			ExpectationsList list = recorder.GetAllExpectationsForProxyAndMethod(demo, voidNoArgs);
			Assert.Same(newExpectation, list[0]);
		}

		[Fact]
		public void ReplaceExpectationWhenNestingOrdering()
		{
			recorder.AddRecorder(CreateRecorder());
			recorder.Record(this.demo, this.voidNoArgs, expectationOne);
			
			AnyArgsExpectation newExpectation = new AnyArgsExpectation(new FakeInvocation(voidNoArgs), new Range(1, 1));
			recorder.ReplaceExpectation(demo, voidNoArgs, expectationOne, newExpectation);
			ExpectationsList list = recorder.GetAllExpectationsForProxyAndMethod(demo, voidNoArgs);
			Assert.Same(newExpectation, list[0]);
		}

		[Fact]
		public void RecordProxyNullThrows()
		{
			string expectedMessage="Value cannot be null.\r\nParameter name: proxy";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => this.recorder.Record(null, voidNoArgs, expectationOne));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void RecordMethodNullThrows()
		{
			string expectedMessage="Value cannot be null.\r\nParameter name: method";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
			                                     () => recorder.Record(demo, null, expectationOne));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void RecordArgsNullThrows()
		{
			string expectedMessage="Value cannot be null.\r\nParameter name: expectation";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.Record(demo, voidNoArgs, null));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void WasRecordedProxyNullThrows()
		{
			recorder.Record(demo, voidNoArgs, expectationOne);
			string expectedMessage="Value cannot be null.\r\nParameter name: proxy";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.GetRecordedExpectation(new FakeInvocation(voidNoArgs), null, voidNoArgs, new object[0]));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void WasRecordedMethodNullThrows()
		{
			recorder.Record(demo, voidNoArgs, expectationOne);
			string expectedMessage="Value cannot be null.\r\nParameter name: method";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.GetRecordedExpectation(new FakeInvocation(null), demo, null, new object[0]));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void WasRecordedArgsNullThrows()
		{
			recorder.Record(demo, voidNoArgs, expectationOne);
			string expectedMessage="Value cannot be null.\r\nParameter name: args";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.GetRecordedExpectation(new FakeInvocation(voidNoArgs), demo, voidNoArgs, null));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void GetAllExpectationsMethodNullThrows()
		{
			string expectedMessage="Value cannot be null.\r\nParameter name: method";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.GetAllExpectationsForProxyAndMethod(demo, null));
Assert.Equal(expectedMessage, ex.Message);
		}

		[Fact]
		public void GetAllExpectationsProxyNullThrows()
		{
			string expectedMessage="Value cannot be null.\r\nParameter name: proxy";
ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
				() => recorder.GetAllExpectationsForProxyAndMethod(null, voidNoArgs));
Assert.Equal(expectedMessage, ex.Message);

		}

		protected abstract IMethodRecorder CreateRecorder();

		protected virtual void ChildSetup()
		{
		}
	}
}