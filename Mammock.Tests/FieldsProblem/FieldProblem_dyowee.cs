using System;
using System.Collections.Generic;
using System.Text;
using ADODB;
using Xunit;

namespace Mammock.Tests.FieldsProblem
{
	
	public class FieldProblem_dyowee
	{
        [Fact(Skip = "System.TypeLoadException: Could not load type 'Castle.Proxies.RecordsetProxy' from assembly 'DynamicProxyGenAssembly2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. The type is marked as eligible for type equivalence, but either it has generic parameters, or it is not a structure, COM imported interface, enumeration, or delegate.")]
		public void MockingRecordSet()
		{
            //TODO: Figure out why this does not work.

            MockRepository mr = new MockRepository();
            Recordset mock = mr.StrictMock<ADODB.Recordset>();
            Assert.NotNull(mock);
            Expect.Call(mock.ActiveConnection).Return("test");
            mr.ReplayAll();
            Assert.Equal("test", mock.ActiveConnection);
            mr.VerifyAll();
		}
	}
}
