using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.IntefaceFramework.WebApi.assembly
{
	public class Test2
	{

		public string SaveData(Test2MainModel MainModel)
		{
		 return "成功！";
	
		}



	}


	
public class Test2MainModel
{
			public string  billID {get;set;}
			public string  Code {get;set;}
			public string  Name {get;set;}
			public List<Test2DataSubModel>  Data {get;set;}
			
}













	public class Test2DataSubModel
	{
			public string  DTLCODE {get;set;}
			public string  DTLNAME {get;set;}
		}







 



}
