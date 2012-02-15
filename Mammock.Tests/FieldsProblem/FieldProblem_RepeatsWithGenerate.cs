using Xunit;
using Mammock.Exceptions;

namespace Mammock.Tests.FieldsProblem
{

	
	public class FieldProblem_RepeatsWithGenerate
	{
		[Fact]
        public void RepeatTimes_Fails_When_Called_More_Then_Expected()
        {
            var mockRepository = new MockRepository();

		    var interfaceMock = mockRepository.StrictMock<IRepeatsWithGenerate>();

            Expect.Call(interfaceMock.GetMyIntValue())
                .Repeat.Times(1)
		        .Return(4);

            mockRepository.ReplayAll();

		    interfaceMock.GetMyIntValue();
		    
			string expectedMessage="IRepeatsWithGenerate.GetMyIntValue(); Expected #1, Actual #2.";
ExpectationViolationException ex = Assert.Throws<ExpectationViolationException>(
														 () => interfaceMock.GetMyIntValue());
Assert.Equal(expectedMessage, ex.Message);
        }

		[Fact]
        public void RepeatTimes_Works_When_Called_Less_Then_Expected()
        {
		    var mockRepository = new MockRepository();

            var interfaceMock = mockRepository.StrictMock<IRepeatsWithGenerate>();

            Expect.Call(interfaceMock.GetMyIntValue())
                .Repeat.Times(2)
		        .Return(4);

            mockRepository.ReplayAll();

		    interfaceMock.GetMyIntValue();

			string expectedMessage="IRepeatsWithGenerate.GetMyIntValue(); Expected #2, Actual #1.";
ExpectationViolationException ex = Assert.Throws<ExpectationViolationException>(
													 () => mockRepository.Verify(interfaceMock));
Assert.Equal(expectedMessage, ex.Message);
   
        }
	}

	public interface IRepeatsWithGenerate
	{
	    int GetMyIntValue();
	}
}