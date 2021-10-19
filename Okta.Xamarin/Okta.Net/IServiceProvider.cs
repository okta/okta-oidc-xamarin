using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net
{
	public interface IServiceProvider
	{
		void RegisterService<IType, CType>()
			where IType : class 
			where CType : class, IType;

		void RegisterService<IType>(IType implementation) where IType : class;

		IType GetService<IType>() where IType : class;
	}
}
